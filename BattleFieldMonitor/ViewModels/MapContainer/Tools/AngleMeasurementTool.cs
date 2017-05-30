using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Swsu.Geo;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Infrastructure;
using Swsu.Maps.Windows.Tools;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer.Tools
{
    class AngleMeasurementTool : Tool
    {
        #region Fields

        private Point _startMousePosition;
        private Point _endMousePosition;
        private bool _mouseButtonPressed;
        protected VehicleContainerGenerator _vehiclesContainerGenerator;
        private bool _isMeasured;
        private double _radius = 100.0;
        private GeographicLocation _startPoint;
        private GeographicLocation _endPoint;

        #endregion

        #region Dependency Properties

        public readonly static DependencyProperty VehiclesSourceProperty = DependencyProperty.Register(
            nameof(VehiclesSource),
            typeof(IEnumerable),
            typeof(AngleMeasurementTool),
            new FrameworkPropertyMetadata(
                null,
                CallbackHelpers.Create<AngleMeasurementTool, IEnumerable>(o => o.OnVehiclesSourceChanged)));

        public readonly static DependencyProperty VehicleContainerStyleProperty = DependencyProperty.Register(
            nameof(VehicleContainerStyle),
            typeof(Style),
            typeof(AngleMeasurementTool),
            new FrameworkPropertyMetadata(
                null,
                CallbackHelpers.Create<AngleMeasurementTool, Style>(o => o.OnVehicleContainerStyleChanged)));

        #endregion

        #region Properties

        /// <summary>
        /// Стиль, применяемый к элементу контейнера, созданного для каждой засечки времени
        /// </summary>
        public Style VehicleContainerStyle
        {
            get { return (Style)GetValue(VehicleContainerStyleProperty); }
            set { SetValue(VehicleContainerStyleProperty, value); }
        }

        /// <summary>
        /// Коллекция, используемая для создания коллекции танков
        /// </summary>
        public IEnumerable VehiclesSource
        {
            get { return (IEnumerable) GetValue(VehiclesSourceProperty); }
            set { SetValue(VehiclesSourceProperty, value); }
        }

        private Collection<VehicleContainer> VehicleContainersMutable { get; }

        public ReadOnlyCollection<VehicleContainer> VehicleContainers { get; }

        #endregion

        #region Constructor

        public AngleMeasurementTool()
        {
            _vehiclesContainerGenerator = new VehicleContainerGenerator(this);
            VehicleContainers =
                new ReadOnlyCollection<VehicleContainer>(VehicleContainersMutable = new VehiclesCollection(this));
        }

        #endregion

        #region Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            //TODO: При включении инструмента центрирования всё съезжает
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            var vehicleContainer = VehicleContainers[0];

            // Вытаскиваем широту, долготу и азимут объекта
            var latitude = vehicleContainer.Latitude;
            var longitude = vehicleContainer.Longitude;
            var vehicleAzimuth = vehicleContainer.Azimuth;

            // Вычисляем начальную и конечную точки вектора, направленнного по ходу движения РТК
            var point0 = Viewer.GeographicLocationToPosition(latitude, longitude, AngleUnit.Degree);

            // Рисуем линию по ходу движения РТК
            Pen pen = new Pen(Brushes.White, 1);
            Pen linePen = new Pen(Brushes.Black, 1);
            
            if (_isMeasured)
            {
                // Находим новый радиус полупрозрачного круга
                _radius = Math.Sqrt(
                    Math.Pow(_startMousePosition.X - _endMousePosition.X, 2) +
                    Math.Pow(_startMousePosition.Y - _endMousePosition.Y, 2));

                var point1 = new Point(point0.X, point0.Y - _radius);

                // Рисуем полупрозрачный круг
                var ellipseBrush = new SolidColorBrush(Colors.Black) {Opacity = 0.1};
                drawingContext.DrawEllipse(ellipseBrush, null, _startMousePosition, _radius, _radius);

                var lineGeometry = new LineGeometry(point0, point1)
                {
                    Transform = new RotateTransform(vehicleAzimuth, point0.X, point0.Y)
                };
                drawingContext.DrawGeometry(Brushes.Black, linePen, lineGeometry);

                // Рисуем линию и маркер
                drawingContext.DrawLine(linePen, _startMousePosition, _endMousePosition);
                drawingContext.DrawEllipse(Brushes.Black, pen, _endMousePosition, 10, 10);

                //var streamGeometry = new StreamGeometry();
                //using (var context = streamGeometry.Open())
                //{
                //    context.BeginFigure(_endMousePosition, false, false);
                //    context.ArcTo(point1, new Size(_radius / 2, _radius / 2), 0, true, SweepDirection.Clockwise, true, true);
                //}
                //drawingContext.DrawGeometry(null, linePen, streamGeometry);

                // Находим угол от РТК до географической точки
                var sphere = new Sphere(6378136);
                var solution = sphere.SolveInverseGeodeticProblem(
                    _startPoint.Latitude.Degrees,
                    _startPoint.Longitude.Degrees,
                    _endPoint.Latitude.Degrees,
                    _endPoint.Longitude.Degrees);

                var userAzimuth = solution.Azimuth2;

                // Находим разницу между азимутами 
                var resultAngle = userAzimuth - vehicleAzimuth;

                if (resultAngle < -180)
                {
                    resultAngle += 360;
                }

                // Рисуем текст
                var textAngle = vehicleAzimuth + resultAngle/2.0;

                var textDistance = _radius/2 + 6;

                var textAngleRad = textAngle*Math.PI/180;
                var cosTextAngleRad = Math.Sin(textAngleRad);
                var sinTextAngleRad = Math.Cos(textAngleRad);

                var x = textDistance*cosTextAngleRad;
                var y = -textDistance*sinTextAngleRad - 6;

                var textPosition = new Point(_startMousePosition.X + x, _startMousePosition.Y + y);

                //var textPosition = new Point(
                //    _startMousePosition.X,
                //    _startMousePosition.Y);

                //var transformGroup = new TransformGroup();

                //var translateTransform = new TranslateTransform(0, -_radius/2.0 - 12.0);
                //var rotateTransform = new RotateTransform(textAngle, _startMousePosition.X, _startMousePosition.Y);

                //if (resultAngle < -180)
                //{
                //    rotateTransform = new RotateTransform(textAngle + 180, _startMousePosition.X, _startMousePosition.Y);
                //}

                //transformGroup.Children.Add(translateTransform);
                //transformGroup.Children.Add(rotateTransform);

                var typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold,
                    FontStretches.Normal);
                var angleString = resultAngle.ToString("0.00", CultureInfo.InvariantCulture) + "°";

                var formattedText = new FormattedText(angleString, CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight, typeface, 12, Brushes.Black);
                formattedText.TextAlignment = TextAlignment.Center;

                //drawingContext.PushTransform(transformGroup);
                drawingContext.DrawText(formattedText, textPosition);
            }
            else
            {
                // Рисуем полупрозрачный круг
                var ellipseBrush = new SolidColorBrush(Colors.Black) { Opacity = 0.1 };
                drawingContext.DrawEllipse(ellipseBrush, null, point0, _radius, _radius);

                var point1 = new Point(point0.X, point0.Y - _radius);

                var lineGeometry = new LineGeometry(point0, point1)
                {
                    Transform = new RotateTransform(vehicleAzimuth, point0.X, point0.Y)
                };
                drawingContext.DrawGeometry(Brushes.Black, linePen, lineGeometry);

                // Рисуем линию и маркер
                drawingContext.DrawLine(linePen, _startMousePosition, _endMousePosition);

                drawingContext.DrawEllipse(Brushes.Black, pen, _endMousePosition, 10, 10);
            }

            
            //TODO: Как удалить фигуры при повторном включении инструмента
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            e.Handled = true;

            _endMousePosition = e.GetPosition(this);
            _endPoint = Viewer.PositionToGeographicLocation(_endMousePosition);

            _mouseButtonPressed = true;
            _isMeasured = true;

            var vehicleContainer = VehicleContainers[0];

            // Вытаскиваем широту, долготу и азимут объекта
            var latitude = vehicleContainer.Latitude;
            var longitude = vehicleContainer.Longitude;

            _startMousePosition = Viewer.GeographicLocationToPosition(latitude, longitude, AngleUnit.Degree);
            _startPoint = Viewer.PositionToGeographicLocation(_startMousePosition);
            
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_mouseButtonPressed)
            {
                e.Handled = true;

                _endMousePosition = e.GetPosition(this);
                _endPoint = Viewer.PositionToGeographicLocation(_endMousePosition);

                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_mouseButtonPressed)
            {
                e.Handled = true;

                _mouseButtonPressed = false;

                _endMousePosition = e.GetPosition(this);
                _endPoint = Viewer.PositionToGeographicLocation(_endMousePosition);

                InvalidateVisual();

                //_startMousePosition = new Point(Double.NaN, Double.NaN);
                //_endMousePosition = new Point(Double.NaN, Double.NaN);
            }
        }

        private void OnVehiclesSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            _vehiclesContainerGenerator.NotifyItemsSourceChanged(oldValue, newValue);
        }

        private void OnVehicleContainerStyleChanged(Style oldValue, Style newValue)
        {
            _vehiclesContainerGenerator.NotifyItemContainerStyleChanged(oldValue, newValue);
        }

        private void ClearVehicles(IList<VehicleContainer> items, Action baseClearItems)
        {
            foreach (var item in items)
            {
                OnVehicleContainerRemoving(item);
            }

            baseClearItems();
        }

        private void InsertVehicle(int index, VehicleContainer item, IList<VehicleContainer> items, Action<int, VehicleContainer> baseInsertItem)
        {
            baseInsertItem(index, item);
            
            OnVehicleContainerAdded(item);
        }

        private void RemoveVehicle(int index, IList<VehicleContainer> items, Action<int> baseRemoveItem)
        {
            OnVehicleContainerRemoving(items[index]);
            baseRemoveItem(index);
        }

        private void SetVehicle(int index, VehicleContainer item, IList<VehicleContainer> items, Action<int, VehicleContainer> baseSetItem)
        {
            OnVehicleContainerRemoving(items[index]);
            baseSetItem(index, item);
            OnVehicleContainerAdded(item);
        }

        private void OnVehicleContainerAdded(VehicleContainer vehicle)
        {
            ItemContainerHelpers.OnContainerAdded(this, vehicle, AddLogicalChild);
        }

        private void OnVehicleContainerRemoving(VehicleContainer vehicle)
        {
           ItemContainerHelpers.OnContainerRemoved(this, vehicle, RemoveLogicalChild);
        }
        #endregion

        #region Nested Types

        protected class VehicleContainerGenerator : ItemContainerGenerator2<VehicleContainer>
        {
            #region Fields

            private readonly AngleMeasurementTool _outer;

            #endregion

            #region Constructor

            public VehicleContainerGenerator(AngleMeasurementTool outer)
            {
                _outer = outer;
            }

            #endregion

            #region Properties

            protected override IList<VehicleContainer> ItemContainers
            {
                get { return _outer.VehicleContainersMutable; }
            }

            protected override Style ItemContainerStyle => _outer.VehicleContainerStyle;

            protected override IEnumerable ItemsSource => _outer.VehiclesSource;

            #endregion

            #region Methods

            protected override VehicleContainer CreateItemContainerCore()
            {
                return new VehicleContainer();
            }

            protected override object FindName(string name) => _outer.FindName(name);

            protected override void RaiseEvent(RoutedEventArgs e) => _outer.RaiseEvent(e);

            protected override void VerifyAccess() => _outer.VerifyAccess();

            #endregion
        }

        private class VehiclesCollection : Collection<VehicleContainer>
        {
            #region Fields

            private readonly AngleMeasurementTool _outer;

            #endregion

            #region Constructors

            public VehiclesCollection(AngleMeasurementTool outer)
            {
                _outer = outer;
            }

            #endregion

            #region Methods

            protected override void ClearItems()
            {
                _outer.ClearVehicles(Items, base.ClearItems);
            }

            protected override void InsertItem(int index, VehicleContainer item)
            {
                _outer.InsertVehicle(index, item, Items, base.InsertItem);
            }

            protected override void RemoveItem(int index)
            {
                _outer.RemoveVehicle(index, Items, base.RemoveItem);
            }

            protected override void SetItem(int index, VehicleContainer item)
            {
                _outer.SetVehicle(index, item, Items, base.SetItem);
            }

            #endregion
        }

        #endregion
    }
}
