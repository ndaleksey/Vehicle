namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class SizeOf
    {
        #region Fields
        public readonly static int Coordinates3D;

        public readonly static int Keypoint;

        public readonly static int VehicleTelemetry;
        #endregion

        #region Constructors
        static unsafe SizeOf()
        {
            Coordinates3D = sizeof(Coordinates3D);
            Keypoint = sizeof(Keypoint);
            VehicleTelemetry = sizeof(VehicleTelemetry);
        }
        #endregion
    }
}