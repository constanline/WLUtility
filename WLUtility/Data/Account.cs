namespace WLUtility.Data
{
    public class Account
    {
        public ushort RoleId { get; set; }

        public string Username { get; set; }

        public bool IsAutoSell { get; set; }

        public string AutoSellItem { get; set; }
    }
}