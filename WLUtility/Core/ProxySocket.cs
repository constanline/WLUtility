using Magician.Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using WLUtility.CustomControl;
using WLUtility.Engine;
using WLUtility.Helper;
using WLUtility.Model;

namespace WLUtility.Core
{
    internal class ProxySocket : BaseSocket
    {
        public readonly Socket LocalSocket;

        public readonly Socket RemoteSocket;

        public event Action<ProxySocket> RoleLoginFinish;

        public event Action<BagItem[]> BagUpdated;

        public event Action AutoSellItemUpdated;

        public PlayerInfo PlayerInfo { get; }

        public WoodManInfo WoodManInfo { get; }

        private static Dictionary<byte, Dictionary<byte, Rule>> _dicRules;

        private readonly List<IProcesser> _processes;

        private ILogger _logger;

        private RoleControl _roleControl;

        public void Log(string msg)
        {
            _logger?.LogWithTime(msg);
        }

        public void SetRoleControl(RoleControl roleControl)
        {
            _roleControl = roleControl;
            _logger = _roleControl.Logger;
        }

        //private static void AddRule(byte typeA, byte typeB, Rule rule)
        //{
        //    if (!_dicRules.ContainsKey(typeA))
        //        _dicRules.Add(typeA, new Dictionary<byte, Rule>());

        //    _dicRules[typeA][typeB] = rule;
        //}

        public void RevLoginRole()
        {
            RoleLoginFinish?.Invoke(this);
        }

        public void UpdateBag(BagItem[] bagItems)
        {
            BagUpdated?.Invoke(bagItems);
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

        // private static void HandleRule(Rule rule, List<byte> listBuffer, ref int len, ref bool isSkip, ref int offset)
        // {
        //     switch (rule.RuleType)
        //     {
        //         case Rule.ERuleType.Skip:
        //             isSkip = true;
        //             break;
        //         case Rule.ERuleType.Add:
        //             var insStartIndex = rule.Offset > 0 ? offset : 0;
        //
        //             if (rule.Index <= 0)
        //             {
        //                 insStartIndex += len - rule.Len;
        //             }
        //             else if (rule.Len <= 0)
        //             {
        //                 insStartIndex += rule.Index;
        //             }
        //             else
        //             {
        //                 insStartIndex += rule.Index;
        //             }
        //
        //             if (rule.StrIndex != null)
        //             {
        //                 foreach (var strIdx in rule.StrIndex)
        //                 {
        //                     var strLen = listBuffer[offset + strIdx] ^ XOR_BYTE;
        //                     if (strIdx < insStartIndex)
        //                     {
        //                         insStartIndex += strLen;
        //                     }
        //                 }
        //             }
        //             listBuffer.InsertRange(insStartIndex, rule.AddBuffer);
        //             break;
        //         case Rule.ERuleType.Remove:
        //             {
        //                 if (rule.Index > 0 || rule.Len > 0)
        //                 {
        //                     int removeLen;
        //
        //                     var startIndex = rule.Offset > 0 ? offset : 0;
        //
        //                     if (rule.Index <= 0)
        //                     {
        //                         startIndex += len - rule.Len;
        //                         removeLen = rule.Len;
        //                     }
        //                     else if (rule.Len <= 0)
        //                     {
        //                         startIndex += rule.Index;
        //                         removeLen = len - rule.Index;
        //                     }
        //                     else
        //                     {
        //                         startIndex += rule.Index;
        //                         removeLen = rule.Len;
        //                     }
        //
        //                     if (rule.StrIndex != null)
        //                     {
        //                         foreach (var strIdx in rule.StrIndex)
        //                         {
        //                             var strLen = listBuffer[offset + strIdx] ^ XOR_BYTE;
        //                             if (strIdx < startIndex)
        //                             {
        //                                 startIndex += strLen;
        //                                 offset += strLen;
        //                             }
        //                             else if (strIdx > startIndex + removeLen)
        //                                 offset += strLen;
        //                             else
        //                                 removeLen += strLen;
        //                         }
        //                     }
        //
        //                     len -= removeLen;
        //                     listBuffer.RemoveRange(startIndex, removeLen);
        //                 }
        //
        //                 if (rule.Offset > 0)
        //                 {
        //                     offset += rule.Offset;
        //                 }
        //
        //                 break;
        //             }
        //         case Rule.ERuleType.Parent:
        //             {
        //                 foreach (var childRule in rule.Children)
        //                 {
        //                     offset = 0;
        //                     HandleRule(childRule, listBuffer, ref len, ref isSkip, ref offset);
        //                 }
        //
        //                 break;
        //             }
        //         case Rule.ERuleType.Loop:
        //             {
        //                 var totalCount = int.MaxValue;
        //                 if (rule.Index > 0)
        //                 {
        //                     totalCount = listBuffer[rule.Index] ^ XOR_BYTE;
        //                 }
        //                 if (rule.Offset > 0)
        //                 {
        //                     offset += rule.Offset;
        //                 }
        //
        //                 var runCount = 0;
        //                 while (offset < len && runCount < totalCount)
        //                 {
        //                     runCount++;
        //                     foreach (var childRule in rule.Children)
        //                     {
        //                         HandleRule(childRule, listBuffer, ref len, ref isSkip, ref offset);
        //                     }
        //                 }
        //
        //                 break;
        //             }
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        //
        //     if (rule.NextRule != null)
        //     {
        //         HandleRule(rule.NextRule, listBuffer, ref len, ref isSkip, ref offset);
        //     }
        // }

        private static byte[] ToBuffer(IEnumerable<byte> buffer, int len = 0)
        {
            var bytes = buffer as byte[] ?? buffer.ToArray();
            if (len == 0)
            {
                len = bytes.Count();
            }
            return bytes.Take(len).ToArray();
        }

        public ProxySocket(Socket localSocket, Socket remoteSocket)
        {
            _processes = new List<IProcesser>
            {
                new ItemProcesser(this),
                new SignProcesser(this),
                new LotteryProcesser(this),
                new PlayerProcesser(this),
                new WoodManProcesser(this),
                new PetProcesser(this)
            };
            PlayerInfo = new PlayerInfo(this);
            WoodManInfo = new WoodManInfo(this);

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
                            DirectSendPacket(ToBuffer(data, read));
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
                    catch (Exception ex)
                    {
                        LogHelper.SilentLog(ex);
                        break;
                    }
                }

                SocketEngine.StopSocket(SocketId);
            });
        }

        public int DirectSendPacket(byte[] packet)
        {
            if (GlobalSetting.RecordPacket)
            {
                LogHelper.LogPacket(packet, true);
            }
            return RemoteSocket.Send(packet);
        }

        public int SendPacket(byte[] buffer)
        {
            var cLen = buffer.Length;
            var packet = new byte[cLen + 4];
            packet[0] = HEAD_BYTE[0];
            packet[1] = HEAD_BYTE[1];
            packet[2] = (byte)(cLen & 0xFF);
            packet[3] = (byte)(cLen >> 8);
            Array.Copy(buffer, 0, packet, 4, buffer.Length);
            for(var i = 2; i < packet.Length; i++)
            {
                packet[i] ^= XOR_BYTE;
            }
            return DirectSendPacket(packet);
        }

        public int RevPacket(byte[] buffer)
        {
            var cLen = buffer.Length;
            var packet = new byte[cLen + 4];
            packet[0] = HEAD_BYTE[0];
            packet[1] = HEAD_BYTE[1];
            packet[2] = (byte)(cLen & 0xFF);
            packet[3] = (byte)(cLen >> 8);
            Array.Copy(buffer, 0, packet, 4, buffer.Length);
            for (var i = 2; i < packet.Length; i++)
            {
                packet[i] ^= XOR_BYTE;
            }
            return LocalSocket.Send(packet);
        }


        public override bool Connected => LocalSocket.Connected && RemoteSocket.Connected;

        protected override void RevMessage(int len)
        {
            base.RevMessage(len);

            var packet = LastPacket.ToList();

            var aType = (byte)(packet[4] ^ XOR_BYTE);

            byte bType;
            if (len > 5)
                bType = (byte)(packet[5] ^ XOR_BYTE);
            else
                bType = 0;

            var isSkip = false;
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
            foreach (var process in _processes)
            {
                process.Handle(aType, bType, packet, ref isSkip);
                if (isSkip)
                    break;
            }

            if (!isSkip)
            {
                LocalSocket.Send(ToBuffer(packet));
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