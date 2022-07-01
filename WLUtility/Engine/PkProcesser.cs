using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class PkProcesser : IProcesser
    {

        private readonly ProxySocket _socket;

        public PkProcesser(ProxySocket socket)
        {
            _socket = socket;
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0xB7)
            {
                if (bType == 0x03)
                {
                    RevPkInfo(packet);
                }
                else if (bType == 0x05)
                {
                    var iPos = 6;
                    var id = ByteUtil.ReadPacket<int>(packet, ref iPos);
                    if (_socket.PlayerInfo.Id == id)
                    {
                        _socket.PlayerInfo.TotalWin = ByteUtil.ReadPacket<int>(packet, ref iPos);
                        _socket.PlayerInfo.CurrentWin = ByteUtil.ReadPacket<ushort>(packet, ref iPos);
                        _socket.PlayerInfo.Rank = ByteUtil.ReadPacket<int>(packet, ref iPos);
                    }
                }
                else if (bType == 0x0B)
                {
                    var iPos = 6;
                    var cType = ByteUtil.ReadPacket<byte>(packet, ref iPos);
                    switch (cType)
                    {
                        case 0x04:
                        {
                            var idx = ByteUtil.ReadPacket<byte>(packet, ref iPos);
                            _socket.PlayerInfo.TreasureKind[idx] = ByteUtil.ReadPacket<ushort>(packet, ref iPos);
                            _socket.PlayerInfo.TreasureState[idx] = ByteUtil.ReadPacket<byte>(packet, ref iPos);
                            _socket.PlayerInfo.UpdateTreasure();
                            break;
                        }
                        case 0x05:
                        {
                            for (var i = 1; i <= 3; i++)
                            {
                                _socket.PlayerInfo.TreasureState[i] = ByteUtil.ReadPacket<byte>(packet, ref iPos);
                                _socket.PlayerInfo.TreasureKind[i] = ByteUtil.ReadPacket<ushort>(packet, ref iPos);
                            }
                            _socket.PlayerInfo.UpdateTreasure();
                            break;
                        }
                        case 0x09:
                            break;
                        default:
                            LogHelper.LogPacket(packet.ToArray(), false);
                            break;
                    }
                }
                else if (bType == 0x0C)
                {
                    var iPos = 6;
                    if (ByteUtil.ReadPacket<byte>(packet, ref iPos) == 1)
                    {
                        _socket.PlayerInfo.UpdateEnergy(ByteUtil.ReadPacket<byte>(packet, ref iPos));
                    }
                }
            }
        }

        private void RevPkInfo(List<byte> packet)
        {
            var iPos = 6;
            _socket.PlayerInfo.OfflineFlag = ByteUtil.ReadPacket<byte>(packet, ref iPos);
            ByteUtil.ReadPacket<int>(packet, ref iPos);
            ByteUtil.ReadPacket<ushort>(packet, ref iPos);
            for (var i = 1; i <= 3; i++)
            {
                _socket.PlayerInfo.TreasureState[i] = ByteUtil.ReadPacket<byte>(packet, ref iPos);
                _socket.PlayerInfo.TreasureKind[i] = ByteUtil.ReadPacket<ushort>(packet, ref iPos);
            }

            _socket.SendPacket(new PacketBuilder(0xB7, 0x09).Build());
            _socket.PlayerInfo.UpdateTreasure();
        }
    }
}
