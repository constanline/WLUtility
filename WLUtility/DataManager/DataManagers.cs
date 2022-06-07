namespace WLUtility.DataManager
{
    internal class DataManagers
    {
        public static ItemManager ItemManager;

        public static void Init()
        {
            ItemManager = new ItemManager();
        }
    }
}
