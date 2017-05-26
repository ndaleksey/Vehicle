using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// �����, �������������� ����� �����������, ������������ �� ����� � ���� ��������
    /// </summary>
    public class Obstacle : BindableBase
    {
        #region Properties

        private bool _isEditMode;

        /// <summary>
        /// ��������� ��������� ��������
        /// </summary>
        public ObservableCollection<Coord> Coords { get; }

        /// <summary>
        /// �������, ����������� �� ��, ��� ������ ��������� � ������ ��������������
        /// </summary>
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value, nameof(IsEditMode)); }
        }

        #endregion

        #region Constructors

        public Obstacle()
        {
            Coords = new ObservableCollection<Coord>();
        }

        #endregion
    }
}