using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class ReferenceData
    {
        #region Properties
        public static byte[] GetReturnPoint_Request
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)RequestType.GetReturnPoint);
                    stream.Write(BigEndian.GetBytes((uint)0));
                    return stream.ToArray();
                }
            }
        }

        public static byte[] GetReturnPoint_Response
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)ResponseStatus.OK);
                    stream.Write(BigEndian.GetBytes((uint)SizeOf.Coordinates3D));
                    stream.Write(BigEndian.GetBytes(30.0));
                    stream.Write(BigEndian.GetBytes(50.0));
                    stream.Write(BigEndian.GetBytes(10.0));
                    return stream.ToArray();
                }
            }
        }

        public static byte[] GetTrajectory_Request
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)RequestType.GetTrajectory);
                    stream.Write(BigEndian.GetBytes((uint)0));
                    return stream.ToArray();
                }
            }
        }

        public static byte[] GetTrajectory_Response
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)ResponseStatus.OK);
                    stream.Write(BigEndian.GetBytes((uint)(sizeof(ulong) + sizeof(ushort) + 3 * SizeOf.Coordinates3D)));
                    stream.Write(BigEndian.GetBytes((ulong)100500));
                    stream.Write(BigEndian.GetBytes(3));

                    stream.Write(BigEndian.GetBytes(30.0));
                    stream.Write(BigEndian.GetBytes(50.0));
                    stream.Write(BigEndian.GetBytes(10.0));

                    stream.Write(BigEndian.GetBytes(32.0));
                    stream.Write(BigEndian.GetBytes(52.0));
                    stream.Write(BigEndian.GetBytes(11.0));

                    stream.Write(BigEndian.GetBytes(35.0));
                    stream.Write(BigEndian.GetBytes(55.0));
                    stream.Write(BigEndian.GetBytes(12.0));
                    return stream.ToArray();
                }
            }
        }

        public static byte[] GetUgvTelemetry_Request
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)RequestType.GetUgvTelemetry);
                    stream.Write(BigEndian.GetBytes((uint)0));
                    return stream.ToArray();
                }
            }
        }

        public static byte[] GetUgvTelemetry_Response
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte((byte)ProtocolVersion.V1);
                    stream.WriteByte((byte)ResponseStatus.OK);
                    stream.Write(BigEndian.GetBytes((uint)SizeOf.VehicleTelemetry));
                    stream.Write(BigEndian.GetBytes(30.0));
                    stream.Write(BigEndian.GetBytes(50.0));
                    stream.Write(BigEndian.GetBytes(10.0));
                    stream.Write(BigEndian.GetBytes(5.0f));
                    stream.Write(BigEndian.GetBytes(7.0f));
                    stream.Write(BigEndian.GetBytes(3.0f));
                    stream.Write(BigEndian.GetBytes(9.0f));
                    return stream.ToArray();
                }
            }
        }
        #endregion
    }
}