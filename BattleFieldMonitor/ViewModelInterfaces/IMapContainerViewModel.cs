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
        /// Включить/отключить мини-карту
        /// </summary>
        /// <param name="value">Истина, если включить, иначе ложь</param>
        void SetMiniMap(bool value);
        
        /// <summary>
        /// Включить/отключить режим центрирования (слежения за танком)
        /// </summary>
        /// <param name="value">Истина, если включить, иначе ложь</param>
        void SetCenteringMode(bool value);

        /// <summary>
        /// Переключиться на инструмент измерения углов
        /// </summary>
        void SwitchToAngleMeasurementTool();

        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        void SwitchToBeaconDrawingTool();

        /// <summary>
        /// Переключиться на инструмент измерения расстояний
        /// </summary>
        void SwitchToDistanceMeasurementTool();

        /// <summary>
        /// Переключиться на инструмент измерения перепадов высот
        /// </summary>
        void SwitchToHeightMeasurementTool();

        /// <summary>
        /// Переключиться на инструмент добавления точек
        /// </summary>
        void SwitchToPointDrawingTool();

        /// <summary>
        /// Переключиться на инструмент рисования объектов точками
        /// </summary>
        void SwitchToPreciseLineStringDrawingTool();

        /// <summary>
        /// Переключиться на инструмент добавления трасс
        /// </summary>
        void SwitchToRouteDrawingTool();

        /// <summary>
        /// Переключиться на инструмент Лассо
        /// </summary>
        void SwitchToQuickLineStringDrawingTool();

        /// <summary>
        /// Переключиться на инструмент простого выделения
        /// </summary>
        void SwitchToSimpleSelectionTool();

        /// <summary>
        /// Переключиться на инструмент измерения перепадов высот
        /// </summary>
        void SwitchToHeightMeasurementTool();

        #endregion
    }
}
