using System.Net;
using System.Runtime.InteropServices;

namespace WLUtility.Helper
{
    internal class DllHelper

    {
        public static char[] Str2Char16(string str)
        {
            var result = new char[16];
            str.ToCharArray().CopyTo(result, 0);
            return result;
        }

        public struct ProxyMapping
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] RemoteIp;

            public ushort RemotePort;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] LocalIp;

            public ushort LocalPort;

            public int IsEnabled;

            public ProxyMapping(string remoteIp, ushort remotePort, string localIp, ushort localPort)
            {
                RemoteIp = Str2Char16(remoteIp);
                RemotePort = remotePort;

                LocalIp = Str2Char16(localIp);
                LocalPort = localPort;

                IsEnabled = 1;
            }

            public IPEndPoint GetLocalEndPoint()
            {
                //var localIpStr = new string(LocalIp).TrimEnd('\0');
                //return localIpStr == "0.0.0.0" ? new IPEndPoint(IPAddress.Any, LocalPort) : new IPEndPoint(IPAddress.Parse(localIpStr), LocalPort);
                return new IPEndPoint(IPAddress.Any, LocalPort);
            }

            public IPEndPoint GetRemoteEndPoint()
            {
                return new IPEndPoint(IPAddress.Parse(new string(RemoteIp).TrimEnd('\0')), LocalPort);
            }
        };

        [DllImport("WLHook.dll")]
        public static extern void SetTargetPid(uint dwPid);

        [DllImport("WLHook.dll")]
        public static extern void SetProxyMapping(ProxyMapping[] pmList);

        [DllImport("WLHook.dll")]
        public static extern void DecConnectionCount();

        [DllImport("WLHook.dll")]
        public static extern uint GetConnectionCount();
    }
}
