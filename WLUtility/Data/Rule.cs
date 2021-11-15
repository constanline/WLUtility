using System.Collections.Generic;

namespace WLUtility.Data
{
    internal class Rule
    {
        /// <summary>
        /// 规则类型
        /// </summary>
        public ERuleType RuleType { get; }

        /// <summary>
        /// 子规则
        /// </summary>
        public List<Rule> Children { get; }

        /// <summary>
        /// 字符串长度索引数组
        /// </summary>
        public List<int> StrIndex { get; }

        /// <summary>
        /// 要增加字节数组
        /// </summary>
        public byte[] AddBuffer { get; }

        /// <summary>
        /// 调整索引
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 调整长度
        /// </summary>
        public int Len { get; }

        /// <summary>
        /// 偏移
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// 下个规则
        /// </summary>
        public Rule NextRule { get; set; }

        /// <summary>
        /// 规则链追加规则
        /// </summary>
        /// <param name="newRule">要追加的规则</param>
        /// <returns>追加后的结果</returns>
        public Rule Append(Rule newRule)
        {
            var rule = this;
            while (rule.NextRule != null)
            {
                rule = rule.NextRule;
            }

            rule.NextRule = newRule;
            return this;
        }

        /// <summary>
        /// 创建跳过规则
        /// </summary>
        /// <returns></returns>
        public static Rule BuildSkipRule()
        {
            return new Rule(ERuleType.Skip, null, null, -1, -1, -1);
        }

        /// <summary>
        /// 创建移除规则
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="len">长度</param>
        /// <param name="strIndex">字符串索引数组</param>
        /// <returns>创建的规则</returns>
        public static Rule BuildRemoveRule(int index, int len, List<int> strIndex)
        {
            return BuildRemoveRule(index, len, 0, strIndex);
        }

        public static Rule BuildAddRule(int index, byte[] buffer, int offset = 0, List<int> strIndex = null)
        {
            return new Rule(ERuleType.Add, null, strIndex, index, -1, offset, buffer);
        }

        public static Rule BuildRemoveRule(int index, int len = -1, int offset = 0, List<int> strIndex = null)
        {
            return new Rule(ERuleType.Remove, null, strIndex, index, len, offset);
        }

        public static Rule BuildParentRule(List<Rule> children)
        {
            return new Rule(ERuleType.Parent, children, null, -1, -1, -1);
        }

        public static Rule BuildLoopRule(List<Rule> children, int offset, int index = -1)
        {
            return new Rule(ERuleType.Loop, children, null, index, -1, offset);
        }

        private Rule(ERuleType ruleType, List<Rule> children, List<int> strIndex, int index, int len, int offset, byte[] buffer = null)
        {
            RuleType = ruleType;
            Children = children;
            strIndex?.Sort();
            StrIndex = strIndex;
            Index = index;
            Len = len;
            Offset = offset;
            AddBuffer = buffer;
        }

        internal enum ERuleType
        {
            Skip,
            Add,
            Remove,
            Parent,
            Loop

        }
    }
}
