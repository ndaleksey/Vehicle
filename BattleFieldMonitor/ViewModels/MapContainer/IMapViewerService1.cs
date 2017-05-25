using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Swsu.BattleFieldMonitor.Common;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal interface IMapViewerService1
    {
        #region Properties
        double ScaleDenominator { get; }

        RectangularEnvelope ViewBoundary { get; }
        #endregion

        #region Methods
        Task ExportAsync(Func<Stream> streamFactory, CancellationToken cancellationToken);

        Task ExportAsync(Func<CancellationToken, Task<Stream>> streamFactoryAsync, CancellationToken cancellationToken);

        Task ExportAsync<T>(Func<T, Stream> streamFactory, T arg, CancellationToken cancellationToken);

        Task ExportAsync<T>(Func<T, CancellationToken, Task<Stream>> streamFactoryAsync, T arg, CancellationToken cancellationToken);

        void Locate(GeographicCoordinatesTuple location);

        Task LocateAsync(GeographicCoordinatesTuple location, CancellationToken cancellationToken);

        void LocateAndScaleToFit(IEnumerable<GeographicCoordinatesTuple> locations);

        Task LocateAndScaleToFitAsync(IEnumerable<GeographicCoordinatesTuple> locations, CancellationToken cancellationToken);
        #endregion
    }
}
