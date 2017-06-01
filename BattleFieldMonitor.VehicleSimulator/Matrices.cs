using Swsu.Common;
using Swsu.Mathematics.LinearAlgebra;
using System;

namespace Swsu.BattleFieldMonitor.VehicleSimulator
{
    internal static class Matrices
    {
        #region Methods
        public static Matrix3 EastNorthUpToEarthCentered(double latitude, double longitude)
        {
            var phi = MathHelpers.DegreesToRadians(latitude);
            var lambda = MathHelpers.DegreesToRadians(longitude);
            var sinPhi = Math.Sin(phi);
            var cosPhi = Math.Cos(phi);
            var sinLambda = Math.Sin(lambda);
            var cosLambda = Math.Cos(lambda);

            return new Matrix3(
                -sinLambda, -sinPhi * cosLambda, cosPhi * cosLambda,
                cosLambda, -sinPhi * sinLambda, cosPhi * sinLambda,
                0, cosPhi, sinPhi);
        }
        #endregion
    }
}