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
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            foreach (var vehicleContainer in VehicleContainers)
            {
                
            }

            if (_isMeasured)
            {
                Pen pen = new Pen(Brushes.White, 2);
                Pen linePen = new Pen(Brushes.Black, 2);

                // Рисуем линию и два маркера
                drawingContext.DrawLine(linePen, _startMousePosition, _endMousePosition);

                drawingContext.DrawEllipse(Brushes.Red, pen, _startMousePosition, 15, 15);
                drawingContext.DrawEllipse(Brushes.Red, pen, _endMousePosition, 15, 15);

                // Находим расстояние между точками
                var sphere = new Sphere(6378136);
                var solution = sphere.SolveInverseGeodeticProblem(
                    _startPoint.Latitude.Degrees,
                    _startPoint.Longitude.Degrees,
                    _endPoint.Latitude.Degrees,
                    _endPoint.Longitude.Degrees);

                var distance = solution.Distance;

                // Рисуем текст
                var textAngle = Math.Atan2(
                    _endMousePosition.Y - _startMousePosition.Y,
                    _endMousePosition.X - _startMousePosition.X) * 180 / Math.PI;

                var textPosition = new Point(
                    (_startMousePosition.X + _endMousePosition.X) / 2,
                    (_startMousePosition.Y + _endMousePosition.Y) / 2);

                var transformGroup = new TransformGroup();

                var translateTransform = new TranslateTransform(0, -20);
                var rotateTransform = new RotateTransform(textAngle, textPosition.X, textPosition.Y);

                if (!(textAngle >= -90 && textAngle < 90))
                {
                    rotateTransform = new RotateTransform(textAngle + 180, textPosition.X, textPosition.Y);
                }

                transformGroup.Children.Add(translateTransform);
                transformGroup.Children.Add(rotateTransform);

                var typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                var distanceString = (distance / 1000).ToString("0.00", CultureInfo.InvariantCulture) + " км.";

                var formattedText = new FormattedText(distanceString, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 12, Brushes.Black);
                formattedText.TextAlignment = TextAlignment.Center;

                drawingContext.PushTransform(transformGroup);
                drawingContext.DrawText(formattedText, textPosition);
                //drawingContext.Pop();
            }
            //TODO: Как удалить фигуры при повторном включении инструмента
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            e.Handled = true;

            _endMousePosition = new Point(Double.NaN, Double.NaN);

            _mouseButtonPressed = true;
            _isMeasured = true;

            _startMousePosition = e.GetPosition(this);
            _startPoint = Viewer.PositionToGeographicLocation(_startMousePosition);
            var selectedItems = Viewer.LayerContainers[0].SelectedItems;
            
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
