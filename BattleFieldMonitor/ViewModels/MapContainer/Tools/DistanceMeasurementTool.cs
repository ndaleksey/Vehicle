using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Swsu.Geo;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Tools;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer.Tools
{
    class DistanceMeasurementTool : Tool
    {
        #region Fields

        private Point _startMousePosition;
        private Point _endMousePosition;
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

                drawingContext.DrawEllipse(Brushes.Red, pen, _startMousePosition, 10, 10);
                drawingContext.DrawEllipse(Brushes.Red, pen, _endMousePosition, 10, 10);

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

        #endregion

    }
}
