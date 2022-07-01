using System;
using System.Collections.Generic;
using System.Linq;
using WLUtility.Helper;

namespace WLUtility.Core
{
    internal abstract class BaseSocket
    {
        public static readonly byte XOR_BYTE = 0xA8;
        public static readonly byte[] HEAD_BYTE = { 0x15, 0x6E };

        protected object ObjLocker;

        protected List<byte> ListBuffer;

        protected byte[] LastPacket;

        protected int SocketId;

        public abstract bool Connected { get; }

        protected virtual void RevMessage(int len)
        {
            var buffer = new byte[0];
            lock (ListBuffer)
            {
                LastPacket = ListBuffer.Take(len).ToArray();
                
                ListBuffer.RemoveAt(0);
                if (len > 5)
                {
                    buffer = ListBuffer.Take(len - 5).ToArray();
                    ListBuffer.RemoveRange(0, len - 1);
                }
            }
            if (GlobalSetting.RecordPacket)
            {
                LogHelper.LogPacket(LastPacket, false);
            }
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(buffer[i] ^ XOR_BYTE);
            }
        }

        protected BaseSocket()
        {
            ObjLocker = new object();
            ListBuffer = new List<byte>();
        }

        // protected abstract void Init();
        //
        // protected abstract int Receive(byte[] buffer);
        //
        // protected void ReceiveHandler(object obj)
        // {
        //     while (Connected)
        //     {
        //         try
        //         {
        //             var data = new byte[1024];
        //             var read = Receive(data);
        //             if (read > 0)
        //             {
        //                 var tmp = new byte[read];
        //                 Array.Copy(data, tmp, tmp.Length);
        //                 ReceivePacket(tmp);
        //             }
        //             else
        //             {
        //                 break;
        //             }
        //         }
        //         catch (Exception)
        //         {
        //             break;
        //         }
        //     }
        //
        //     SocketEngine.StopSocket(SocketId);
        // }

        public abstract void Close();

        public void ReceivePacket(byte[] data)
        {
            while (true)
            {
                lock (ListBuffer)
                {
                    if (data != null)
                    {
                        ListBuffer.AddRange(data);
                        data = null;
                    }

                    var totalLen = ListBuffer.LongCount();
                    if (totalLen >= 5)
                    {
                        if (ListBuffer[0] == HEAD_BYTE[0] && ListBuffer[1] == HEAD_BYTE[1])
                        {
                            var len = (ListBuffer[2] ^ XOR_BYTE) + ((ListBuffer[3] ^ XOR_BYTE) << 8) + 4;
                            if (totalLen >= len)
                            {
                                RevMessage(len);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(BitConverter.ToString(LastPacket.ToArray(), 0).Replace("-", string.Empty).ToLower());
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}