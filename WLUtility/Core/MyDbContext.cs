using System;
using System.Data.Entity;
using WLUtility.Data;

namespace WLUtility.Core
{
    internal class MyDbContext : DbContext, IDisposable
    {
        private static MyDbContext _instance;

        public static MyDbContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MyDbContext();
                }
                return _instance;
            }
        }

        public DbSet<Item> ItemList { get; set; }

        public MyDbContext() : base("ConnStr")
        {
        }
    }
}
