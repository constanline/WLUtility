using System;
using System.Collections.Generic;
using System.Linq;
using static WLUtility.Helper.SocketHelper;

namespace WLUtility.Data
{
    internal class PacketAnalyzer
    {
        internal static byte XorByte = 0xAD;
        internal static byte[] HeadByte = new byte[2];

        static void HandleRule(Rule rule, byte typeA, byte typeB, List<byte> listBuffer)
        {
            if (typeA == rule.TypeA)
            {
                if ((rule.CmpTypeB == ECmpTypeB.None) ||
                    (rule.CmpTypeB == ECmpTypeB.Equal && typeB == rule.TypeB) ||
                    (rule.CmpTypeB == ECmpTypeB.MoreThen && typeB > rule.TypeB) ||
                    (rule.CmpTypeB == ECmpTypeB.LessThen && typeB < rule.TypeB))
                {

                }
            }
        }

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

                                    Rule rule;
                                    switch (typeA)
                                    {
                                        case 1:
                                            if (typeB >= 30) isSkip = true;
                                            //rule = new Rule(1, ECmpTypeB.MoreThen, 29);
                                            break;
                                        case 4:
                                            len -= 3;
                                            socketPair.ListBuffer.RemoveRange(len, 3);
                                            //rule = new Rule(4, ECmpTypeB.None, 0, ERuleType.CutOff, 0, 3);
                                            break;
                                        case 5:
                                            if (typeB == 3)
                                            {
                                                len -= 8;
                                                socketPair.ListBuffer.RemoveRange(66, 8);
                                                //rule = new Rule(5, ECmpTypeB.Equal, 3, ERuleType.Remove, 66);
                                            }

                                            break;
                                        case 11:
                                        {
                                            if (typeB == 5)
                                            {
                                                var removeLen = len - 4 - 36;
                                                len -= removeLen;
                                                socketPair.ListBuffer.RemoveRange(41, removeLen - 1);
                                                socketPair.ListBuffer.RemoveRange(6, 1);
                                                //List<Rule> children = new List<Rule>();
                                                //rule = new Rule(ERuleType.Remove, 41);
                                                //children.Add(rule); 
                                                //children = new List<Rule>();
                                                //rule = new Rule(ERuleType.Remove, 6, 1);
                                                //children.Add(rule);
                                                //rule = new Rule(11, ECmpTypeB.Equal, 5, ERuleType.Parent, children);
                                            }
                                            else if (typeB == 250)
                                            {
                                                var currentPos = 8;
                                                var count = socketPair.ListBuffer[currentPos] ^ XorByte;
                                                socketPair.ListBuffer.RemoveRange(currentPos, 1);
                                                var removeLen = 1;
                                                for (var i = 0; i < count; i++)
                                                {
                                                    currentPos += 30;
                                                    var nameLen = socketPair.ListBuffer[currentPos + 14] ^ XorByte;
                                                    socketPair.ListBuffer.RemoveRange(currentPos, 15 + nameLen + 12);
                                                    removeLen += 15 + nameLen + 12;
                                                }

                                                len -= removeLen;
                                            }

                                            break;
                                        }
                                        case 14:
                                        {
                                            if (typeB == 5)
                                            {
                                                var removeLen = 0;
                                                var currentPos = 6;
                                                while (currentPos + removeLen < len)
                                                {
                                                    currentPos += 4;
                                                    var nameLen = socketPair.ListBuffer[currentPos] ^ XorByte;
                                                    currentPos += 1 + nameLen + 14;

                                                    //NickName
                                                    nameLen = socketPair.ListBuffer[currentPos] ^ XorByte;
                                                    currentPos += 1 + nameLen;

                                                    //OrgName
                                                    nameLen = socketPair.ListBuffer[currentPos] ^ XorByte;
                                                    currentPos += 1 + nameLen + 4;

                                                    //此处缺数据，不确定是Byte(StrLen)+Str还是Byte，暂时认为Byte(StrLen)+Str
                                                    nameLen = socketPair.ListBuffer[currentPos] ^ XorByte;

                                                    socketPair.ListBuffer.RemoveRange(currentPos, 1 + nameLen);
                                                    removeLen += 1 + nameLen;
                                                }

                                                len -= removeLen;
                                            }

                                            break;
                                        }
                                        case 15 when typeB == 8:
                                        {
                                            if (typeB == 8)
                                            {
                                                var removeLen = 0;
                                                var currentPos = 6;
                                                var count = len / 183;
                                                for (var i = 0; i < count; i++)
                                                {
                                                    currentPos += 29;
                                                    var nameLen = socketPair.ListBuffer[currentPos] ^ XorByte;
                                                    currentPos += nameLen + 151;
                                                    socketPair.ListBuffer.RemoveRange(currentPos + 1, 2);
                                                    removeLen += 2;
                                                }

                                                len -= removeLen;
                                            }

                                            break;
                                        }
                                    }

                                    var byteLen = BitConverter.GetBytes((short) (len - 4));
                                    socketPair.ListBuffer[2] = (byte) (byteLen[0] ^ XorByte);
                                    socketPair.ListBuffer[3] = (byte) (byteLen[1] ^ XorByte);
                                }

                                while (len > 1024) len -= SendPacket(socketPair, 1024, isSkip);

                                SendPacket(socketPair, len, isSkip);
                            }
                        }
                        else
                        {
                            Console.WriteLine(socketPair.ListBuffer[0]);
                            Console.WriteLine(socketPair.ListBuffer[1]);
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