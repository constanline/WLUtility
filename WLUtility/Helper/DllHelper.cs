using System.Net;
using System.Runtime.InteropServices;

namespace WLUtility.Helper
{
    internal static class DllHelper

    {
        public static char[] StrToChar16(string str)
        {
            var result = new char[16];
            str.ToCharArray().CopyTo(result, 0);
            return result;
        }

        public static string Char16ToStr(char[] chrArray)
        {
            return new string(chrArray);
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

            public void Enable()
            {
                IsEnabled = 1;
            }

            public void Disable()
            {
                IsEnabled = 0;
            }

            public bool SetValue(string remoteIp, ushort remotePort, string localIp, ushort localMinPort, ushort localMaxPort)
            {
                RemoteIp = StrToChar16(remoteIp);
                RemotePort = remotePort;

                LocalIp = StrToChar16(localIp);

                for (var i = localMinPort; i <= localMaxPort; i++)
                {
                    if (SocketHelper.PortInUse(i)) continue;

                    IsEnabled = 1;
                    LocalPort = i;
                    break;
                }

                return LocalPort != 0;
            }

            public IPEndPoint GetLocalEndPoint()
            {
                //var localIpStr = new string(LocalIp).TrimEnd('\0');
                //return localIpStr == "0.0.0.0" ? new IPEndPoint(IPAddress.Any, LocalPort) : new IPEndPoint(IPAddress.Parse(localIpStr), LocalPort);
                return new IPEndPoint(IPAddress.Any, LocalPort);
            }

            public IPEndPoint GetRemoteEndPoint()
            {
                return new IPEndPoint(IPAddress.Parse(new string(RemoteIp).TrimEnd('\0')), RemotePort);
            }
        }

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
