using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Swsu.Coverages;
using Swsu.Geo;
using Swsu.Geo.Epsg;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Tools;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer.Tools
{
    class HeightMeasurementTool : Tool
    {
        #region Fields

        private Point _startMousePosition;
        private Point _endMousePosition;
        private double _heightDifference;
        private bool _mouseButtonPressed;
        private bool _isMeasured;
        private GeographicLocation _startPoint;
        private GeographicLocation _endPoint;

        #endregion

        #region Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (_isMeasured)
            {
                Pen pen = new Pen(Brushes.White, 1);
                Pen linePen = new Pen(Brushes.Black, 1);

                // Рисуем линию и два маркера
                drawingContext.DrawLine(linePen, _startMousePosition, _endMousePosition);

                drawingContext.DrawEllipse(Brushes.Black, pen, _startMousePosition, 10, 10);

                var typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                var text = "h₁"; //h₂
                var heightMarkerFormattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 12, Brushes.White) { TextAlignment = TextAlignment.Center };
                drawingContext.DrawText(heightMarkerFormattedText, new Point(_startMousePosition.X, _startMousePosition.Y - 7));

                drawingContext.DrawEllipse(Brushes.Black, pen, _endMousePosition, 10, 10);
                text = "h₂";
                heightMarkerFormattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 12, Brushes.White) { TextAlignment = TextAlignment.Center };
                drawingContext.DrawText(heightMarkerFormattedText, new Point(_endMousePosition.X, _endMousePosition.Y - 7));

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

                var distanceString = "h₂ - h₁ = " + _heightDifference.ToString("0.00", CultureInfo.InvariantCulture) + " м.";

                if (double.IsNaN(_heightDifference))
                {
                    distanceString = "";
                }

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

            _heightDifference = double.NaN;

            _endMousePosition = new Point(Double.NaN, Double.NaN);

            _mouseButtonPressed = true;
            _isMeasured = true;

            _startMousePosition = e.GetPosition(this);
            _startPoint = Viewer.PositionToGeographicLocation(_startMousePosition);

            InvalidateVisual();
        }

        protected override async void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_mouseButtonPressed)
            {
                e.Handled = true;

                _endMousePosition = e.GetPosition(this);
                _endPoint = Viewer.PositionToGeographicLocation(_endMousePosition);

                // Находим перепад высот между точками
                //_heightDifference = await Task.Run(() => FindHeightDifference(_startPoint, _endPoint));
                InvalidateVisual();
            }
        }

        protected override async void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_mouseButtonPressed)
            {
                e.Handled = true;

                _mouseButtonPressed = false;

                _endMousePosition = e.GetPosition(this);
                _endPoint = Viewer.PositionToGeographicLocation(_endMousePosition);

                // Находим перепад высот между точками
                _heightDifference = await Task.Run(() => FindHeightDifference(_startPoint, _endPoint));

                InvalidateVisual();

                //_startMousePosition = new Point(Double.NaN, Double.NaN);
                //_endMousePosition = new Point(Double.NaN, Double.NaN);
            }
        }

        private double FindHeightDifference(GeographicLocation startPoint, GeographicLocation endPoint)
        {
            var height1 = FindHeight(startPoint);
            var height2 = FindHeight(endPoint);

            var difference = height2 - height1;

            //InvalidateVisual();
            return difference;
        }

        private double FindHeight(GeographicLocation startPoint)
        {
            var latitude = Convert.ToDouble(startPoint.Latitude.Degrees);
            var longitude = Convert.ToDouble(startPoint.Longitude.Degrees);
            double height;

            string id = "world";  //строка задает идентификатор покрытия
            string crsUri = null;
            var uri = new Uri("http://10.6.7.179:13013/?SERVICE=WCS&VERSION=1.1.1&REQUEST=GetCapabilities");

            ICoverage wcsCover = new WcsCoverage(uri, id, new MicrosoftAccessEpsgRegistry(@"C:\EPSG\EPSG_v8_9.mdb"));
            Interpolation interpolation = Interpolation.Linear;

            try
            {
                //height = await Task.Run(() => wcsCover.Get(latitude, longitude, crsUri, interpolation));
                height = wcsCover.Get(latitude, longitude, crsUri, interpolation);

            }
            catch (Exception ex)
            {
                //Debug.Error(ex, "Не удается получить высоту");
                return 0;
            }

            return height;
        }

        #endregion
    }
}
