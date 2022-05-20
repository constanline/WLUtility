using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class SignProcesser : IProcesser
    {
        private readonly ProxySocket _socket;

        public SignProcesser(ProxySocket socket)
        {
            _socket = socket;
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            var num = 6;
            if(aType == 0x01 && bType == 0x1E)
            {
                var cType = ByteUtil.ReadPacket<byte>(packet, ref num);
                if (cType == 0x02)
                {
                    var signDay = ByteUtil.ReadPacket<byte>(packet, ref num);
                    var isSigned = ByteUtil.ReadPacket<bool>(packet, ref num);
                    var signMaxDay = ByteUtil.ReadPacket<byte>(packet, ref num);
                    var msg = "已签到" + signDay + "/" + signMaxDay + "天";
                    if (!isSigned)
                    {
                        msg += "。今天未签到，执行签到";
                        _socket.SendPacket(new byte[] { 0x01, 0x1E, 0x03 });
                    }
                    _socket.Log(msg);
                }
            }
        }
    }
}
