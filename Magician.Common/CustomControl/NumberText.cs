using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CCWin.SkinControl;

namespace Magician.Common.CustomControl
{
    public class NumberText : SkinTextBox
    {
        #region 字段
        private readonly ErrorProvider _ep;

        private event Action ValidChanged;

        private bool _isValid;

        private bool _showErrorProvider = true;

        private string _pattern;

        private string _tip = string.Empty;

        private int _decimalPlaces;

        private int _maxValue = 100;

        private int _minValue;
        #endregion

        #region 属性
        public decimal? Value
        {
            get
            {
                if (decimal.TryParse(Text, out var val))
                {
                    return val;
                }
                return null;
            }
        }

        public byte? ByteValue
        {
            get
            {
                var tmp = Value;
                if (tmp == null)
                {
                    return null;
                }
                return Convert.ToByte(tmp);
            }
        }

        public int? IntValue
        {
            get
            {
                var tmp = Value;
                if (tmp == null)
                {
                    return null;
                }
                return Convert.ToInt32(tmp);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("文本"), Category("文本")]
        public new int MaxLength
        {
            get;
            set;
        }

        [Description("文本"), Category("文本")]
        public bool ShowErrorProvider
        {
            get => _showErrorProvider;
            set
            {
                _showErrorProvider = value;
                TipError();
            }
        }

        [Description("文本"), Category("文本")]
        public override string Text
        {
            get => SkinTxt.Text;
            set
            {
                if (_pattern == null)
                {
                    CombinePattern();
                }
                SkinTxt.Text = value;
                CheckValid(value);
            }
        }


        [Description("保留小数位数"), Category("小数位数")]
        public int DecimalPlaces
        {
            get => _decimalPlaces;
            set
            {
                if (value <= 0)
                    _decimalPlaces = 0;
                else if (value >= 4)
                    _decimalPlaces = 4;
                else
                    _decimalPlaces = value;

                CombinePattern();
            }
        }

        [Description("最大数值"), Category("最大值")]
        public int MaxValue
        {
            get => _maxValue;

            set
            {
                if (value >= _minValue)
                {
                    _maxValue = value;
                }
                CombinePattern();
            }
        }

        [Description("最小数值"), Category("最小值")]
        public int MinValue
        {
            get => _minValue;

            set
            {
                if (value <= _maxValue)
                {
                    _minValue = value;
                }
                CombinePattern();
            }
        }
        #endregion

        #region 事件
        public new EventHandler GotFocus;

        public new EventHandler TextChanged;

        public KeyPressEventHandler BeforeKeyPress;

        public KeyPressEventHandler AfterKeyPress;
        #endregion

        #region 方法
        private void CombinePattern()
        {
            StringBuilder sb = new StringBuilder("^");
            StringBuilder sbTip = new StringBuilder(string.Concat("只允许输入范围[", _minValue, "~", _maxValue, "]的数值"));
            if (_maxValue < 0)
            {
                sb.Append("-");
            }
            else if (_minValue < 0)
            {
                sb.Append("-?");
            }

            sb.Append(@"\d+");


            if (_decimalPlaces > 0)
            {
                sbTip.AppendFormat("，小数位数不能超过{0}位", _decimalPlaces);
                sb.AppendFormat(@"(\.\d{{1,{0}}})?", _decimalPlaces);
            }
            sb.Append("$");

            _pattern = sb.ToString();
            _tip = sbTip.ToString();
        }

        private void TipError()
        {
            if (_showErrorProvider && !_isValid)
                _ep.SetError(this, _tip);
            else
                _ep.Clear();
        }

        private bool CheckValid(string newText)
        {
            bool isValid;
            if (decimal.TryParse(newText, out var val))
                isValid = Regex.IsMatch(newText, _pattern) && val >= _minValue && val <= _maxValue;
            else
                isValid = false;

            if (_isValid != isValid)
            {
                _isValid = isValid;
                ValidChanged?.Invoke();
            }
            return _isValid;
        }
        #endregion

        #region 构造器
        public NumberText()
        {
            SkinTxt.GotFocus += NumberText_GotFocus;
            SkinTxt.TextChanged += NumberText_TextChanged;
            SkinTxt.KeyPress += NumberText_KeyPress;
            SkinTxt.Leave += NumberText_Leave;
            ValidChanged += NumberText_ValidChanged;

            SkinTxt.MaxLength = 255;
            _ep = new ErrorProvider();
        }

        private void NumberText_ValidChanged()
        {
            TipError();
        }
        #endregion

        #region 事件响应
        private void NumberText_GotFocus(object sender, EventArgs e)
        {
            GotFocus?.Invoke(sender, e);
        }

        private void NumberText_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void NumberText_Leave(object sender, EventArgs e)
        {
        }

        private void NumberText_KeyPress(object sender, KeyPressEventArgs e)
        {
            BeforeKeyPress?.Invoke(this, e);

            if (e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '\b' && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '-' && _minValue >= 0)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '.' && _decimalPlaces == 0)
            {
                e.Handled = true;
                return;
            }

            var newValue = new StringBuilder();
            newValue.Append(Text.Substring(0, SkinTxt.SelectionStart));
            if (e.KeyChar == '\b')
            {
                if (SkinTxt.SelectionLength == 0 && newValue.Length > 0)
                {
                    newValue.Remove(newValue.Length - 1, 1);
                }
            }
            else
            {
                newValue.Append(e.KeyChar);
            }
            newValue.Append(Text.Substring(SkinTxt.SelectionStart + SkinTxt.SelectionLength));

            var newText = newValue.ToString();
            if (newText.Equals("-") || newText.Length == 0 || newText.IndexOf('.') == newText.Length - 1)
            {
                return;
            }

            if (!CheckValid(newText))
            {
                return;
            }

            AfterKeyPress?.Invoke(this, e);
        }
        #endregion
    }
}
