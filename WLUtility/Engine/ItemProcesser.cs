using System.Collections.Generic;
using WLUtility.Core;

namespace WLUtility.Engine
{
    internal class ItemProcesser : IProcesser
    {
        private readonly Dictionary<ushort, ushort> _dicReplaceItem = new Dictionary<ushort, ushort>();

        public ItemProcesser()
        {
            _dicReplaceItem.Add(34281, 34285);
            _dicReplaceItem.Add(34330, 34285);
            _dicReplaceItem.Add(34339, 34285);
            _dicReplaceItem.Add(34282, 34286);
            _dicReplaceItem.Add(34331, 34286);
            _dicReplaceItem.Add(34340, 34286);
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0x17)
            {
                if (bType == 0x05)
                {
                    ReplaceItem(packet);
                }
            }
        }

        private void ReplaceItem(List<byte> packet)
        {
            const int offset = 7;
            var num = 0;
            while (num * 32 + offset < packet.Count)
            {
                var tmp = num * 32 + offset;
                var itemId = (ushort)((packet[tmp] ^ BaseSocket.XOR_BYTE) + ((packet[tmp + 1] ^ BaseSocket.XOR_BYTE) << 8));
                if (_dicReplaceItem.ContainsKey(itemId))
                {
                    var toId = _dicReplaceItem[itemId];
                    packet[tmp] = (byte)((toId & 0xFF) ^ BaseSocket.XOR_BYTE);
                    packet[tmp + 1] = (byte)((toId >> 8) ^ BaseSocket.XOR_BYTE);
                }
                num++;
            }
        }
    }
}
