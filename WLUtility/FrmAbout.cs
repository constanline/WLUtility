using System;
using System.Reflection;
using Magician.Common.CustomControl;

namespace WLUtility
{
    partial class FrmAbout : MagicianForm
    {
        string _url = "https://github.com/constanline/WLUtility";
        public FrmAbout()
        {
            InitializeComponent();

            Text = string.Format("{0} {1}", GetLanguageString("About"), AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("{0} {1}", GetLanguageString("Version"), AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            textBoxDescription.Text = GetLanguageString("Description");
            lblGithubUrl.Text = GetLanguageString("AccessGithub");
        }

        private void lblGithubUrl_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_url);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 程序集特性访问器

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        #endregion
    }
}
