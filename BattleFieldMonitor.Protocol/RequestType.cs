namespace Swsu.BattleFieldMonitor.Protocol
{
    public enum RequestType : byte
    {
        /// <summary>
        /// Запрос watchdog от ГИС к АРМ ДУ и от АРМ ДУ к ГИС.
        /// </summary>
        Watchdog = 0x00,

        /// <summary>
        /// Запрос телеметрии РТК от ГИС к АРМ ДУ.
        /// </summary>
        GetUgvTelemetry = 0x01,

        /// <summary>
        /// Запрос телеметрии ПУ от ГИС к АРМ ДУ.
        /// </summary>
        GetControlCenterTelemetry = 0x02,

        /// <summary>
        /// Запрос реализуемой траектории РТК от ГИС к АРМ ДУ.
        /// </summary>
        GetTrajectory = 0x03,

        /// <summary>
        /// Запрос на установку телеметрии РТК от АРМ ДУ к ГИС.
        /// </summary>
        SetUgvTelemetry = 0x04,

        /// <summary>
        /// Запрос на установку телеметрии ПУ от АРМ ДУ к ГИС.
        /// </summary>
        SetControlCenterTelemetry = 0x05,

        /// <summary>
        /// Запрос на установку реализуемой траектории РТК от АРМ ДУ к ГИС.
        /// </summary>
        SetTrajectory = 0x06,

        /// <summary>
        /// Запрос точки возврата РТК.
        /// </summary>
        GetReturnPoint = 0x07,

        /// <summary>
        /// Запрос маршрута движения РТК.
        /// </summary>
        GetPath = 0x08
    }
}