using System;
using System.Windows.Data;

namespace Anythink.Wpf.Utilities.Converters
{
	/// <summary>
	/// This converter allows for simple debugging.  If it is used with a binding, you can set a breakpoint in this
	/// converter and see what the value, parameter, etc. are that are passed into the binding.
	/// </summary>
	public class DebugConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			System.Diagnostics.Debug.WriteLine("Value: " + value.ToString());
			if (parameter != null)
			{
				System.Diagnostics.Debug.WriteLine("Parameter: " + parameter.ToString());
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
