using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WLUtility.Engine;

namespace WLUtility.Helper
{
    internal static class SocketHelper
    {
        internal const int SINGLE_SEND_MAX_LENGTH = 1024;

        internal static byte[] LastPacket;

        internal class SocketPair
        {
            public readonly Socket LocalSocket;
            public readonly Socket RemoteSocket;
            public bool IsReceived;
            public readonly object ObjLocker;
            public readonly List<byte> ListBuffer;

            public SocketPair(Socket localSocket, Socket remoteSocket)
            {
                LocalSocket = localSocket;
                RemoteSocket = remoteSocket;
                IsReceived = false;
                ObjLocker = new object();
                ListBuffer = new List<byte>();
            }
        }

        public static int SendPacket(SocketPair socketPair, byte[] buffer)
        {
            if (SocketEngine.RecordPacket)
            {
                LogHelper.LogPacket(buffer, true);
            }
            return socketPair.RemoteSocket.Send(buffer);
        }

        public static int RecvPacket(SocketPair socketPair, int len, bool isSkip = false)
        {
            if (!isSkip)
            {
                if (SocketEngine.RecordPacket)
                {
                    LogHelper.LogPacket(socketPair.ListBuffer.Take(len).ToArray(), false);
                }

                while (len > 0)
                {
                    lock (socketPair.ListBuffer)
                    {
                        var currentLen = Math.Min(SINGLE_SEND_MAX_LENGTH, len);
                        LastPacket = socketPair.ListBuffer.Take(currentLen).ToArray();
                        socketPair.LocalSocket.Send(LastPacket);
                        socketPair.ListBuffer.RemoveRange(0, currentLen);
                        len -= currentLen;
                    }
                }
            }
            else
            {
                socketPair.ListBuffer.RemoveRange(0, len);
            }

            return len;
        }

        public static bool PortInUse(int port)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            
            return ipProperties.GetActiveTcpListeners().Any(endPoint => endPoint.Port == port) || 
                   ipProperties.GetActiveUdpListeners().Any(endPoint => endPoint.Port == port);
        }
    }
}
