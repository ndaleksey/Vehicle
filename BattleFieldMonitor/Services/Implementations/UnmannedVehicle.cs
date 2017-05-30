using Swsu.Geo;
using System;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class UnmannedVehicle : IdentifiableObject, IUnmannedVehicle
    {
        #region Constructors
        public UnmannedVehicle(Guid id, string displayName, GeographicCoordinates location, double heading, double speed) : base(id)
        {
            DisplayName = displayName;
            Location = location;
            Heading = heading;
            Speed = speed;
        }
        #endregion

        #region Properties
        public string DisplayName
        {
            get;
            private set;
        }

        public double Heading
        {
            get;
            private set;
        }

        public GeographicCoordinates Location
        {
            get;
            private set;
        }

        public double Speed
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        internal void Set(string displayName, GeographicCoordinates location, double heading, double speed)
        {
            DisplayName = displayName;
            Location = location;
            Heading = heading;
            Speed = speed;
            OnChanged(EventArgs.Empty);
        }
        #endregion

        #region Events
        public event EventHandler Changed;
        #endregion
    }
}