namespace Swsu.BattleFieldMonitor.Protocol
{
    public interface IProtocolHandler
    {
        #region Methods
        VehicleTelemetry GetControlCenterTelemetry();

        Coordinates3D GetReturnPoint();

        VehicleTelemetry GetUgvTelemetry();
        #endregion
    }
}
