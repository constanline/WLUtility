using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace WLUtility.Helper
{
    class SocketHelper
    {
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
            len = Math.Min(socketPair.ListBuffer.Count, len);
            if (!isSkip)
                socketPair.LocalSocket.Send(socketPair.ListBuffer.Take(len).ToArray());
            socketPair.ListBuffer.RemoveRange(0, len);
            return len;
        }
    }
}
