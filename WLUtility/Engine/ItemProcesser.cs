using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.Data;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class ItemProcesser : IProcesser
    {
        private readonly Dictionary<ushort, ushort> _dicReplaceItem = new Dictionary<ushort, ushort>();

        private readonly ProxySocket _socket;

        public ItemProcesser(ProxySocket socket)
        {
            _socket = socket;

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
                    InitItem(packet);
                    ReplaceItem(packet);
                }else if(bType == 0x06)
                {
                    AddItem(packet);
                }
            }
        }

        private void InitItem(List<byte> packet)
        {
            var bagItems = _socket.PlayerInfo.BagItems;
            for (var i = 1; i <= 50; i++)
            {
                bagItems[i] = new Model.BagItem();
            }
            const int startOffset = 6;
            var num = 0;
            while (num * 32 + startOffset < packet.Count)
            {
                var offset = num * 32 + startOffset;
                var pos = ByteUtil.ReadPacket<byte>(packet, ref offset);
                var id = ByteUtil.ReadPacket<ushort>(packet, ref offset);
                var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
                var damage = ByteUtil.ReadPacket<byte>(packet, ref offset);
                offset += 2;
                var durable = ByteUtil.ReadPacket<int>(packet, ref offset);
                offset += 21;
                num++;
                _socket.PlayerInfo.AddBagItem(id, qty, damage, durable, pos);
            }
            _socket.UpdateBag(bagItems);
        }

        private void AddItem(List<byte> packet)
        {
            var offset = 6;
            var id = ByteUtil.ReadPacket<ushort>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var damage = ByteUtil.ReadPacket<byte>(packet, ref offset);
            offset += 2;
            var durable = ByteUtil.ReadPacket<int>(packet, ref offset);
            offset += 21;
            _socket.PlayerInfo.AddBagItem(id, qty, damage, durable);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
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
