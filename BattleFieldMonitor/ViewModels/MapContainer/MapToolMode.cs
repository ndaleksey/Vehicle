namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
	public enum MapToolMode
	{
        /// <summary>
        /// Панорамирование
        /// </summary>
        Pan,

        /// <summary>
        /// Рисование препятствий одной точкой
        /// </summary>
        PointDrawing,

        /// <summary>
        /// Рисование препятствий точками
        /// </summary>
        PreciseLineStringDrawing,

        /// <summary>
        /// Рисование препятствий инструментом Лассо
        /// </summary>
        QuickLineStringDrawing,

        /// <summary>
        /// Простое выделение
        /// </summary>
	    SimpleSelection,

        /// <summary>
        /// Редактирование препятствий
        /// </summary>
        PolygonReshaping,

        /// <summary>
        /// Редактирование линий
        /// </summary>
        LineReshaping,

        /// <summary>
        /// Рисование маяков
        /// </summary>
	    BeaconDrawing,

        /// <summary>
        /// Рисование трасс
        /// </summary>
	    RouteDrawing,

        /// <summary>
        /// Измерение расстояний
        /// </summary>
	    DistanceMeasurement,

        /// <summary>
        /// Измерение углов
        /// </summary>
        AngleMeasurement,

        /// <summary>
        /// Измерение перепадов высот
        /// </summary>
	    HeightMeasurement
	}
}