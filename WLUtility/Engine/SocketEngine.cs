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
    class SocketEngine
    {
        const int MAX_SOCKET_SERVER_COUNT = 20;

        internal Action<Exception> CbException;

        private int _numSocket;
        private bool _isRunning;
        private readonly Dictionary<int, SocketPair> _dicSocketPairs = new Dictionary<int, SocketPair>();
        private readonly Thread[] _arrThreadForward = new Thread[MAX_SOCKET_SERVER_COUNT];
        private readonly Socket[] _arrSocketServer = new Socket[MAX_SOCKET_SERVER_COUNT];
        private readonly ProxyMapping[] arrProxyMapping = new ProxyMapping[MAX_SOCKET_SERVER_COUNT];

        public bool IsRunning { get => _isRunning; set => _isRunning = value; }

        internal SocketEngine()
        {
            arrProxyMapping[0] = new ProxyMapping("47.100.116.187", 6414, "127.0.0.1", 6414);
        }

        private byte _xorByte = 0x05;
        private byte[] XorByte(byte[] buffer, int len)
        {
            var result = buffer.Take(len).ToArray();
            if(_xorByte != 0)
            {
                for (var i = 0; i < len; i++)
                {
                    result[i] ^= _xorByte;
                }
            }
            return result;
        }

        private void StopSocketPair(int socketId)
        {
            if (_dicSocketPairs.ContainsKey(socketId))
            {
                var socketPair = _dicSocketPairs[socketId];
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
                _dicSocketPairs.Remove(socketId);
            }
        }

        private void SendHandler(object obj)
        {
            var socketId = (int)obj;
            while (IsRunning && !_dicSocketPairs.ContainsKey(socketId))
            {
                Thread.Sleep(50);
            }
            if (!IsRunning) return;
            var socketPair = _dicSocketPairs[socketId];
            while (!socketPair.IsReceived)
            {
                Thread.Sleep(50);
            }
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
            {
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.LocalSocket.Receive(data);
                    if (read > 0)
                        socketPair.RemoteSocket.Send(XorByte(data, read));
                    else
                        break;
                }
                catch
                {
                    break;
                }
            }
            StopSocketPair(socketId);
        }

        private void ReceiveHandler(object obj)
        {
            var socketId = (int)obj;
            while (IsRunning && !_dicSocketPairs.ContainsKey(socketId))
            {
                Thread.Sleep(50);
            }
            if (!IsRunning) return;
            var socketPair = _dicSocketPairs[socketId];
            while (socketPair.LocalSocket.Connected && socketPair.RemoteSocket.Connected)
            {
                try
                {
                    var data = new byte[1024];
                    var read = socketPair.RemoteSocket.Receive(data);
                    socketPair.IsReceived = true;
                    if (read > 0)
                    {
                        PacketAnalyzer.AnalyzePacket(XorByte(data, read), socketPair);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }
            StopSocketPair(socketId);
        }

        private void SocketForward(object obj)
        {
            int idx = (int)obj;
            ProxyMapping pm = arrProxyMapping[idx];

            _arrSocketServer[idx] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _arrSocketServer[idx].SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            _arrSocketServer[idx].Bind(pm.GetLocalIPEP());
            _arrSocketServer[idx].Listen(int.MaxValue);
            try
            {
                while (IsRunning)
                {
                    var curNum = Interlocked.Increment(ref _numSocket);
                    var localSocket = _arrSocketServer[idx].Accept();
                    ThreadPool.QueueUserWorkItem(SendHandler, curNum);

                    var remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    remoteSocket.Connect(pm.GetRemoteIPEP());
                    ThreadPool.QueueUserWorkItem(ReceiveHandler, curNum);

                    _dicSocketPairs.Add(curNum, new SocketPair(localSocket, remoteSocket));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                {
                    CbException?.Invoke(ex);
                }
            }
        }

        internal void StartForward()
        {
            IsRunning = true;
            for (int i = 0; i < MAX_SOCKET_SERVER_COUNT; i++)
            {
                if (arrProxyMapping[i].IsEnabled > 0)
                {
                    _arrThreadForward[i] = new Thread(SocketForward);
                    _arrThreadForward[i].Start(i);
                }
            }
        }

        internal void StopForward()
        {
            IsRunning = false;
            for (var i = 0; i < MAX_SOCKET_SERVER_COUNT; i++)
            {
                _arrSocketServer[i]?.Close();
                _arrSocketServer[i] = null;
                _arrThreadForward[i]?.Abort();
                _arrThreadForward[i] = null;
            }
        }
    }
}
