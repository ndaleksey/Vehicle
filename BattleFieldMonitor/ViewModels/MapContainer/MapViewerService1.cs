using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm.UI;
using Swsu.BattleFieldMonitor.Common;
using Swsu.Maps.Windows;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal class MapViewerService1 : ServiceBase, IMapViewerService1
    {
        #region Fields
        public readonly static DependencyProperty AngleUnitProperty = DependencyProperty.Register(
            nameof(AngleUnit),
            typeof(AngleUnit),
            typeof(MapViewerService1),
            new PropertyMetadata(AngleUnit.Degree));

        public readonly static DependencyProperty LengthUnitProperty = DependencyProperty.Register(
            nameof(LengthUnit),
            typeof(LengthUnit),
            typeof(MapViewerService1),
            new PropertyMetadata(LengthUnit.Meter));

        public readonly static DependencyProperty ViewerProperty = DependencyProperty.Register(
            nameof(Viewer),
            typeof(MapViewer),
            typeof(MapViewerService1),
            new PropertyMetadata(null));
        #endregion

        #region Constructors
        public MapViewerService1() { }
        #endregion

        #region Properties
        public AngleUnit AngleUnit
        {
            get { return (AngleUnit)GetValue(AngleUnitProperty); }
            set { SetValue(AngleUnitProperty, value); }
        }

        public LengthUnit LengthUnit
        {
            get { return (LengthUnit)GetValue(LengthUnitProperty); }
            set { SetValue(LengthUnitProperty, value); }
        }

        public double ScaleDenominator
        {
            get { return ViewerNotNull.ScaleDenominator; }
            set { ViewerNotNull.ScaleDenominator = value; }
        }

        public RectangularEnvelope ViewBoundary
        {
            get { return ViewerNotNull.ViewBoundary.ToEnvelope(LengthUnit); }
        }

        public MapViewer Viewer
        {
            get { return (MapViewer)GetValue(ViewerProperty); }
            set { SetValue(ViewerProperty, value); }
        }

        private MapViewer ViewerNotNull
        {
            get
            {
                var viewer = Viewer;

                if (null == viewer)
                {
                    throw new InvalidOperationException("'Viewer' property must be set.");
                }

                return viewer;
            }
        }
        #endregion

        #region Methods
        public async Task ExportAsync(Func<Stream> streamFactory, CancellationToken cancellationToken)
        {
            var encoder = await ExportAsync(cancellationToken);

            using (var stream = streamFactory())
            {
                encoder.Save(stream);
            }
        }

        public async Task ExportAsync(Func<CancellationToken, Task<Stream>> streamFactoryAsync, CancellationToken cancellationToken)
        {
            var encoder = await ExportAsync(cancellationToken);

            using (var stream = await streamFactoryAsync(cancellationToken))
            {
                encoder.Save(stream);
            }
        }

        public async Task ExportAsync<T>(Func<T, Stream> streamFactory, T arg, CancellationToken cancellationToken)
        {
            var encoder = await ExportAsync(cancellationToken);

            using (var stream = streamFactory(arg))
            {
                encoder.Save(stream);
            }
        }

        public async Task ExportAsync<T>(Func<T, CancellationToken, Task<Stream>> streamFactoryAsync, T arg, CancellationToken cancellationToken)
        {
            var encoder = await ExportAsync(cancellationToken);

            using (var stream = await streamFactoryAsync(arg, cancellationToken))
            {
                encoder.Save(stream);
            }
        }

        public void Locate(GeographicCoordinatesTuple location)
        {
            ViewerNotNull.Locate(ToLocation(location, AngleUnit));
        }

        public Task LocateAsync(GeographicCoordinatesTuple location, CancellationToken cancellationToken)
        {
            return ViewerNotNull.LocateAsync(ToLocation(location, AngleUnit), cancellationToken);
        }

        public void LocateAndScaleToFit(IEnumerable<GeographicCoordinatesTuple> locations)
        {
            ViewerNotNull.LocateAndScaleToFit(locations.Select(l => ToLocation(l, AngleUnit)));
        }

        public Task LocateAndScaleToFitAsync(IEnumerable<GeographicCoordinatesTuple> locations, CancellationToken cancellationToken)
        {
            return ViewerNotNull.LocateAndScaleToFitAsync(locations.Select(l => ToLocation(l, AngleUnit)), cancellationToken);
        }

        public Task<BitmapEncoder> ExportAsync(CancellationToken cancellationToken)
        {
            var bitmap = new RenderTargetBitmap(600, 400, 96.0, 96.0, PixelFormats.Pbgra32);
            bitmap.Render(ViewerNotNull);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            return Task.FromResult<BitmapEncoder>(encoder);
        }

        private static GeographicLocation ToLocation(GeographicCoordinatesTuple coordinates, AngleUnit unit)
        {
            return new GeographicLocation(coordinates.Latitude, coordinates.Longitude, unit);
        }
        #endregion
    }
}
