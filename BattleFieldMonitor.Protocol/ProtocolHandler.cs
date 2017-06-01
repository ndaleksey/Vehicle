namespace Swsu.BattleFieldMonitor.Protocol
{
    public abstract class ProtocolHandler : IProtocolHandler
    {
        #region Constructors
        protected ProtocolHandler()
        {
        }
        #endregion

        #region Methods
        protected virtual VehicleTelemetry GetControlCenterTelemetry()
        {
            throw new WrongRequestTypeException();
        }

        protected virtual Coordinates3D GetReturnPoint()
        {
            throw new WrongRequestTypeException();
        }

        protected virtual Trajectory GetTrajectory()
        {
            throw new WrongRequestTypeException();
        }

        protected virtual VehicleTelemetry GetUgvTelemetry()
        {
            throw new WrongRequestTypeException();
        }

        VehicleTelemetry IProtocolHandler.GetControlCenterTelemetry() => GetControlCenterTelemetry();

        Coordinates3D IProtocolHandler.GetReturnPoint() => GetReturnPoint();

        Trajectory IProtocolHandler.GetTrajectory() => GetTrajectory();

        VehicleTelemetry IProtocolHandler.GetUgvTelemetry() => GetUgvTelemetry();
        #endregion
    }
}
