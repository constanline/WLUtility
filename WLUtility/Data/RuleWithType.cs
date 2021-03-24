using System.Collections.Generic;

namespace WLUtility.Data
{
    internal class RuleWithType
    {
        public byte TypeA { get; }

        public ECmpTypeB CmpTypeB { get; }

        public byte TypeB { get; }

        public Rule.ERuleType RuleType { get; }

        public List<RuleWithType> Children { get; }

        public List<int> StrIndex { get; }

        public int Index { get; }

        public int Len { get; }

        public int Offset { get; }

        public static RuleWithType BuildSkipRule(byte typeA, ECmpTypeB cmpTypeB, byte typeB)
        {
            return new RuleWithType(typeA, cmpTypeB, typeB, Rule.ERuleType.Skip, null, null, -1, -1, -1);
        }

        public static RuleWithType BuildChildRemoveRule(int index, int len = -1, int offset = 0)
        {
            return new RuleWithType(0, ECmpTypeB.None, 0, Rule.ERuleType.Remove, null, null, index, len, offset);
        }

        public static RuleWithType BuildRemoveRule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, int index, int len = -1)
        {
            return new RuleWithType(typeA, cmpTypeB, typeB, Rule.ERuleType.Remove, null, null, index, len, -1);
        }

        public static RuleWithType BuildParentRule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, List<RuleWithType> children)
        {
            return new RuleWithType(typeA, cmpTypeB, typeB, Rule.ERuleType.Parent, children, null, -1, -1, -1);
        }

        public static RuleWithType BuildLoopRule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, List<RuleWithType> children, int offset)
        {
            return new RuleWithType(typeA, cmpTypeB, typeB, Rule.ERuleType.Loop, children, null, -1, -1, offset);
        }

        private RuleWithType(byte typeA, ECmpTypeB cmpTypeB, byte typeB, Rule.ERuleType ruleType, List<RuleWithType> children, List<int> strIndex, 
            int index, int len, int offset)
        {
            TypeA = typeA;
            CmpTypeB = cmpTypeB;
            TypeB = typeB;
            RuleType = ruleType;
            Children = children;
            StrIndex = strIndex;
            Index = index;
            Len = len;
            Offset = offset;
        }
    }

    internal enum ECmpTypeB
    {
        None,
        Equal,
        MoreThen,
        LessThen,
    }
}
