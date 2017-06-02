namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class BigEndian
    {
        #region Methods
        public static byte[] GetBytes(double value)
        {
            var bytes = new byte[8];
            BinaryDataHelpers.Put(bytes, 0, value);
            return bytes;
        }

        public static byte[] GetBytes(float value)
        {
            var bytes = new byte[4];
            BinaryDataHelpers.Put(bytes, 0, value);
            return bytes;
        }

        public static byte[] GetBytes(uint value)
        {
            var bytes = new byte[4];
            BinaryDataHelpers.Put(bytes, 0, value);
            return bytes;
        }

        public static byte[] GetBytes(ulong value)
        {
            var bytes = new byte[8];
            BinaryDataHelpers.Put(bytes, 0, value);
            return bytes;
        }

        public static byte[] GetBytes(ushort value)
        {
            var bytes = new byte[2];
            BinaryDataHelpers.Put(bytes, 0, value);
            return bytes;
        }
        #endregion
    }
}
