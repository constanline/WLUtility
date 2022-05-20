using System.Collections.Generic;
using System.Text;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class LotteryProcesser : IProcesser
    {
        private readonly ProxySocket _socket;

        public LotteryProcesser(ProxySocket socket)
        {
            _socket = socket;
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0x7A)
            {
                if (bType == 0x01)
                {
                    RevLotteryInfo(packet);
                }
            }
        }

        private void RevLotteryInfo(List<byte> buffer)
        {
            var iPos = 6;
            var num = ByteUtil.ReadPacket<byte>(buffer, ref iPos);
            var msg = new StringBuilder();
            msg.Append("抽奖次数").Append(num);
            if (num > 0)
            {
                msg.Append(",开始抽奖");
                _socket.SendPacket(new byte[] { 0x7A, 0x02 });
            }
            _socket.Log(msg.ToString());
        }
    }
}
