using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using WLUtility.Core;
using static WLUtility.Helper.DllHelper;

namespace WLUtility.Engine
{
    internal class SocketEngine
    {
        public const int MAX_SOCKET_SERVER_COUNT = 4;

        public static readonly ProxyMappingInfo[] ArrProxyMapping = new ProxyMappingInfo[MAX_SOCKET_SERVER_COUNT];
        private static readonly Socket[] ArrSocketServer = new Socket[MAX_SOCKET_SERVER_COUNT];
        private static readonly Thread[] ArrThreadForward = new Thread[MAX_SOCKET_SERVER_COUNT];
        private static readonly Dictionary<int, BaseSocket> DicSocket = new Dictionary<int, BaseSocket>();

        private int _numSocket;

        public event Action<Exception> CbException;

        internal SocketEngine()
        {
            InitProxyMapping();
        }

        public bool IsRunning { get; set; }

        private static void InitProxyMapping()
        {
            ArrProxyMapping[0] = new ProxyMappingInfo("47.102.123.163", 6414, "127.0.0.1", 5501, 6000);
            ArrProxyMapping[1] = new ProxyMappingInfo("47.101.132.111", 6414, "127.0.0.1", 6001, 7000);
            ArrProxyMapping[2] = new ProxyMappingInfo("106.14.173.155", 6414, "127.0.0.1", 7001, 8000);
            ArrProxyMapping[3] = new ProxyMappingInfo("47.102.211.215", 6414, "127.0.0.1", 8001, 9000);
            //var flag =
            //    ArrProxyMapping[0].SetValue("47.100.107.72", 6414, "127.0.0.1", 5002, 6000) &&
            //    ArrProxyMapping[1].SetValue("47.102.211.215", 6414, "127.0.0.1", 6001, 7000) &&
            //    ArrProxyMapping[2].SetValue("47.100.116.187", 6414, "127.0.0.1", 7001, 8000);
            //if (!flag)
            //{
            //    LogHelper.Log("No available port");
            //}
        }

        public static void StopSocket(int socketId)
        {
            if (!DicSocket.ContainsKey(socketId)) return;

            var socket = DicSocket[socketId];
            socket.Close();
            DicSocket.Remove(socketId);
        }

        private void SocketForward(object obj)
        {
            try
            {
                var idx = (int)obj;
                var pm = ArrProxyMapping[idx];

                ArrSocketServer[idx] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ArrSocketServer[idx].SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                ArrSocketServer[idx].Bind(pm.GetLocalEndPoint());
                ArrSocketServer[idx].Listen(int.MaxValue);
                while (IsRunning)
                {
                    var curNum = Interlocked.Increment(ref _numSocket);
                    var localSocket = ArrSocketServer[idx].Accept();

                    var remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    remoteSocket.Connect(pm.GetRemoteEndPoint());

                    var socket = new ProxySocket(localSocket, remoteSocket) { SocketId = curNum };
                    DicSocket.Add(curNum, socket);
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException)) CbException?.Invoke(ex);
            }
        }

        internal void StartForward()
        {
            try
            {
                IsRunning = true;
                SetProxyMapping(ArrProxyMapping.Select(e => e.GetProxyMapping()).ToArray());
                for (var i = 0; i < MAX_SOCKET_SERVER_COUNT; i++)
                {
                    if (ArrProxyMapping[i].IsEnabled)
                    {
                        ArrThreadForward[i] = new Thread(SocketForward);
                        ArrThreadForward[i].Start(i);
                    }
                }
            }
            catch (Exception ex)
            {
                CbException?.Invoke(ex);
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