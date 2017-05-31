namespace Swsu.BattleFieldMonitor.Protocol
{
    internal enum RequestType : byte
    {
        GetUgvTelemetry = 0x01,
        GetControlCenterTelemetry = 0x02,
        GetPath = 0x03,
        GetTrajectory = 0x04
    }
}