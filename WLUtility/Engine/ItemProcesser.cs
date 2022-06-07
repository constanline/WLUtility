﻿using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.DataManager;
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
                switch (bType)
                {
                    //初始化物品
                    case 0x05:
                        InitItem(packet);
                        ReplaceItem(packet);
                        break;
                    case 0x06:
                        AddItem(packet);
                        break;
                    case 0x07:
                        DelItem(packet);
                        break;
                    case 0x08:
                        AddItemWithPos(packet);
                        break;
                    case 0x09:
                        DelItemWithPos(packet);
                        break;
                    case 0x0A:
                        MoveItem(packet);
                        break;
                    case 0x0B:
                        InitEquip(packet);
                        break;
                    case 0x10:
                        TakeOffEquip(packet);
                        break;
                    case 0x11:
                        PutOnEquip(packet);
                        break;
                    case 0x17:
                        PetPutOnEquip(packet);
                        break;
                    case 0x1B:
                        RevDamageInfo(packet);
                        break;
                    case 0x1C:
                        RevFNpcFightDamage(packet);
                        break;
                }
            }
            else if (aType == 0x1A)
            {
                if (bType == 0x04)
                {
                    SetGold(packet);
                }
            }
            else if (aType == 0x1B)
            {
                //物品出售
                if (bType == 0x06)
                {
                    SoldItem(packet);
                }
            }
        }

        private void InitItem(List<byte> packet)
        {
            var bagItems = _socket.PlayerInfo.BagItems;
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
                num++;
                _socket.PlayerInfo.AddBagItem(id, qty, damage, durable, pos);
            }
            _socket.UpdateBag(bagItems);
        }

        private void SetGold(List<byte> packet)
        {
            var offset = 6;
            _socket.PlayerInfo.Gold = ByteUtil.ReadPacket<int>(packet, ref offset);
        }

        private void AddItem(List<byte> packet)
        {
            var offset = 6;
            var id = ByteUtil.ReadPacket<ushort>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var damage = ByteUtil.ReadPacket<byte>(packet, ref offset);
            offset += 2;
            var durable = ByteUtil.ReadPacket<int>(packet, ref offset);
            _socket.PlayerInfo.AddBagItem(id, qty, damage, durable);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void DelItem(List<byte> packet)
        {
            var offset = 6;
            var id = ByteUtil.ReadPacket<ushort>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            _socket.PlayerInfo.DelBagItem(id, qty);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void AddItemWithPos(List<byte> packet)
        {
            var offset = 6;
            var pos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var id = ByteUtil.ReadPacket<ushort>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var byDamage = ByteUtil.ReadPacket<byte>(packet, ref offset);
            offset += 2;
            var iDurable = ByteUtil.ReadPacket<int>(packet, ref offset);
            _socket.PlayerInfo.AddBagItem(id, qty, byDamage, iDurable, pos);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void DelItemWithPos(List<byte> packet)
        {
            var offset = 6;
            var pos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            _socket.PlayerInfo.DelBagItemWithPos(pos, qty);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void MoveItem(List<byte> packet)
        {
            var offset = 6;
            var srcPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var dstPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            _socket.PlayerInfo.MoveBagItem(srcPos, qty, dstPos);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void SoldItem(List<byte> packet)
        {
            var offset = 6;
            offset += 4;
            // var gold = ByteUtil.ReadPacket<int>(packet, ref offset);
            var pos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var qty = ByteUtil.ReadPacket<byte>(packet, ref offset);
            _socket.PlayerInfo.DelBagItemWithPos(pos, qty);

            //增加金币
            // _socket.LocalSocket.Send(new PacketBuilder(0x1A, 0x04).Add(_socket.PlayerInfo.Gold + gold).Build());
            //移除物品
            _socket.RevPacket(new PacketBuilder(0x17, 0x09).Add(pos).Add(qty).Build());

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void InitEquip(List<byte> buffer)
        {
            var offset = 6;
            while (offset < buffer.Count)
            {
                var itemId = ByteUtil.ReadPacket<ushort>(buffer, ref offset);
                var damage = ByteUtil.ReadPacket<byte>(buffer, ref offset);
                offset += 19;
                var fitType = DataManagers.ItemManager.GetOne(itemId).FitType;
                _socket.PlayerInfo.Equips[fitType].Id = itemId;
                _socket.PlayerInfo.Equips[fitType].Damage = damage;
                _socket.PlayerInfo.Equips[fitType].Qty = 1;
            }
        }

        private void PutOnEquip(List<byte> packet)
        {
            var offset = 6;
            var itemPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var unPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var tmp = _socket.PlayerInfo.BagItems[itemPos].Clone();
            var equipPos = DataManagers.ItemManager.GetOne(tmp.Id).FitType;
            _socket.PlayerInfo.BagItems[itemPos].Clear();
            if (_socket.PlayerInfo.Equips[equipPos].Id > 0)
            {
                _socket.PlayerInfo.Equips[equipPos].CopyTo(_socket.PlayerInfo.BagItems[unPos]);
            }
            tmp.CopyTo(_socket.PlayerInfo.Equips[equipPos]);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void TakeOffEquip(List<byte> packet)
        {
            var offset = 6;
            var equipPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var itemPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            _socket.PlayerInfo.Equips[equipPos].CopyTo(_socket.PlayerInfo.BagItems[itemPos]);
            _socket.PlayerInfo.Equips[equipPos].Clear();

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }

        private void PetPutOnEquip(List<byte> packet)
        {
            var offset = 6;
            var npcPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var itemPos = ByteUtil.ReadPacket<byte>(packet, ref offset);
            var tmp = _socket.PlayerInfo.BagItems[itemPos].Clone();
            var equipPos = DataManagers.ItemManager.GetOne(tmp.Id).FitType;
            _socket.PlayerInfo.BagItems[itemPos].Clear();
            if (_socket.PlayerInfo.Pets[npcPos].Equips[equipPos].Id > 0)
            {
                _socket.PlayerInfo.Pets[npcPos].Equips[equipPos].CopyTo(_socket.PlayerInfo.BagItems[itemPos]);
            }
            tmp.CopyTo(_socket.PlayerInfo.Pets[npcPos].Equips[equipPos]);

            _socket.UpdateBag(_socket.PlayerInfo.BagItems);
        }



        public void RevDamageInfo(List<byte> packet)
        {
            var iPos = 6;
            var equipPos = ByteUtil.ReadPacket<byte>(packet, ref iPos);
            var damage = ByteUtil.ReadPacket<byte>(packet, ref iPos);
            if (equipPos == 0 || equipPos > 6) return;

            if (_socket.PlayerInfo.Equips[equipPos].Id ==0)return;

            _socket.PlayerInfo.Equips[equipPos].Damage = damage;
            _socket.Log(_socket.PlayerInfo.Equips[equipPos].Name + "损坏度" + damage + "/250");
            if (damage >= 250)_socket.Log(_socket.PlayerInfo.Equips[equipPos].Name + "毁坏！");
        }

        public void RevFNpcFightDamage(List<byte> buffer)
        {
            var iPos = 6;
            var npcPos = ByteUtil.ReadPacket<byte>(buffer, ref iPos);
            var equipPos = ByteUtil.ReadPacket<byte>(buffer, ref iPos);
            var damage = ByteUtil.ReadPacket<byte>(buffer, ref iPos);

            var npc = _socket.PlayerInfo.Pets[npcPos];
            if (npc == null || npc.Id == 0) return;
            if (npc.Equips[equipPos].Id == 0) return;

            npc.Equips[equipPos].Damage = damage;
            _socket.Log(npc.Equips[equipPos].Name + "损坏度" + damage + "/250");
            if (damage >= 250) _socket.Log(npc.Equips[equipPos].Name + "毁坏！");
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
