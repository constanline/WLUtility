using System.Collections.Generic;
using WLUtility.Helper;

namespace WLUtility.Core
{
    public class PacketBuilder
    {
        private readonly List<byte> _buffer = new List<byte>();

        public PacketBuilder(byte aType)
        {
            _buffer.Add(aType);
        }

        public PacketBuilder(byte aType, byte bType)
        {
            _buffer.Add(aType);
            _buffer.Add(bType);
        }

        public int Length => _buffer.Count;


        public byte[] Build()
        {
            return _buffer.ToArray();
        }

        public PacketBuilder Add(byte[] buffer)
        {
            _buffer.AddRange(buffer);
            return this;
        }

        public PacketBuilder Add(byte b)
        {
            _buffer.Add(b);
            return this;
        }

        public PacketBuilder Add(ushort v)
        {
            _buffer.AddRange(ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Add(uint v)
        {
            _buffer.AddRange(ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Add(int v)
        {
            _buffer.AddRange(ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Add(string v, bool withLen = false)
        {
            var buffer = ByteUtil.ToBuffer(v);
            if (withLen) _buffer.Add((byte)buffer.Length);
            _buffer.AddRange(buffer);
            return this;
        }

        public PacketBuilder Insert(int index, byte[] buffer)
        {
            _buffer.InsertRange(index, buffer);
            return this;
        }

        public PacketBuilder Insert(int index, byte b)
        {
            _buffer.Insert(index, b);
            return this;
        }

        public PacketBuilder Insert(int index, ushort v)
        {
            _buffer.InsertRange(index, ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Insert(int index, uint v)
        {
            _buffer.InsertRange(index, ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Insert(int index, int v)
        {
            _buffer.InsertRange(index, ByteUtil.ToBuffer(v));
            return this;
        }

        public PacketBuilder Insert(int index, string v, bool withLen = false)
        {
            var buffer = ByteUtil.ToBuffer(v);
            _buffer.InsertRange(index, buffer);
            if (withLen) _buffer.Insert(index, (byte)buffer.Length);
            return this;
        }
    }
}