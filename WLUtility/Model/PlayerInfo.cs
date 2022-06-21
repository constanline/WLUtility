using System;
using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.DataManager;
using WLUtility.Helper;

namespace WLUtility.Model
{
    internal class PlayerInfo
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public BagItem[] BagItems { get; } = new BagItem[51];

        public BagItem[] Equips { get; } = new BagItem[7];

        public Pet[] Pets { get; } = new Pet[5];

        public int Gold { get; set; }

        public string Name { get; set; }

        public List<ushort> AutoSellItemList { get; } = new List<ushort>();

        public bool IsAutoSell { get; set; }

        public bool IsSellWhenFull { get; set; }

        public List<ushort> AutoDropItemList { get; } = new List<ushort>();

        public event Action AutoSellItemUpdated;

        public bool IsAutoDrop { get; set; }

        public event Action AutoDropItemUpdated;

        public byte UnfitWhenDamage { get; set; } = 240;

        public event Action InfoUpdate;

        private readonly ProxySocket _socket;

        public bool IsAutoSellingOrDropping;

        public PlayerInfo(ProxySocket socket)
        {
            _socket = socket;
            for (var i = 1; i <= 50; i++)
            {
                BagItems[i] = new BagItem();
            }
            for (var i = 1; i <= 6; i++)
            {
                Equips[i] = new BagItem();
            }
            for (var i = 1; i <= 4; i++)
            {
                Pets[i] = new Pet();
            }
        }

        public void InitPlayerAccount()
        {
            var autoSellItem = IniHelper.Account.GetString(Id.ToString(), "AutoSellItem");
            var spSellItem = autoSellItem.Split('|');
            foreach (var item in spSellItem)
            {
                if (ushort.TryParse(item, out var id))
                {
                    AutoSellItemList.Add(id);
                }
            }
            var isAutoSell = IniHelper.Account.GetString(Id.ToString(), "IsAutoSell");
            if (bool.TryParse(isAutoSell, out var result))
                IsAutoSell = result;

            var isSellWhenFull = IniHelper.Account.GetString(Id.ToString(), "IsSellWhenFull");
            if (bool.TryParse(isSellWhenFull, out result))
                IsSellWhenFull = result;

            var autoDropItem = IniHelper.Account.GetString(Id.ToString(), "AutoDropItem");
            var spDropItem = autoDropItem.Split('|');
            foreach (var item in spDropItem)
            {
                if (ushort.TryParse(item, out var id))
                {
                    AutoDropItemList.Add(id);
                }
            }
            var isAutoDrop = IniHelper.Account.GetString(Id.ToString(), "IsAutoDrop");
            if (bool.TryParse(isAutoDrop, out result))
                IsAutoDrop = result;

            var eventNoStr = IniHelper.Account.GetString(Id.ToString(), "WoodManEventNo");
            if (byte.TryParse(eventNoStr, out var eventNo))
            {
                _socket.WoodManInfo.EventNo = eventNo;
            }

            InfoUpdate?.Invoke();

            AutoSellItemUpdated?.Invoke();
            AutoDropItemUpdated?.Invoke();

            AutoSellItemUpdated += PlayerInfo_AutoSellItemUpdated;
            AutoDropItemUpdated += PlayerInfo_AutoDropItemUpdated;
        }

        public void SellAndDropItem()
        {
            if(!IsAutoSell && !IsAutoDrop)return;
            if (IsAutoSellingOrDropping) return;

            IsAutoSellingOrDropping = true;
            var bagItems = BagItems;
            for (byte pos = 1; pos <= 50; pos++)
            {
                if (bagItems[pos].Id <= 0) continue;

                if (IsAutoSell && AutoSellItemList.Contains(bagItems[pos].Id))
                {
                    var flag = true;
                    if (IsSellWhenFull)
                    {
                        flag = (bagItems[pos].Qty == 50 || !DataManagers.ItemManager.IsOverlap(bagItems[pos].Id));
                    }

                    if (flag)
                    {
                        _socket.SendPacket(new PacketBuilder(0x1B, 0x03).Add(pos).Build());
                        return;
                    }
                }
                else if (IsAutoDrop && AutoDropItemList.Contains(bagItems[pos].Id))
                {
                    _socket.SendPacket(new PacketBuilder(0x17, 0x03).Add(pos).Add(bagItems[pos].Qty).Add((byte)1).Build());
                    _socket.SendPacket(new PacketBuilder(0x17, 0x7C).Add(pos).Add(bagItems[pos].Qty).Add((byte)2).Build());
                    return;
                }
            }
        }

        private void PlayerInfo_AutoSellItemUpdated()
        {
            var autoSellItem = string.Join("|", AutoSellItemList.ToArray());
            IniHelper.Account.WriteString(Id.ToString(), "AutoSellItem", autoSellItem);
            
            SellAndDropItem();
        }

        private void PlayerInfo_AutoDropItemUpdated()
        {
            var autoDropItem = string.Join("|", AutoDropItemList.ToArray());
            IniHelper.Account.WriteString(Id.ToString(), "AutoDropItem", autoDropItem);

            SellAndDropItem();
        }

        public void AddAutoSellItemIdx(int idx)
        {
            if (idx <= 0 || idx >= BagItems.Length || BagItems[idx].Id == 0) return;
            if (AutoSellItemList.Contains(BagItems[idx].Id)) return;
            AutoSellItemList.Add(BagItems[idx].Id);

            AutoSellItemUpdated?.Invoke();
        }

        public void DelAutoSellItemIdx(int idx)
        {
            if (AutoSellItemList.Count <= idx) return;
            AutoSellItemList.RemoveAt(idx);

            AutoSellItemUpdated?.Invoke();
        }

        public void SwitchAutoSell(bool isAutoSell)
        {
            _socket.PlayerInfo.IsAutoSell = isAutoSell;
            IniHelper.Account.WriteString(Id.ToString(), "IsAutoSell", isAutoSell.ToString());

            if(isAutoSell)
                SellAndDropItem();
        }

        public void SwitchSellWhenFull(bool isSellWhenFull)
        {
            _socket.PlayerInfo.IsSellWhenFull = isSellWhenFull;
            IniHelper.Account.WriteString(Id.ToString(), "IsSellWhenFull", isSellWhenFull.ToString());

            if (!isSellWhenFull)
                SellAndDropItem();
        }

        public void AddAutoDropItemIdx(int idx)
        {
            if (idx <= 0 || idx >= BagItems.Length || BagItems[idx].Id == 0) return;
            if (AutoDropItemList.Contains(BagItems[idx].Id)) return;
            AutoDropItemList.Add(BagItems[idx].Id);

            AutoDropItemUpdated?.Invoke();
        }

        public void DelAutoDropItemIdx(int idx)
        {
            if (AutoDropItemList.Count <= idx) return;
            AutoDropItemList.RemoveAt(idx);

            AutoDropItemUpdated?.Invoke();
        }

        public void SwitchAutoDrop(bool isAutoDrop)
        {
            _socket.PlayerInfo.IsAutoDrop = isAutoDrop;
            IniHelper.Account.WriteString(Id.ToString(), "IsAutoDrop", isAutoDrop.ToString());

            if (isAutoDrop)
                SellAndDropItem();
        }

        public void AddBagItem(ushort id, byte qty, byte damage, int durable, byte defPos = 0)
        {
            var flag = false;
            byte remaining;
            for (var num = qty; num != 0; num -= remaining)
            {
                byte pos;
                if (defPos > 0)
                {
                    if (flag) break;
                    flag = true;
                    pos = defPos;
                }
                else
                {
                    pos = DataManagers.ItemManager.IsOverlap(id)
                        ? FindItemPos(id, true)
                        : (byte)0;

                    if (pos == 0) pos = FindEmptyPos();
                }
                remaining = BagItems[pos].AddItem(id, num, damage, durable);
            }
        }

        public void DelBagItem(ushort id, byte qty)
        {
            while (qty > 0)
            {
                var pos = FindItemPos(id);
                if(pos == 0) break;
                var delQty = BagItems[pos].DelItem(qty);
                qty -= delQty;
            }
        }

        public void DelBagItemWithPos(byte pos, byte qty)
        {
            BagItems[pos].DelItem(qty);
        }

        public void MoveBagItem(byte srcPos, byte qty, byte dstPos)
        {
            var bagItem = BagItems[srcPos];
            BagItems[dstPos].AddItem(bagItem.Id, qty, bagItem.Damage, bagItem.Durable);
            BagItems[srcPos].DelItem(qty);
        }

        private byte FindItemPos(ushort id, bool needEmpty = false)
        {
            byte pos = 0;
            for (byte i = 1; i <= 50; i++)
            {
                if (BagItems[i].Id == id)
                {
                    if (!needEmpty || BagItems[i].Qty < 50)
                    {
                        pos = i;
                        break;
                    }
                }
            }
            return pos;
        }

        public byte FindEmptyPos(byte startIdx = 1)
        {
            byte pos = 0;
            for (byte i = startIdx; i <= 50; i++)
            {
                if (BagItems[i].Id == 0)
                {
                    pos = i;
                    break;
                }
            }
            return pos;
        }
    }
}