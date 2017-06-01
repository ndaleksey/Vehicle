namespace Swsu.BattleFieldMonitor.Protocol
{
    public enum RequestType : byte
    {
        GetUgvTelemetry = 0x01,
        GetControlCenterTelemetry = 0x02,
        GetPath = 0x03,
        GetTrajectory = 0x04,
        GetReturnPoint = 0x05,
        Watchdog = 0x06,
        SetPath = 0x07,
        SetReturnPoint = 0x08
    }
}