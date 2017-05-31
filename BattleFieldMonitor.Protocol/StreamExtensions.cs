using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class StreamExtensions
    {
        #region Fields
        [ThreadStatic]
        private static byte[] _buffer;
        #endregion

        #region Methods
        public static unsafe void ReadFully(this Stream stream, byte* ptr, int count)
        {
            ArrayHelpers.EnsureLength(ref _buffer, count);
            stream.ReadFully(_buffer, 0, count);

            fixed (byte* bufferPtr = &_buffer[0])
            {
                for (var i = 0; i < count; ++i)
                {
                    ptr[i] = bufferPtr[i];
                }
            }
        }

        public static void ReadFully(this Stream stream, byte[] buffer, int offset, int count)
        {
            for (int n; count > 0; offset += n, count -= n)
            {
                n = stream.Read(buffer, offset, count);

                if (n == 0)
                {
                    throw new EndOfStreamException();
                }
            }
        }

        public static unsafe void Write(this Stream stream, byte* ptr, int count)
        {
            ArrayHelpers.EnsureLength(ref _buffer, count);

            fixed (byte* bufferPtr = &_buffer[0])
            {
                for (var i = 0; i < count; ++i)
                {
                    bufferPtr[i] = ptr[i];
                }
            }

            stream.Write(_buffer, 0, count);
        }
        #endregion
    }
}
