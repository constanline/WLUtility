using WLUtility.Core;

namespace WLUtility.Model
{
    internal class WoodManInfo
    {
        public byte EventId { get; set; }

        private readonly ProxySocket _socket;

        public WoodManInfo(ProxySocket socket)
        {
            _socket = socket;
        }

        public bool IsSetEventId()
        {
            return EventId != 0;
        }

        public void StartWoodMan(byte eventId = 0)
        {
            if (eventId > 0)
                EventId = eventId;

            if (EventId == 0)
            {
                _socket.Log("木人桩EventID未设置");
                return;
            }
            _socket.SendPacket(new byte[] { 0x69, 0x03 });
            _socket.SendPacket(new byte[] { 0x14, 0x06 });
            _socket.SendPacket(new byte[] { 0x14, 0x01, EventId, 0x00 });
            _socket.SendPacket(new byte[] { 0x69, 0x02 });
            _socket.SendPacket(new byte[] { 0x14, 0x06 });
        }
    }
}