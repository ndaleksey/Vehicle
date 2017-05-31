namespace Swsu.BattleFieldMonitor.Protocol
{
    internal enum ResponseStatus : byte
    {
        /// <summary>
        /// Запрос выполнен успешно.
        /// </summary>
        OK = 0x00,

        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        UnknownError = 0x80,

        /// <summary>
        /// Нет готовности.
        /// </summary>
        NotReady = 0x81,

        /// <summary>
        /// Неверный тип запроса.
        /// </summary>
        WrongRequestType = 0x82,

        /// <summary>
        /// Некорректные данные в payload.
        /// </summary>
        Malformed = 0x83
    }
}
