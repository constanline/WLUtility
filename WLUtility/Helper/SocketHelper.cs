using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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

        public static int SendPacket(SocketPair socketPair, int len, bool isSkip = false)
        {
            if (!isSkip)
            {
                len = Math.Min(SINGLE_SEND_MAX_LENGTH, len);
                LastPacket = socketPair.ListBuffer.Take(len).ToArray();
                socketPair.LocalSocket.Send(LastPacket);
            }
            socketPair.ListBuffer.RemoveRange(0, len);
            return len;
        }
    }
}
