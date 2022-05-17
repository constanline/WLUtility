using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using WLUtility.Engine;
using WLUtility.Helper;

namespace WLUtility.Core
{
    internal class ProxySocket : BaseSocket
    {
        public readonly Socket LocalSocket;

        public readonly Socket RemoteSocket;

        private static Dictionary<byte, Dictionary<byte, Rule>> _dicRules;

        private static readonly byte _xorByte = 0x00;

        private static void AddRule(byte typeA, byte typeB, Rule rule)
        {
            if (!_dicRules.ContainsKey(typeA))
                _dicRules.Add(typeA, new Dictionary<byte, Rule>());

            _dicRules[typeA][typeB] = rule;
        }

        public static void InitRules()
        {
            if (_dicRules != null) return;
            _dicRules = new Dictionary<byte, Dictionary<byte, Rule>>();

            //var skipRule = Rule.BuildSkipRule();
            //AddRule(1, 30, skipRule);
            //AddRule(1, 31, skipRule);
            //AddRule(1, 32, skipRule);

            ////在线人员信息？记不清了
            //AddRule(4, 0, Rule.BuildRemoveRule(-1, 3));

            ////战斗
            //var children = new List<Rule>(2);
            //var rule = Rule.BuildRemoveRule(41);
            //children.Add(rule);
            //rule = Rule.BuildRemoveRule(6, 1);
            //children.Add(rule);
            //AddRule(11, 5, Rule.BuildParentRule(children));

            ////战斗队友
            //children = new List<Rule>();
            //rule = Rule.BuildRemoveRule(30, 27, 30, new List<int> { 44 });
            //children.Add(rule);
            //rule = Rule.BuildLoopRule(children, 9, 8);
            //children = new List<Rule> { rule, Rule.BuildRemoveRule(8, 1) };
            //AddRule(11, 250, Rule.BuildParentRule(children));

            ////好友
            //children = new List<Rule> { Rule.BuildRemoveRule(25, 1, 25, new List<int> { 4, 19, 20, 25 }) };
            //AddRule(14, 5, Rule.BuildLoopRule(children, 6));


            // 4.09不再需要
            //人物信息
            // AddRule(5, 3, Rule.BuildRemoveRule(66, 8));
            //宠物
            // children = new List<Rule> { Rule.BuildRemoveRule(181, 2, 181,new List<int> { 29 }) };
            // AddRule(15, 8, Rule.BuildLoopRule(children, 6));


        }

        private static void HandleRule(Rule rule, List<byte> listBuffer, ref int len, ref bool isSkip, ref int offset)
        {
            switch (rule.RuleType)
            {
                case Rule.ERuleType.Skip:
                    isSkip = true;
                    break;
                case Rule.ERuleType.Add:
                    var insStartIndex = rule.Offset > 0 ? offset : 0;

                    if (rule.Index <= 0)
                    {
                        insStartIndex += len - rule.Len;
                    }
                    else if (rule.Len <= 0)
                    {
                        insStartIndex += rule.Index;
                    }
                    else
                    {
                        insStartIndex += rule.Index;
                    }

                    if (rule.StrIndex != null)
                    {
                        foreach (var strIdx in rule.StrIndex)
                        {
                            var strLen = listBuffer[offset + strIdx] ^ XOR_BYTE;
                            if (strIdx < insStartIndex)
                            {
                                insStartIndex += strLen;
                            }
                        }
                    }
                    listBuffer.InsertRange(insStartIndex, rule.AddBuffer);
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
                                    var strLen = listBuffer[offset + strIdx] ^ XOR_BYTE;
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
                            totalCount = listBuffer[rule.Index] ^ XOR_BYTE;
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

            if (rule.NextRule != null)
            {
                HandleRule(rule.NextRule, listBuffer, ref len, ref isSkip, ref offset);
            }
        }

        private static byte[] XorByte(IEnumerable<byte> buffer, int len = 0)
        {
            var bytes = buffer as byte[] ?? buffer.ToArray();
            if (len == 0)
            {
                len = bytes.Count();
            }
            var result = bytes.Take(len).ToArray();
            if (_xorByte == 0) return result;
            for (var i = 0; i < len; i++)
                result[i] ^= _xorByte;
            return result;
        }

        public ProxySocket(Socket localSocket, Socket remoteSocket)
        {
            LocalSocket = localSocket;
            RemoteSocket = remoteSocket;

            ThreadPool.QueueUserWorkItem(e =>
            {
                while (Connected)
                {
                    // if (!_isReceive)
                    // {
                    //     Thread.Sleep(50);
                    //     continue;
                    // }
                    try
                    {
                        var data = new byte[1024];
                        var read = LocalSocket.Receive(data);
                        if (read > 0)
                            SendPacket(XorByte(data, read));
                        else
                            break;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                SocketEngine.StopSocket(SocketId);
            });
            ThreadPool.QueueUserWorkItem(e =>
            {
                while (Connected)
                {
                    try
                    {
                        var data = new byte[1024];
                        var read = RemoteSocket.Receive(data);
                        if (read > 0)
                        {
                            if (read < data.Length)
                            {
                                var tmp = new byte[read];
                                Array.Copy(data, tmp, tmp.Length);
                                ReceivePacket(tmp);
                            }
                            else
                            {
                                ReceivePacket(data);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                SocketEngine.StopSocket(SocketId);
            });
        }

        public int SendPacket(byte[] buffer)
        {
            if (GlobalSetting.RecordPacket)
            {
                LogHelper.LogPacket(buffer, true);
            }
            return RemoteSocket.Send(buffer);
        }

        public override bool Connected => LocalSocket.Connected && RemoteSocket.Connected;

        List<int> listHp = new List<int>() { 34281, 34330, 34339 };
        List<int> listSp = new List<int>() { 34282, 34331, 34340 };

        protected override void RevMessage(int len)
        {
            base.RevMessage(len);

            var packet = LastPacket.ToList();

            var isSkip = false;
            var aType = (byte)(packet[4] ^ XOR_BYTE);

            byte bType;
            if (len > 5)
                bType = (byte)(packet[5] ^ XOR_BYTE);
            else
                bType = 0;

            //var offset = 0;
            //if (_dicRules.ContainsKey(aType))
            //{
            //    if (_dicRules[aType].ContainsKey(0))
            //    {
            //        HandleRule(_dicRules[aType][0], packet, ref len, ref isSkip, ref offset);
            //    }
            //    else if (_dicRules[aType].ContainsKey(bType))
            //    {
            //        HandleRule(_dicRules[aType][bType], packet, ref len, ref isSkip, ref offset);
            //    }
            //}
            if (aType == 23)
            {
                if(bType == 5)
                {
                    var offset = 7;
                    var num = 0;
                    while(num * 32 + offset < len)
                    {
                        var tmp = num * 32 + offset;
                        var id = (packet[tmp] + (packet[tmp + 1] << 8)) ^ 0xA8A8;
                        if (listHp.Contains(id))
                        {
                            //34285
                            packet[tmp] = 0x45;
                            packet[tmp + 1] = 0x2D;
                        }
                        else if (listSp.Contains(id))
                        {
                            //34286
                            packet[tmp] = 0x46;
                            packet[tmp + 1] = 0x2D;
                        }
                        num++;
                    }
                }
            }

            if (!isSkip)
            {
                LocalSocket.Send(XorByte(packet));
            }
        }

        public override void Close()
        {
            if (LocalSocket.Connected)
            {
                LocalSocket.Close();
                LocalSocket.Dispose();
            }

            if (RemoteSocket.Connected)
            {
                RemoteSocket.Close();
                RemoteSocket.Dispose();
            }
        }
    }
}