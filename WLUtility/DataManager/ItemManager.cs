using System;
using System.Collections.Generic;
using System.Linq;
using WLUtility.Core;
using WLUtility.Data;

namespace WLUtility.DataManager
{
    internal class ItemManager
    {
        public Dictionary<int, Item> _dicData = new Dictionary<int, Item>();

        public ItemManager()
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

        public string GetName(ushort id)
        {
            return _dicData.ContainsKey(id) ? _dicData[id].Name : null;
        }

        public bool IsOverlap(ushort id)
        {
            if (!_dicData.ContainsKey(id))
            {
                throw new Exception("未知的物品ID！！");
            }
            return _dicData.ContainsKey(id) ? _dicData[id].Overlap : false;
        }
    }
}
