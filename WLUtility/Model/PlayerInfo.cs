﻿using WLUtility.DataManager;

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

        public PlayerInfo()
        {
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

        private byte FindEmptyPos()
        {
            byte pos = 0;
            for (byte i = 1; i <= 50; i++)
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