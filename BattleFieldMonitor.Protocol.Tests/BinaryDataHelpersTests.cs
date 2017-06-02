using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [TestClass]
    public class BinaryDataHelpersTests
    {
        #region Methods
        [TestMethod]
        public void Should_ConvertFromDouble()
        {
            var bytes = new byte[8];
            BinaryDataHelpers.Put(bytes, 0, 12.34);
            CollectionAssert.AreEqual(new byte[] { 0x40, 0x28, 0xae, 0x14, 0x7a, 0xe1, 0x47, 0xae }, bytes);
        }

        [TestMethod]
        public void Should_ConvertFromSingle()
        {
            var bytes = new byte[4];
            BinaryDataHelpers.Put(bytes, 0, 12.34f);
            CollectionAssert.AreEqual(new byte[] { 0x41, 0x45, 0x70, 0xa4 }, bytes);
        }

        [TestMethod]
        public void Should_ConvertFromUInt16()
        {
            var bytes = new byte[2];
            BinaryDataHelpers.Put(bytes, 0, 0xa0b1);
            CollectionAssert.AreEqual(new byte[] { 0xa0, 0xb1 }, bytes);
        }

        [TestMethod]
        public void Should_ConvertFromUInt32()
        {
            var bytes = new byte[4];
            BinaryDataHelpers.Put(bytes, 0, 0xa0b1c2d3);
            CollectionAssert.AreEqual(new byte[] { 0xa0, 0xb1, 0xc2, 0xd3 }, bytes);
        }

        [TestMethod]
        public void Should_ConvertFromUInt64()
        {
            var bytes = new byte[8];
            BinaryDataHelpers.Put(bytes, 0, 0xa0b1c2d3fedcba87);
            CollectionAssert.AreEqual(new byte[] { 0xa0, 0xb1, 0xc2, 0xd3, 0xfe, 0xdc, 0xba, 0x87 }, bytes);
        }

        [TestMethod]
        public void Should_ConvertToDouble()
        {
            Assert.AreEqual(12.34, BinaryDataHelpers.GetDouble(new byte[] { 0x40, 0x28, 0xae, 0x14, 0x7a, 0xe1, 0x47, 0xae }, 0));
        }

        [TestMethod]
        public void Should_ConvertToSingle()
        {
            Assert.AreEqual(12.34f, BinaryDataHelpers.GetSingle(new byte[] { 0x41, 0x45, 0x70, 0xa4 }, 0));
        }

        [TestMethod]
        public void Should_ConvertToUInt16()
        {
            Assert.AreEqual(0xa0b1, BinaryDataHelpers.GetUInt16(new byte[] { 0xa0, 0xb1 }, 0));
        }

        [TestMethod]
        public void Should_ConvertToUInt32()
        {
            Assert.AreEqual(0xa0b1c2d3, BinaryDataHelpers.GetUInt32(new byte[] { 0xa0, 0xb1, 0xc2, 0xd3 }, 0));
        }

        [TestMethod]
        public void Should_ConvertToUInt64()
        {
            Assert.AreEqual(0xa0b1c2d3fedcba87, BinaryDataHelpers.GetUInt64(new byte[] { 0xa0, 0xb1, 0xc2, 0xd3, 0xfe, 0xdc, 0xba, 0x87 }, 0));
        }

        [TestMethod]
        public void Should_SwapBytesInUInt16()
        {
            ushort value = 0xabcd;
            BinaryDataHelpers.SwapBytes(ref value);
            Assert.AreEqual((ushort)0xcdab, value);
        }

        [TestMethod]
        public void Should_SwapBytesInUInt32()
        {
            uint value = 0xabcd0123;
            BinaryDataHelpers.SwapBytes(ref value);
            Assert.AreEqual((uint)0x2301cdab, value);
        }

        [TestMethod]
        public void Should_SwapBytesInUInt64()
        {
            ulong value = 0xabcd0123eeff7788;
            BinaryDataHelpers.SwapBytes(ref value);
            Assert.AreEqual(0x8877ffee2301cdab, value);
        }
        #endregion
    }
}