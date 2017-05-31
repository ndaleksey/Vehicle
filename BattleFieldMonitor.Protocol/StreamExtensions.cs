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
            if (!stream.TryReadFully(ptr, count))
            {
                throw new EndOfStreamException();
            }
        }

        public static void ReadFully(this Stream stream, byte[] buffer, int offset, int count)
        {
            if (!stream.TryReadFully(buffer, offset, count))
            {
                throw new EndOfStreamException();
            }
        }

        public static unsafe bool TryReadFully(this Stream stream, byte* ptr, int count)
        {
            ArrayHelpers.EnsureLength(ref _buffer, count);

            if (stream.TryReadFully(_buffer, 0, count))
            {
                fixed (byte* bufferPtr = &_buffer[0])
                {
                    Copy(bufferPtr, ptr, count);
                }

                return true;
            }

            return false;
        }

        public static bool TryReadFully(this Stream stream, byte[] buffer, int offset, int count)
        {
            for (int n, total = 0; count > 0; offset += n, count -= n, total += n)
            {
                n = stream.Read(buffer, offset, count);

                if (n == 0)
                {
                    if (total == 0)
                    {
                        return false;
                    }
                    else
                    {
                        throw new EndOfStreamException();
                    }
                }
            }

            return true;
        }

        public static unsafe void Write(this Stream stream, byte* ptr, int count)
        {
            ArrayHelpers.EnsureLength(ref _buffer, count);

            fixed (byte* bufferPtr = &_buffer[0])
            {
                Copy(ptr, bufferPtr, count);
            }

            stream.Write(_buffer, 0, count);
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        private static unsafe void Copy(byte* from, byte* to, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                to[i] = from[i];
            }
        }
        #endregion
    }
}