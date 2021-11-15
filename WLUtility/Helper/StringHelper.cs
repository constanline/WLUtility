using System.Text;

namespace WLUtility.Helper
{
    public class StringHelper
    {
        public static string Buffer2Hex(byte[] buffer)
        {
            var sb = new StringBuilder();
            foreach (var t in buffer)
            {
                sb.Append($"{t:x2}");
            }

            return sb.ToString();
        }
    }
}