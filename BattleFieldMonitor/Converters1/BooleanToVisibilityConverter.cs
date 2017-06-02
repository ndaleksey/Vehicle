using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Swsu.BattleFieldMonitor.Converters1
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool)) return null;

			var flag = (bool) value;
			
			return flag ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = value as Visibility?;

			return v == Visibility.Visible;
		}
	}
}