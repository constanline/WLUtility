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
        }

        public class ProxyMappingInfo
        {
            ProxyMapping _proxyMapping;

            public ushort LocalMinPort { get; set; }

            public ushort LocalMaxPort { get; set; }

            public ProxyMapping GetProxyMapping()
            {
                return _proxyMapping;
            }

            public string RemoteIp
            {
                get => new string(_proxyMapping.RemoteIp).Trim('\0');
                set => _proxyMapping.RemoteIp = StrToChar16(value);
            }

            public ushort RemotePort
            {
                get => _proxyMapping.RemotePort;
                set => _proxyMapping.RemotePort = value;
            }

            public string LocalIp
            {
                get => new string(_proxyMapping.LocalIp).Trim('\0');
                set => _proxyMapping.LocalIp = StrToChar16(value);
            }

            public ushort LocalPort
            {
                get => _proxyMapping.LocalPort;
                set => _proxyMapping.LocalPort = value;
            }

            public bool IsEnabled
            {
                get => _proxyMapping.IsEnabled == 1;
                set => _proxyMapping.IsEnabled = value ? 1 : 0;
            }

            public ProxyMappingInfo(string remoteIp, ushort remotePort, string localIp, ushort localMinPort, ushort localMaxPort)
            {
                _proxyMapping.RemoteIp = StrToChar16(remoteIp);
                _proxyMapping.RemotePort = remotePort;

                _proxyMapping.LocalIp = StrToChar16(localIp);

                LocalMinPort = localMinPort;
                LocalMaxPort = localMaxPort;
                for (var i = localMinPort; i <= localMaxPort; i++)
                {
                    if (SocketHelper.PortInUse(i)) continue;

                    _proxyMapping.IsEnabled = 1;
                    _proxyMapping.LocalPort = i;
                    break;
                }
            }

            public IPEndPoint GetLocalEndPoint()
            {
                //var localIpStr = new string(LocalIp).TrimEnd('\0');
                //return localIpStr == "0.0.0.0" ? new IPEndPoint(IPAddress.Any, LocalPort) : new IPEndPoint(IPAddress.Parse(localIpStr), LocalPort);
                return new IPEndPoint(IPAddress.Any, LocalPort);
            }

            public IPEndPoint GetRemoteEndPoint()
            {
                return new IPEndPoint(IPAddress.Parse(RemoteIp), RemotePort);
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
