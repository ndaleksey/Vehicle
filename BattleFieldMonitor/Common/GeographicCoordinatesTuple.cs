using System;
using Swsu.Common;
using Swsu.Geo;
using Swsu.Maps.Windows;

namespace Swsu.BattleFieldMonitor.Common
{
    public struct GeographicCoordinatesTuple :
        IEquatable<GeographicCoordinatesTuple>,
        IFormattable
    {
        #region Fields
        public readonly static GeographicCoordinatesTuple NaN = new GeographicCoordinatesTuple(double.NaN, double.NaN);
        #endregion

        #region Constructors
        public GeographicCoordinatesTuple(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        #endregion

        #region Properties
        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public bool ContainsAtLeastOneNaN()
        {
            return double.IsNaN(Latitude) || double.IsNaN(Longitude);
        }

        public override bool Equals(object obj)
        {
            return ObjectHelpers.EqualsForValues(this, obj);
        }

        public bool Equals(GeographicCoordinatesTuple other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override int GetHashCode()
        {
            return ObjectHelpers.ComposeHashCode(Latitude.GetHashCode(), Longitude.GetHashCode());
        }

        public override string ToString()
        {
            return ToString(Latitude.ToString(), Longitude.ToString());
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(Latitude.ToString(formatProvider), Longitude.ToString(formatProvider));
        }

        public string ToString(string format)
        {
            return ToString(Latitude.ToString(format), Longitude.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(Latitude.ToString(format, formatProvider), Longitude.ToString(format, formatProvider));
        }

        internal static GeographicCoordinatesTuple FromLocation(GeographicLocation source, AngleUnit unit)
        {
            return new GeographicCoordinatesTuple(unit.FromAngle(source.Latitude), unit.FromAngle(source.Longitude));
        }

        internal GeographicLocation ToLocation(AngleUnit unit)
        {
            return new GeographicLocation(unit.ToAngle(Latitude), unit.ToAngle(Longitude));
        }

        private static string ToString(string latitude, string longitude)
        {
            return $"({latitude}, {longitude})";
        }
        #endregion

        #region Operators
        public static bool operator ==(GeographicCoordinatesTuple left, GeographicCoordinatesTuple right)
        {
            return left.Latitude == right.Latitude && left.Longitude == right.Longitude;
        }

        public static bool operator !=(GeographicCoordinatesTuple left, GeographicCoordinatesTuple right)
        {
            return left.Latitude != right.Latitude || left.Longitude != right.Longitude;
        }

        public static implicit operator GeographicCoordinatesTuple(GeographicCoordinates value)
        {
            return new GeographicCoordinatesTuple(value.Latitude, value.Longitude);
        }

        public static implicit operator GeographicCoordinates(GeographicCoordinatesTuple value)
        {
            return new GeographicCoordinates(value.Latitude, value.Longitude);
        }
        #endregion
    }
}
