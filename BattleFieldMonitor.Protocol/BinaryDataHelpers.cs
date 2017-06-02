namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class BinaryDataHelpers
    {
        #region Methods
        public static unsafe double GetDouble(byte[] bytes, int startIndex)
        {
            return UInt64BitsToDouble(GetUInt64(bytes, startIndex));
        }

        public static unsafe float GetSingle(byte[] bytes, int startIndex)
        {
            return UInt32BitsToSingle(GetUInt32(bytes, startIndex));
        }

        public static unsafe ushort GetUInt16(byte[] bytes, int startIndex)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                return unchecked((ushort)(p[0] << 8 | p[1]));
            }
        }

        public static unsafe uint GetUInt32(byte[] bytes, int startIndex)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                return unchecked((uint)(p[0] << 24 | p[1] << 16 | p[2] << 8 | p[3]));
            }
        }

        public static unsafe ulong GetUInt64(byte[] bytes, int startIndex)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                return unchecked(((ulong)p[0] << 56 | (ulong)p[1] << 48 | (ulong)p[2] << 40 | (ulong)p[3] << 32 | (ulong)p[4] << 24 | (ulong)p[5] << 16 | (ulong)p[6] << 8 | p[7]));
            }
        }

        public static unsafe void Put(byte[] bytes, int startIndex, double value)
        {
            Put(bytes, startIndex, DoubleToUInt64Bits(value));
        }

        public static unsafe void Put(byte[] bytes, int startIndex, float value)
        {
            Put(bytes, startIndex, SingleToUInt32Bits(value));
        }

        public static unsafe void Put(byte[] bytes, int startIndex, ushort value)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                unchecked
                {
                    p[0] = (byte)(value >> 8);
                    p[1] = (byte)value;
                }
            }
        }

        public static unsafe void Put(byte[] bytes, int startIndex, uint value)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                unchecked
                {
                    p[0] = (byte)(value >> 24);
                    p[1] = (byte)(value >> 16);
                    p[2] = (byte)(value >> 8);
                    p[3] = (byte)value;
                }
            }
        }

        public static unsafe void Put(byte[] bytes, int startIndex, ulong value)
        {
            fixed (byte* p = &bytes[startIndex])
            {
                unchecked
                {
                    p[0] = (byte)(value >> 56);
                    p[1] = (byte)(value >> 48);
                    p[2] = (byte)(value >> 40);
                    p[3] = (byte)(value >> 32);
                    p[4] = (byte)(value >> 24);
                    p[5] = (byte)(value >> 16);
                    p[6] = (byte)(value >> 8);
                    p[7] = (byte)value;
                }
            }
        }

        public static unsafe void SwapBytes(ref double value)
        {
            var v = value;
            SwapBytes64((byte*)&v);
            value = v;
        }

        public static unsafe void SwapBytes(ref float value)
        {
            var v = value;
            SwapBytes32((byte*)&v);
            value = v;
        }

        public static unsafe void SwapBytes(ref ushort value)
        {
            var v = value;
            SwapBytes16((byte*)&v);
            value = v;
        }

        public static unsafe void SwapBytes(ref uint value)
        {
            var v = value;
            SwapBytes32((byte*)&v);
            value = v;
        }

        public static unsafe void SwapBytes(ref ulong value)
        {
            var v = value;
            SwapBytes64((byte*)&v);
            value = v;
        }

        private static unsafe ulong DoubleToUInt64Bits(double value)
        {
            return *(ulong*)&value;
        }

        private static unsafe uint SingleToUInt32Bits(float value)
        {
            return *(uint*)&value;
        }

        private static unsafe void SwapBytes16(byte* b)
        {
            Swap(ref b[0], ref b[1]);
        }

        private static unsafe void SwapBytes32(byte* b)
        {
            Swap(ref b[0], ref b[3]);
            Swap(ref b[1], ref b[2]);
        }

        private static unsafe void SwapBytes64(byte* b)
        {
            Swap(ref b[0], ref b[7]);
            Swap(ref b[1], ref b[6]);
            Swap(ref b[2], ref b[5]);
            Swap(ref b[3], ref b[4]);
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            var t = a;
            a = b;
            b = t;
        }

        private static unsafe float UInt32BitsToSingle(uint value)
        {
            return *(float*)&value;
        }

        private static unsafe double UInt64BitsToDouble(ulong value)
        {
            return *(double*)&value;
        }
        #endregion
    }
}