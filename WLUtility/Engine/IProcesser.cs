using System.Collections.Generic;

namespace WLUtility.Engine
{
    internal interface IProcesser
    {
        void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip);
    }
}
