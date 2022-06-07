using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.Helper;
using WLUtility.Model;

namespace WLUtility.Engine
{
    internal class PetProcesser : IProcesser
    {
        private readonly ProxySocket _socket;

        public PetProcesser(ProxySocket socket)
        {
            _socket = socket;
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0x0F)
            {
                if (bType == 0x08)
                {
                    var idx = 6;
                    while (idx< packet.Count)
                    {
                        var petPos = ByteUtil.ReadPacket<byte>(packet, ref idx);
                        var id = ByteUtil.ReadPacket<ushort>(packet, ref idx);
                        var pet = new Pet();
                        pet.Id = id;
                        idx += 26;
                        var len = ByteUtil.ReadPacket<byte>(packet, ref idx);
                        pet.Name = ByteUtil.ReadPacket<string>(packet, ref idx, len);
                        idx += 15;

                        for (var i = 1; i <= 6; i++)
                        {
                            pet.Equips[i].Id = ByteUtil.ReadPacket<ushort>(packet, ref idx);
                            pet.Equips[i].Damage = ByteUtil.ReadPacket<byte>(packet, ref idx);
                            if (pet.Equips[i].Id > 0)
                            {
                                pet.Equips[i].Qty = 1;
                            }
                            idx += 19;
                        }

                        idx += 6;
                        _socket.PlayerInfo.Pets[petPos] = pet;
                    }
                }
            }
        }
    }
}