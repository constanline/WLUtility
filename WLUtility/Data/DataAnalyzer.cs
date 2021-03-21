using System;
using System.Collections.Generic;
using System.Linq;
using static WLUtility.Helper.SocketHelper;

namespace WLUtility.Data
{
    class DataAnalyzer
    {

        private static readonly byte xorByte = 0xAD;

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
                        if ((socketPair.ListBuffer[0] == 0x10 && socketPair.ListBuffer[1] == 0x6B) ||
                            (socketPair.ListBuffer[0] == 0x59 && socketPair.ListBuffer[1] == 0xE9))
                        {
                            int len = (socketPair.ListBuffer[2] ^ xorByte) + (socketPair.ListBuffer[3] ^ xorByte) * 0x100 + 4;
                            if (totalLen >= len)
                            {
                                bool isSkip = false;
                                isFull = true;
                                if (len >= 6)
                                {
                                    byte typeA = (byte)(socketPair.ListBuffer[4] ^ xorByte);
                                    byte typeB = (byte)(socketPair.ListBuffer[5] ^ xorByte);
                                    if (typeA == 4)
                                    {
                                        len -= 3;
                                        socketPair.ListBuffer.RemoveRange(len, 3);
                                        //isSkip = true;
                                    }
                                    else if (typeA == 1 && typeB >= 30)
                                    {
                                        isSkip = true;
                                    }
                                    else if (typeA == 5 && typeB == 3)
                                    {
                                        len -= 8;
                                        socketPair.ListBuffer.RemoveRange(66, 8);
                                    }
                                    else if (typeA == 11)
                                    {
                                        if (typeB == 5)
                                        {
                                            var removeLen = len - 4 - 36;
                                            len -= removeLen;
                                            socketPair.ListBuffer.RemoveRange(41, removeLen - 1);
                                            socketPair.ListBuffer.RemoveRange(6, 1);
                                        }
                                        else if (typeB == 250)
                                        {
                                            var currentPos = 8;
                                            var count = socketPair.ListBuffer[currentPos] ^ xorByte;
                                            socketPair.ListBuffer.RemoveRange(currentPos, 1);
                                            var removeLen = 1;
                                            for (var i = 0; i < count; i++)
                                            {
                                                currentPos += 30;
                                                var nameLen = socketPair.ListBuffer[currentPos + 14] ^ xorByte;
                                                socketPair.ListBuffer.RemoveRange(currentPos, 15 + nameLen + 12);
                                                removeLen += 15 + nameLen + 12;
                                            }
                                            len -= removeLen;
                                        }
                                    }
                                    else if (typeA == 14)
                                    {
                                        if (typeB == 5)
                                        {
                                            var removeLen = 0;
                                            var currentPos = 6;
                                            while (currentPos + removeLen < len)
                                            {
                                                currentPos += 4;
                                                var nameLen = socketPair.ListBuffer[currentPos] ^ xorByte;
                                                currentPos += 1 + nameLen + 14;

                                                //NickName
                                                nameLen = socketPair.ListBuffer[currentPos] ^ xorByte;
                                                currentPos += 1 + nameLen;

                                                //OrgName
                                                nameLen = socketPair.ListBuffer[currentPos] ^ xorByte;
                                                currentPos += 1 + nameLen + 4;

                                                //此处缺数据，不确定是Byte(StrLen)+Str还是Byte，暂时认为Byte(StrLen)+Str
                                                nameLen = socketPair.ListBuffer[currentPos] ^ xorByte;

                                                socketPair.ListBuffer.RemoveRange(currentPos, 1 + nameLen);
                                                removeLen += 1 + nameLen;
                                            }
                                            len -= removeLen;
                                        }
                                    }
                                    else if (typeA == 15 && typeB == 8)
                                    {
                                        var removeLen = 0;
                                        var currentPos = 6;
                                        var count = len / 183;
                                        for (var i = 0; i < count; i++)
                                        {
                                            currentPos += 29;
                                            var nameLen = socketPair.ListBuffer[currentPos] ^ xorByte;
                                            currentPos += nameLen + 151;
                                            socketPair.ListBuffer.RemoveRange(currentPos + 1, 2);
                                            removeLen += 2;
                                        }
                                        len -= removeLen;


                                    }
                                    byte[] byteLen = BitConverter.GetBytes((short)(len - 4));
                                    socketPair.ListBuffer[2] = (byte)(byteLen[0] ^ xorByte);
                                    socketPair.ListBuffer[3] = (byte)(byteLen[1] ^ xorByte);
                                }
                                while (len > 1024)
                                {
                                    len -= SendPacket(socketPair, 1024, isSkip);
                                }

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
