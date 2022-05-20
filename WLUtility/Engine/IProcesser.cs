using System.Collections.Generic;
using WLUtility.Core;

namespace WLUtility.Engine
{
    internal interface IProcesser
    {
        void Handle(ProxySocket proxySocket, byte aType, byte bType, List<byte> packet, ref bool isSkip);
    }
}
