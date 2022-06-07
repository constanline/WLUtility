using WLUtility.DataManager;

namespace WLUtility.Model
{
    internal class BagItem
    {
        private string _itemName;

        public ushort Id { get; set; }


        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_itemName) && Id > 0)
                    _itemName = DataManagers.ItemManager.GetName(Id);
                return _itemName;
            }
        }

        public byte Qty { get; set; }

        public byte Damage { get; set; }

        public int Durable { get; set; }

        public string DurableStr
        {
            get
            {
                var item = DataManagers.ItemManager.GetOne(Id);
                if (item != null)
                {
                    if (item.FitType > 0 && item.FitType < 7) return (250 - Damage) + "/250";
                    if (item.SpecialAbility == 151)
                    {
                        var max = item.Value1 - 100;
                        return max - Durable + "/" + max;
                    }
                }

                return string.Empty;
            }
        }
        public string Desc { get; set; }

        public void Init()
        {
            Id = 0;
            _itemName = null;
            Qty = 0;
            Damage = 0;
            Durable = 0;
            Desc = null;                                                                                                                                    
        }
        public virtual byte AddItem(ushort id, byte qty, byte aDamage, int aDurable)
        {
            byte result = 0;
            if (id == 0) return result;
            if (Qty == 0 && qty >= 1) Id = id;
            if (!DataManagers.ItemManager.IsOverlap(id))
            {
                Qty = 1;
                Damage = aDamage;
                Durable = aDurable;
                return qty;
            }

            if (Qty + qty >= 50)
            {
                result = (byte)(50 - Qty);
                Qty = 50;
                return result;
            }

            Qty += qty;
            return qty;
        }

        public byte DelItem(byte quantity)
        {
            byte result;
            if (Qty - quantity <= 0)
            {
                result = Qty;
                Qty = 0;
                Id = 0;
                _itemName = null;
            }
            else
            {
                result = quantity;
                Qty -= quantity;
            }

            return result;
        }

        public void Clear()
        {
            Id = 0;
            Qty = 0;
            _itemName = null;
            Desc = null;
        }

        public BagItem Clone()
        {
            return new BagItem
            {
                Id = Id,
                Qty = Qty,
                Durable = Durable
            };
        }

        public void CopyTo(BagItem bagItem)
        {
            bagItem.Id = Id;
            bagItem.Qty = Qty;
            bagItem.Durable = Durable;
        }
    }
}
