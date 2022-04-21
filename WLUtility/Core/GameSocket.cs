using System;
using System.Net.Sockets;
using System.Threading;
using WLUtility.Engine;
using WLUtility.Helper;

namespace WLUtility.Core
{
    internal class GameSocket : BaseSocket
    {
        private readonly Socket _remoteSocket;

        public GameSocket(Socket remoteSocket) : base()
        {
            _remoteSocket = remoteSocket;

            ThreadPool.QueueUserWorkItem(e =>
            {
                while (Connected)
                {
                    try
                    {
                        var data = new byte[1024];
                        var read = _remoteSocket.Receive(data);
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
            return _remoteSocket.Send(buffer);
        }

        public override bool Connected => _remoteSocket.Connected;

        public override void Close()
        {
            _remoteSocket.Close();
            _remoteSocket.Dispose();
        }
    }
}