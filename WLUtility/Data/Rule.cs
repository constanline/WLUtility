using System.Collections.Generic;

namespace WLUtility.Data
{
    internal class Rule
    {
        public ERuleType RuleType { get; }

        public List<Rule> Children { get; }

        public List<int> StrIndex { get; }

        public int Index { get; }

        public int Len { get; }

        public int Offset { get; }

        public static Rule BuildSkipRule()
        {
            return new Rule(ERuleType.Skip, null, null, -1, -1, -1);
        }

        public static Rule BuildRemoveRule(int index, int len, List<int> strIndex)
        {
            return BuildRemoveRule(index, len, 0, strIndex);
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

        private Rule(ERuleType ruleType, List<Rule> children, List<int> strIndex, int index, int len, int offset)
        {
            RuleType = ruleType;
            Children = children;
            strIndex?.Sort();
            StrIndex = strIndex;
            Index = index;
            Len = len;
            Offset = offset;
        }

        internal enum ERuleType
        {
            Skip,
            Remove,
            Parent,
            Loop

        }
    }
}
