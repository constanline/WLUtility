﻿using System;
using System.Collections.Generic;
using System.Text;
using Magician.Common.Util;
using WLUtility.Core;

namespace WLUtility.Helper
{
    public class ByteUtil
    {
        private static readonly Encoding EncodingBig5 = Encoding.GetEncoding(950);

        private static int XorConvert(List<byte> buffer, ref int idx, int len)
        {
            var tmp = 0;
            for (var i = idx + len - 1; i >= idx; i--)
            {
                tmp = (tmp << 8) + (buffer[i] ^ BaseSocket.XOR_BYTE);
            }
            idx += len;
            return tmp;
        }
        private static string XorConvertString(List<byte> buffer, ref int idx, int len)
        {
            var tmp = new byte[len];
            for (var i = 0; i < len; i++)
            {
                tmp[i] = (byte)(buffer[i + idx] ^ BaseSocket.XOR_BYTE);
            }
            idx += len;
            return EncodingBig5.GetString(tmp);
        }

        public static T ReadPacket<T>(List<byte> buffer, ref int idx, int len = 0)
        {
            T result;
            try
            {
                var resultType = typeof(T);
                if (resultType == typeof(int))
                {
                    var tmp = XorConvert(buffer, ref idx, 4);
                    result = (T)(object)tmp;
                }
                else if (resultType == typeof(ushort))
                {
                    var tmp = XorConvert(buffer, ref idx, 2);
                    result = (T)(object)(ushort)tmp;
                }
                else if (resultType == typeof(uint))
                {
                    var tmp = XorConvert(buffer, ref idx, 4);
                    result = (T)(object)(uint)tmp;
                }
                else if (resultType == typeof(byte))
                {
                    var tmp = XorConvert(buffer, ref idx, 1);
                    result = (T)(object)(byte)tmp;
                }
                else if (resultType == typeof(bool))
                {
                    var tmp = XorConvert(buffer, ref idx, 1);
                    result = (T)(object)(tmp == 1);
                }
                //else if (resultType == typeof(float))
                //{
                //    result = (T)(object)BitConverter.ToDouble(buffer, idx);
                //    idx += 8;
                //}
                else if (resultType == typeof(string))
                {
                    result = (T)(object)XorConvertString(buffer, ref idx, len);
                }
                //else if (resultType == typeof(byte[]))
                //{
                //    var tmp = new byte[len];
                //    Array.Copy(buffer, idx, tmp, 0, len);
                //    idx += len;
                //}
                //else if (resultType == typeof(double))
                //{
                //    result = (T)(object)BitConverter.ToDouble(buffer, idx);
                //    idx += 8;
                //}
                else
                {
                    throw new Exception("[ByteUtil.ReadPacket]UNKNOWN TYPE ![TRACE]" + TraceUtil.GetTraceMethodName(5));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[ByteUtil.ReadPacket]" + ex.Message + " [TRACE] " + ex.StackTrace);
            }
            return result;
        }

        public static T ReadPacket<T>(byte[] buffer, ref int idx, int len = 0)
        {
            var result = default(T);
            try
            {
                var resultType = typeof(T);
                if (resultType == typeof(int))
                {
                    result = (T)(object)BitConverter.ToInt32(buffer, idx);
                    idx += 4;
                }
                else if (resultType == typeof(ushort))
                {
                    result = (T)(object)BitConverter.ToInt16(buffer, idx);
                    idx += 2;
                }
                else if (resultType == typeof(uint))
                {
                    result = (T)(object)BitConverter.ToUInt32(buffer, idx);
                    idx += 4;
                }
                else if (resultType == typeof(byte))
                {
                    result = (T)(object)buffer[idx];
                    idx += 1;
                }
                else if (resultType == typeof(bool))
                {
                    result = (T)(object)(buffer[idx] == 1);
                    idx += 1;
                }
                else if (resultType == typeof(float))
                {
                    result = (T)(object)BitConverter.ToDouble(buffer, idx);
                    idx += 8;
                }
                else if (resultType == typeof(string))
                {
                    result = (T)(object)EncodingBig5.GetString(buffer, idx, len);
                    idx += len;
                }
                else if (resultType == typeof(byte[]))
                {
                    var tmp = new byte[len];
                    Array.Copy(buffer, idx, tmp, 0, len);
                    idx += len;
                }
                else if (resultType == typeof(double))
                {
                    result = (T)(object)BitConverter.ToDouble(buffer, idx);
                    idx += 8;
                }
                else
                {
                    throw new Exception("[ByteUtil.ReadPacket]UNKNOWN TYPE ![TRACE]" + TraceUtil.GetTraceMethodName(5));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[ByteUtil.ReadPacket]" + ex.Message + " [TRACE] " + ex.StackTrace);
            }
            return result;
        }

        public static byte[] ToBuffer(ushort val)
        {
            return BitConverter.GetBytes(val);
        }

        public static byte[] ToBuffer(int val)
        {
            return BitConverter.GetBytes(val);
        }

        public static byte[] ToBuffer(uint val)
        {
            return BitConverter.GetBytes(val);
        }

        public static byte[] ToBuffer(string val)
        {
            return EncodingBig5.GetBytes(val);
        }
    }
}