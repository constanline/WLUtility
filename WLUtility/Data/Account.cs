using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WLUtility.Data
{
    [Table("account")]
    internal class Account
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        public string Username { get; set; }

        public int Role { get; set; }

        [Column("is_auto_sell")]
        public bool IsAutoSell { get;set; }

        [Column("auto_sell_item")]
        public string AutoSellItem { get; set; }
    }
}
