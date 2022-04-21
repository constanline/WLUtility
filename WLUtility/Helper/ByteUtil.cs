using System;
using System.Text;
using Magician.Common.Util;

namespace WLUtility.Helper
{
    public class ByteUtil
    {
        private static readonly Encoding EncodingBig5 = Encoding.GetEncoding(950);

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
    }
}