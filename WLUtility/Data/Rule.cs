using System.Collections.Generic;

namespace WLUtility.Data
{
    class Rule
    {
        public byte TypeA { get; }

        public ECmpTypeB CmpTypeB { get; }

        public byte TypeB { get; }

        public ERuleType RuleType { get; }

        public List<Rule> Children { get; }

        public List<int> StrIndex { get; }

        public int Index { get; }

        public int Len { get; }

        public int Offset { get; }

        /// <summary>
        /// 封包规则，此构造器仅用来创建跳过的封包规则
        /// </summary>
        /// <param name="typeA"></param>
        /// <param name="cmpTypeB"></param>
        /// <param name="typeB"></param>
        public Rule(byte typeA, ECmpTypeB cmpTypeB, byte typeB)
        {
            TypeA = typeA;
            CmpTypeB = cmpTypeB;
            TypeB = typeB;
            RuleType = ERuleType.Skip;
        }

        public Rule(ERuleType ruleType, int index, int len = -1)
        {
            RuleType = ruleType;
            Index = index;
            Len = len;
        }

        public Rule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, ERuleType ruleType, int index, int len = -1)
        {
            TypeA = typeA;
            CmpTypeB = cmpTypeB;
            TypeB = typeB;
            RuleType = ruleType;
            Index = index;
            Len = len;
        }

        public Rule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, ERuleType ruleType, List<Rule> children)
        {
            TypeA = typeA;
            CmpTypeB = cmpTypeB;
            TypeB = typeB;
            RuleType = ruleType;
            Index = index;
            Children = children;
        }

        public Rule(byte typeA, ECmpTypeB cmpTypeB, byte typeB, ERuleType ruleType, List<Rule> children, List<int> strIndex, 
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

    internal enum ERuleType
    {
        Skip,
        Remove,
        Parent,
        Loop

    }
}
