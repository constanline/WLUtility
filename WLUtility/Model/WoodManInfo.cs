using WLUtility.Core;

namespace WLUtility.Model
{
    internal class WoodManInfo
    {
        public byte EventNo { get; set; }

        private readonly ProxySocket _socket;

        public WoodManInfo(ProxySocket socket)
        {
            _socket = socket;
        }

        public bool IsSetEventId()
        {
            return EventNo != 0;
        }

        public void StartWoodMan(byte eventNo = 0)
        {
            if (eventNo > 0)
                EventNo = eventNo;

            if (EventNo == 0)
            {
                _socket.Log("木人桩EventID未设置");
                return;
            }
            _socket.SendPacket(new byte[] { 0x69, 0x03 });
            _socket.SendPacket(new byte[] { 0x14, 0x06 });
            _socket.SendPacket(new byte[] { 0x14, 0x01, EventNo, 0x00 });
            _socket.SendPacket(new byte[] { 0x69, 0x02 });
            _socket.SendPacket(new byte[] { 0x14, 0x06 });
        }
    }
}