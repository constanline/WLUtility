using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using WLUtility.Data;
using static WLUtility.Helper.DllHelper;
using static WLUtility.Helper.SocketHelper;

namespace WLUtility.Engine
{
    internal class SocketEngine
    {
        internal const int MAX_SOCKET_SERVER_COUNT = 4;
        public static readonly ProxyMapping[] ArrProxyMapping = new ProxyMapping[MAX_SOCKET_SERVER_COUNT];
        private static readonly Socket[] ArrSocketServer = new Socket[MAX_SOCKET_SERVER_COUNT];
        private static readonly Thread[] ArrThreadForward = new Thread[MAX_SOCKET_SERVER_COUNT];
        private static readonly Dictionary<int, SocketPair> DicSocketPairs = new Dictionary<int, SocketPair>();

        private readonly byte _xorByte = 0x05;

        private int _numSocket;

        internal Action<Exception> CbException;

        internal SocketEngine()
        {
            InitProxyMapping();
        }

        public bool IsRunning { get; set; }

        private static void InitProxyMapping()
        {
            ArrProxyMapping[0].SetValue("47.100.107.72", 6414, "127.0.0.1", 5000,10000);
            ArrProxyMapping[1].SetValue("47.102.211.215", 6414, "127.0.0.1", 5000, 10000);
            ArrProxyMapping[2].SetValue("47.100.116.187", 6414, "127.0.0.1", 5000, 10000);
        }

        private byte[] XorByte(IEnumerable<byte> buffer, int len)
        {
            var result = buffer.Take(len).ToArray();
            if (_xorByte == 0) return result;
            for (var i = 0; i < len; i++)
                result[i] ^= _xorByte;
            return result;
        }

        private void StopSocketPair(int socketId)
        {
            if (DicSocketPairs.ContainsKey(socketId))
            {
                var socketPair = DicSocketPairs[socketId];
                if (socketPair.LocalSocket.Connected)
                {
                    //socketPair.LocalSocket.Disconnect(false);
                    socketPair.LocalSocket.Close();
                    socketPair.LocalSocket.Dispose();
                }

                if (socketPair.RemoteSocket.Connected)
                {
                    //socketPair.RemoteSocket.Disconnect(false);
                    socketPair.RemoteSocket.Close();
                    socketPair.RemoteSocket.Dispose();
                }

                DicSocketPairs.Remove(socketId);
            }
        }

        private void SendHandler(object obj)
        {
            var socketId = (int) obj;
            while (IsRunning && !DicSocketPairs.ContainsKey(socketId)) Thread.Sleep(50);
            if (!IsRunning) return;
            var socketPair = DicSocketPairs[socketId];
            while (!socketPair.IsReceived) Thread.Sleep(50);
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.LocalSocket.Receive(data);
                    if (read > 0)
                        socketPair.RemoteSocket.Send(XorByte(data, read));
                    else
                        break;
                }
                catch (Exception)
                {
                    break;
                }

            StopSocketPair(socketId);
        }

        private void ReceiveHandler(object obj)
        {
            var socketId = (int) obj;
            while (IsRunning && !DicSocketPairs.ContainsKey(socketId)) Thread.Sleep(50);
            if (!IsRunning) return;
            var socketPair = DicSocketPairs[socketId];
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.RemoteSocket.Receive(data);
                    socketPair.IsReceived = true;
                    if (read > 0)
                        PacketAnalyzer.AnalyzePacket(XorByte(data, read), socketPair);
                    else
                        break;
                }
                catch (Exception)
                {
                    break;
                }

            StopSocketPair(socketId);
        }

        private void SocketForward(object obj)
        {
            var idx = (int) obj;
            var pm = ArrProxyMapping[idx];

            ArrSocketServer[idx] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ArrSocketServer[idx].SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            ArrSocketServer[idx].Bind(pm.GetLocalEndPoint());
            ArrSocketServer[idx].Listen(int.MaxValue);
            try
            {
                while (IsRunning)
                {
                    var curNum = Interlocked.Increment(ref _numSocket);
                    var localSocket = ArrSocketServer[idx].Accept();
                    ThreadPool.QueueUserWorkItem(SendHandler, curNum);

                    var remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    remoteSocket.Connect(pm.GetRemoteEndPoint());
                    ThreadPool.QueueUserWorkItem(ReceiveHandler, curNum);

                    DicSocketPairs.Add(curNum, new SocketPair(localSocket, remoteSocket));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException)) CbException?.Invoke(ex);
            }
        }

        internal void StartForward()
        {
            IsRunning = true;
            SetProxyMapping(ArrProxyMapping);
            for (var i = 0; i < MAX_SOCKET_SERVER_COUNT; i++)
                if (ArrProxyMapping[i].IsEnabled > 0)
                {
                    ArrThreadForward[i] = new Thread(SocketForward);
                    ArrThreadForward[i].Start(i);
                }
        }

        internal void StopForward()
        {
            IsRunning = false;
            for (var i = 0; i < MAX_SOCKET_SERVER_COUNT; i++)
            {
                ArrSocketServer[i]?.Close();
                ArrSocketServer[i] = null;
                ArrThreadForward[i]?.Abort();
                ArrThreadForward[i] = null;
            }
        }
    }
}