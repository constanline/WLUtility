using System.Collections.Generic;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class PlayerProcesser : IProcesser
    {
        private readonly ProxySocket _socket;

        public PlayerProcesser(ProxySocket socket)
        {
            _socket = socket;
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0x01)
            {
                if (bType == 0x04)
                {
                    var idx = 6;
                    _socket.PlayerInfo.Id = ByteUtil.ReadPacket<int>(packet, ref idx);
                    var len = ByteUtil.ReadPacket<byte>(packet, ref idx);
                    _socket.PlayerInfo.Username = ByteUtil.ReadPacket<string>(packet, ref idx, len);
                    _socket.RevLoginRole();
                }
            }
            if (aType == 0x03)
            {
                var idx = 5;
                var playerId = ByteUtil.ReadPacket<int>(packet, ref idx);
                var mainId = playerId <= 4500000 ? playerId : playerId - 4500000;
                if (mainId == _socket.PlayerInfo.Id)
                {
                    _socket.PlayerInfo.Id = playerId;
                }
            }
        }
    }
}