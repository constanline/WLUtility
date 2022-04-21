using System.Linq;
using System.Net.NetworkInformation;

namespace WLUtility.Helper
{
    internal static class SocketHelper
    {
        // public const int SINGLE_SEND_MAX_LENGTH = 1024;
        //
        // public class SocketPair
        // {
        //     public readonly Socket LocalSocket;
        //     public readonly Socket RemoteSocket;
        //     public bool IsReceived;
        //     public readonly object ObjLocker;
        //     public readonly List<byte> ListBuffer;
        //
        //     // public Global Global;
        //
        //     public Account Account;
        //
        //     public byte[] LastPacket;
        //
        //     public SocketPair(Socket localSocket, Socket remoteSocket)
        //     {
        //         LocalSocket = localSocket;
        //         RemoteSocket = remoteSocket;
        //         IsReceived = false;
        //         ObjLocker = new object();
        //         ListBuffer = new List<byte>();
        //         // Global = new Global();
        //         Account = new Account();
        //     }
        // }
        //
        // public static int SendPacket(SocketPair socketPair, byte[] buffer)
        // {
        //     if (SocketEngine.RecordPacket)
        //     {
        //         LogHelper.LogPacket(buffer, true);
        //     }
        //     return socketPair.RemoteSocket.Send(buffer);
        // }
        //
        // public static int RecvPacket(SocketPair socketPair, int len, bool isSkip = false)
        // {
        //     if (!isSkip)
        //     {
        //         if (SocketEngine.RecordPacket)
        //         {
        //             LogHelper.LogPacket(socketPair.ListBuffer.Take(len).ToArray(), false);
        //         }
        //
        //         while (len > 0)
        //         {
        //             lock (socketPair.ListBuffer)
        //             {
        //                 var currentLen = Math.Min(SINGLE_SEND_MAX_LENGTH, len);
        //                 socketPair.LastPacket = socketPair.ListBuffer.Take(currentLen).ToArray();
        //                 socketPair.LocalSocket.Send(socketPair.LastPacket);
        //                 socketPair.ListBuffer.RemoveRange(0, currentLen);
        //                 len -= currentLen;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         socketPair.ListBuffer.RemoveRange(0, len);
        //     }
        //
        //     return len;
        // }

        public static bool PortInUse(int port)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            return ipProperties.GetActiveTcpListeners().Any(endPoint => endPoint.Port == port) ||
                   ipProperties.GetActiveUdpListeners().Any(endPoint => endPoint.Port == port);
        }
    }
}
