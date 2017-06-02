namespace Swsu.BattleFieldMonitor.Protocol
{
    public static class DefaultPorts
    {
        #region Constants
        /// <summary>
        /// Клиент обращается к серверу с запросами, а сервер возвращает клиенту отклики.
        /// </summary>
        public const int ClientDriven = 4480;

        /// <summary>
        /// Сервер обращается к клиенту с запросами, а клиент возвращает серверу отклики.
        /// </summary>
        public const int ServerDriven = 4481;
        #endregion
    }
}
