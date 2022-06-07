using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WLUtility.Data
{
    [Table("item")]
    internal class Item
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Kind { get; set; }

        public ushort Attribute1 { get; set; }

        public ushort Attribute2 { get; set; }

        public byte Value1 { get; set; }

        public byte Value2 { get; set; }

        public byte Level { get; set; }

        [Column("fit_type")]
        public byte FitType { get; set; }

        [Column("special_ability")]
        public byte SpecialAbility { get; set; }

        [Column("need_lv")]
        public byte NeedLV { get; set; }

        public string Desc { get; set; }

        public bool Overlap { get; set; }
    }
}
