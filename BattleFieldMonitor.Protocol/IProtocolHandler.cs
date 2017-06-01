namespace Swsu.BattleFieldMonitor.Protocol
{
    public interface IProtocolHandler
    {
        #region Methods
        VehicleTelemetry GetControlCenterTelemetry();

        Coordinates3D GetReturnPoint();

        Trajectory GetTrajectory();

        VehicleTelemetry GetUgvTelemetry();
        #endregion
    }
}
