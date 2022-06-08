using System.Collections.Generic;
using System.Linq;
using WLUtility.Core;
using WLUtility.Data;
using WLUtility.Helper;

namespace WLUtility.DataManager
{
    internal class ItemManager
    {
        private const bool USE_CACHE = true;

        private readonly Dictionary<int, Item> _dicData = new Dictionary<int, Item>();

        public ItemManager()
        {
            if (USE_CACHE)
            {
                using (var db = new MyDbContext())
                {
                    var ex = from r in db.ItemList select r;
                    foreach (var item in ex)
                    {
                        //if (_dicData.ContainsKey(item.Id))
                        //{
                        //    _dicData[item.Id] = item;
                        //}
                        _dicData.Add(item.Id, item);
                    }
                }
            }
        }

        public Item GetOne(ushort id)
        {
            if (USE_CACHE)
            {
                return _dicData.ContainsKey(id) ? _dicData[id] : null;
            }
            else
            {
                var ex = from r in MyDbContext.Instance.ItemList where r.Id == id select r;
                return ex.FirstOrDefault();
            }
        }

        public string GetName(ushort id)
        {
            if (USE_CACHE)
            {
                return _dicData.ContainsKey(id) ? _dicData[id].Name : "未知物品";
            }
            else
            {
                var ex = from r in MyDbContext.Instance.ItemList where r.Id == id select r.Name;
                return ex.FirstOrDefault();
            }
        }

        public bool IsOverlap(ushort id)
        {
            if (USE_CACHE)
            {
                if (!_dicData.ContainsKey(id))
                {
                    LogHelper.SilentLog($"未知的物品ID[{id}]，请维护到Data中！！");
                }
                return _dicData.ContainsKey(id) && _dicData[id].Overlap;
            }
            else
            {
                var ex = from r in MyDbContext.Instance.ItemList where r.Id == id select r.Overlap;
                return ex.FirstOrDefault();
            }
        }
    }
}
