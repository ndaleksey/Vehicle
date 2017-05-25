namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    /// <summary>
    /// Используется моделями представлений, содержащих MapContainer.
    /// Реализуется моделью представления MapContainer.
    /// </summary>
    internal interface IMapContainerViewModel
    {
        #region Properties

        double ScaleDenominator
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        void SwitchToBeaconDrawingTool();

        /// <summary>
        /// Переключиться на инструмент добавления точек
        /// </summary>
        void SwitchToPointDrawingTool();

        /// <summary>
        /// Переключиться на инструмент рисования объектов точками
        /// </summary>
        void SwitchToPreciseLineStringDrawingTool();

        /// <summary>
        /// Переключиться на инструмент Лассо
        /// </summary>
        void SwitchToQuickLineStringDrawingTool();

        /// <summary>
        /// Переключиться на инструмент простого выделения
        /// </summary>
        void SwitchToSimpleSelectionTool();

        #endregion
    }
}
