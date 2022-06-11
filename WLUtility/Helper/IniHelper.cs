using System.Runtime.InteropServices;
using System.Text;

namespace WLUtility.Helper
{
    public class IniHelper
    {
        public static IniHelper Account { get; } = new IniHelper("account.ini");

        private readonly string _filename;

        public IniHelper(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="section">段落名</param>
        /// <param name="key">键名</param>
        /// <param name="defVal">读取异常是的缺省值</param>
        /// <param name="retVal">键名所对应的的值，没有找到返回空值</param>
        /// <param name="size">返回值允许的大小</param>
        /// <param name="filepath">ini文件的完整路径</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string defVal,
            StringBuilder retVal,
            int size,
            string filepath);

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="section">需要写入的段落名</param>
        /// <param name="key">需要写入的键名</param>
        /// <param name="val">写入值</param>
        /// <param name="filepath">ini文件的完整路径</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(
            string section,
            string key,
            string val,
            string filepath);


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="section">段落名</param>
        /// <param name="key">键名</param>
        /// <param name="def">没有找到时返回的默认值</param>
        /// <returns></returns>
        public string GetString(string section, string key, string def = "")
        {
            var sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, sb, 1024, _filename);
            return sb.ToString();
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="section">段落名</param>
        /// <param name="key">键名</param>
        /// <param name="val">写入值</param>
        public void WriteString(string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, _filename);
        }
    }
}