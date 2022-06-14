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

        public byte DropWhenDamage { get; set; } = 240;

        public event Action AutoSellItemUpdated;

        public event Action InfoUpdate;

        private readonly ProxySocket _socket;

        public bool IsAutoSelling;

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

            var eventNoStr = IniHelper.Account.GetString(Id.ToString(), "WoodManEventNo");
            if (byte.TryParse(eventNoStr, out var eventNo))
            {
                _socket.WoodManInfo.EventNo = eventNo;
            }

            InfoUpdate?.Invoke();

            AutoSellItemUpdated?.Invoke();

            AutoSellItemUpdated += PlayerInfo_AutoSellItemUpdated;
        }

        public void SellItem()
        {
            if(!IsAutoSell)return;
            if (IsAutoSelling) return;

            IsAutoSelling = true;
            var bagItems = BagItems;
            for (byte i = 1; i <= 50; i++)
            {
                if (bagItems[i].Id > 0)
                {
                    if (AutoSellItemList.Contains(bagItems[i].Id))
                    {
                        var flag = true;
                        if (IsSellWhenFull)
                        {
                            flag = (bagItems[i].Qty == 50 || !DataManagers.ItemManager.IsOverlap(bagItems[i].Id));
                        }

                        if (flag)
                        {
                            _socket.SendPacket(new PacketBuilder(0x1B, 0x03).Add(i).Build());
                            return;
                        }
                    }
                }
            }
            IsAutoSelling = false;
        }

        private void PlayerInfo_AutoSellItemUpdated()
        {
            var autoSellItem = string.Join("|", AutoSellItemList.ToArray());
            IniHelper.Account.WriteString(Id.ToString(), "AutoSellItem", autoSellItem);
            
            SellItem();
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
                SellItem();
        }

        public void SwitchSellWhenFull(bool isSellWhenFull)
        {
            _socket.PlayerInfo.IsSellWhenFull = isSellWhenFull;
            IniHelper.Account.WriteString(Id.ToString(), "IsSellWhenFull", isSellWhenFull.ToString());

            if (!isSellWhenFull)
                SellItem();
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

                //GetBagItemMsg?.Invoke("得到物品" + DataMgrs.ItemData.GetName(id) + "*" + remaining + ",放到物品栏第" + pos + "个位置");
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