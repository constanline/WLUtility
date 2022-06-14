using System;
using System.Collections.Generic;
using System.Threading;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.Engine
{
    internal class WoodManProcesser : IProcesser, IDisposable
    {
        private const int DELAY_TIME = 8 * 60 * 60 * 1000;
        private readonly ProxySocket _socket;

        private readonly Timer _timer;

        public WoodManProcesser(ProxySocket socket)
        {
            _socket = socket;
            _timer = new Timer(ResetWoodMan, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Handle(byte aType, byte bType, List<byte> packet, ref bool isSkip)
        {
            if (aType == 0x69)
            {
                if (bType == 0x02)
                {
                    var iPos = 6;
                    var playerId = ByteUtil.ReadPacket<int>(packet, ref iPos);
                    if (playerId == _socket.PlayerInfo.Id)
                    {
                        if (_socket.WoodManInfo.IsSetEventId())
                        {
                            _socket.Log("检测到木人挂机，8小时后自动重置[木人序号:" + _socket.WoodManInfo.EventNo + "]");
                            _timer.Change(DELAY_TIME, Timeout.Infinite);
                        }
                        else
                        {
                            _socket.Log("检测到木人挂机，但没有设置木人ID");
                        }
                    }
                }
            }
        }

        private void ResetWoodMan(object obj)
        {
            _socket.WoodManInfo.StartWoodMan();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}