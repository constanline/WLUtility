using System;
using System.Collections.Generic;
using System.Linq;
using WLUtility.Helper;
using static WLUtility.Helper.SocketHelper;

namespace WLUtility.Data
{
    internal static class PacketAnalyzer
    {
        internal static byte XorByte = 0xAD;
        internal static byte[] HeadByte = {0x10, 0x6B};

        //private static List<RuleWithType> _listRules = InitRules();
        private static Dictionary<byte, Dictionary<byte, Rule>> _dicRules;

        private static void AddRule(byte typeA, byte typeB, Rule rule)
        {
            if(!_dicRules.ContainsKey(typeA))
                _dicRules.Add(typeA, new Dictionary<byte, Rule>());

            _dicRules[typeA][typeB] = rule;
        }

        public static void InitRules()
        {
            if (_dicRules != null) return;

            _dicRules = new Dictionary<byte, Dictionary<byte, Rule>>();
            var skipRule = Rule.BuildSkipRule();
            AddRule(1, 30, skipRule);
            AddRule(1, 31, skipRule);
            AddRule(1, 32, skipRule);

            //在线人员信息？记不清了
            AddRule(4, 0, Rule.BuildRemoveRule(-1, 3));

            //人物信息
            AddRule(5, 3, Rule.BuildRemoveRule(66, 8));
            
            //战斗
            var children = new List<Rule>(2);
            var rule = Rule.BuildRemoveRule(41);
            children.Add(rule);
            rule = Rule.BuildRemoveRule(6, 1);
            children.Add(rule);
            AddRule(11, 5, Rule.BuildParentRule(children));

            //战斗队友
            children = new List<Rule>();
            rule = Rule.BuildRemoveRule(30, 27, 30, new List<int> { 44 });
            children.Add(rule);
            rule = Rule.BuildLoopRule(children, 9, 8);
            children = new List<Rule> { rule, Rule.BuildRemoveRule(8, 1) };
            AddRule(11, 250, Rule.BuildParentRule(children));

            //好友
            children = new List<Rule> { Rule.BuildRemoveRule(25, 1, 25, new List<int> { 4, 19, 20, 25 }) };
            AddRule(14, 5, Rule.BuildLoopRule(children, 6));

            //宠物
            children = new List<Rule> { Rule.BuildRemoveRule(181, 2, 181,new List<int> { 29 }) };
            AddRule(15, 8, Rule.BuildLoopRule(children, 6));
        }

        //List Way
        //static void HandleRule(RuleWithType rule, byte typeA, byte typeB, List<byte> listBuffer, ref int len, ref bool isSkip, ref int offset)
        //{
        //    if (typeA == rule.TypeA)
        //    {
        //        if ((rule.CmpTypeB == ECmpTypeB.None) ||
        //            (rule.CmpTypeB == ECmpTypeB.Equal && typeB == rule.TypeB) ||
        //            (rule.CmpTypeB == ECmpTypeB.MoreThen && typeB > rule.TypeB) ||
        //            (rule.CmpTypeB == ECmpTypeB.LessThen && typeB < rule.TypeB))
        //        {
        //            if (rule.RuleType == ERuleType.Skip)
        //            {
        //                isSkip = true;
        //            }
        //            else if (rule.RuleType == ERuleType.Remove)
        //            {
        //                if (rule.Index <= 0 && rule.Len <= 0)
        //                {
        //                }
        //                else if (rule.Index <= 0)
        //                {
        //                    len -= rule.Len;
        //                    listBuffer.RemoveRange(len, rule.Len);
        //                }
        //                else if (rule.Len <= 0)
        //                {
        //                    listBuffer.RemoveRange(rule.Index, len - rule.Index);
        //                    len = rule.Index;
        //                }
        //                else
        //                {
        //                    len -= rule.Len;
        //                    listBuffer.RemoveRange(rule.Index, rule.Len);
        //                }

        //                if (rule.Offset > 0)
        //                {
        //                    offset += rule.Offset;
        //                }
        //            }
        //            else if (rule.RuleType == ERuleType.Parent)
        //            {
        //                foreach (var childRule in rule.Children)
        //                {
        //                    HandleRule(childRule, typeA, typeB, listBuffer, ref len, ref isSkip, ref offset);
        //                }
        //            }
        //            else if (rule.RuleType == ERuleType.Loop)
        //            {
        //                offset += rule.Offset;
        //                while (offset < len)
        //                {
        //                    foreach (var childRule in rule.Children)
        //                    {
        //                        HandleRule(childRule, typeA, typeB, listBuffer, ref len, ref isSkip, ref offset);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //Dic Way
        private static void HandleRule(Rule rule, List<byte> listBuffer, ref int len, ref bool isSkip, ref int offset)
        {
            switch (rule.RuleType)
            {
                case Rule.ERuleType.Skip:
                    isSkip = true;
                    break;
                case Rule.ERuleType.Remove:
                {
                    if (rule.Index > 0 || rule.Len > 0)
                    {
                        int removeLen;

                        var startIndex = rule.Offset > 0 ? offset : 0;

                        if (rule.Index <= 0)
                        {
                            startIndex += len - rule.Len;
                            removeLen = rule.Len;
                        }
                        else if (rule.Len <= 0)
                        {
                            startIndex += rule.Index;
                            removeLen = len - rule.Index;
                        }
                        else
                        {
                            startIndex += rule.Index;
                            removeLen = rule.Len;
                        }

                        if (rule.StrIndex != null)
                        {
                            foreach (var strIdx in rule.StrIndex)
                            {
                                var strLen = listBuffer[offset + strIdx] ^ XorByte;
                                if (strIdx < startIndex)
                                {
                                    startIndex += strLen;
                                    offset += strLen;
                                }
                                else if (strIdx > startIndex + removeLen)
                                    offset += strLen;
                                else
                                    removeLen += strLen;
                            }
                        }

                        len -= removeLen;
                        listBuffer.RemoveRange(startIndex, removeLen);
                    }

                    if (rule.Offset > 0)
                    {    
                        offset += rule.Offset;
                    }

                    break;
                }
                case Rule.ERuleType.Parent:
                {
                    foreach (var childRule in rule.Children)
                    {
                        offset = 0;
                        HandleRule(childRule, listBuffer, ref len, ref isSkip, ref offset);
                    }

                    break;
                }
                case Rule.ERuleType.Loop:
                {
                    var totalCount = int.MaxValue;
                    if (rule.Index > 0)
                    {
                        totalCount = listBuffer[rule.Index] ^ XorByte;
                    }
                    if (rule.Offset > 0)
                    {
                        offset += rule.Offset;
                    }

                    var runCount = 0;
                    while (offset < len && runCount < totalCount)
                    {
                        runCount++;
                        foreach (var childRule in rule.Children)
                        {
                            HandleRule(childRule, listBuffer, ref len, ref isSkip, ref offset);
                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static byte LastTypeA = 0, LastTypeB = 0;

        internal static void AnalyzePacket(IEnumerable<byte> data, SocketPair socketPair)
        {
            while (true)
            {
                var isFull = false;
                lock (socketPair.ObjLocker)
                {
                    if (data != null) socketPair.ListBuffer.AddRange(data);

                    var totalLen = socketPair.ListBuffer.LongCount();
                    if (totalLen >= 5)
                    {
                        if (socketPair.ListBuffer[0] == HeadByte[0] && socketPair.ListBuffer[1] == HeadByte[1])
                        {
                            var len = (socketPair.ListBuffer[2] ^ XorByte) + (socketPair.ListBuffer[3] ^ XorByte) * 0x100 + 4;
                            if (totalLen >= len)
                            {
                                var isSkip = false;
                                isFull = true;
                                if (len >= 5)
                                {
                                    var typeA = (byte) (socketPair.ListBuffer[4] ^ XorByte);
                                    byte typeB;
                                    if (len > 5)
                                        typeB = (byte) (socketPair.ListBuffer[5] ^ XorByte);
                                    else
                                        typeB = 0;

                                    var offset = 0;

                                    if (_dicRules.ContainsKey(typeA))
                                    {
                                        if (_dicRules[typeA].ContainsKey(0))
                                        {
                                            LastTypeA = typeA;
                                            LastTypeB = 0;
                                            HandleRule(_dicRules[typeA][0], socketPair.ListBuffer, ref len, ref isSkip, ref offset);
                                        }
                                        else if (_dicRules[typeA].ContainsKey(typeB))
                                        {
                                            LastTypeA = typeA;
                                            LastTypeB = typeB;
                                            HandleRule(_dicRules[typeA][typeB], socketPair.ListBuffer, ref len, ref isSkip, ref offset);
                                        }
                                    }
                                    

                                    var byteLen = BitConverter.GetBytes((short) (len - 4));
                                    socketPair.ListBuffer[2] = (byte) (byteLen[0] ^ XorByte);
                                    socketPair.ListBuffer[3] = (byte) (byteLen[1] ^ XorByte);
                                }

                                while (len > 0) len -= SendPacket(socketPair, len, isSkip);
                            }
                        }
                        else
                        {
                            Console.WriteLine(BitConverter.ToString(LastPacket, 0).Replace("-", string.Empty).ToLower());
                        }
                    }
                }

                if (isFull)
                {
                    data = null;
                    continue;
                }

                break;
            }
        }
    }
}