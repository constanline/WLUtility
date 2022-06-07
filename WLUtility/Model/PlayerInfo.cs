using System;

namespace WLUtility.Model
{
    internal class PlayerInfo
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public BagItem[] BagItems { get; } = new BagItem[51];

        public void AddBagItem(ushort id, byte qty, byte damage, int durable, byte defPos = 0)
        {
            bool flag = false;
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
                    pos = IsOverlap(id)
                        ? FindItemPos(id, true)
                        : (byte)0;

                    if (pos == 0) pos = FindEmptyPos();
                }
                remaining = BagItems[pos].AddItem(id, num, damage, durable);

                //GetBagItemMsg?.Invoke("得到物品" + DataMgrs.ItemData.GetName(id) + "*" + remaining + ",放到物品栏第" + pos + "个位置");
            }
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

        private bool IsOverlap(ushort id)
        {
            //TODO 判断道具是否能堆叠
            return true;
        }
    }
}