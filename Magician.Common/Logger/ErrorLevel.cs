using System.ComponentModel;

namespace Magician.Common.Logger
{
    [Description("�쳣/�������ؼ���")]
    public enum ErrorLevel
    {
        [Description("������")]
        Fatal = 4,
        [Description("��")]
        High = 3,
        [Description("��ͨ")]
        Standard = 2,
        [Description("��")]
        Low = 1
    }    
}
