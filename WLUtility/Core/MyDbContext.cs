using System;
using System.Data.Entity;
using WLUtility.Data;

namespace WLUtility.Core
{
    internal class MyDbContext : DbContext, IDisposable
    {
        private static MyDbContext _instance;

        public static MyDbContext Instance => _instance ?? (_instance = new MyDbContext());

        public DbSet<Item> ItemList { get; set; }

        public MyDbContext() : base("ConnStr")
        {
        }
    }
}
