namespace Swsu.BattleFieldMonitor.Protocol
{
    public interface IProtocolHandler
    {
        #region Methods
        VehicleTelemetry GetControlCenterTelemetry();

        VehicleTelemetry GetUgvTelemetry();
        #endregion
    }
}
