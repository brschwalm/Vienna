using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Anythink.Wpf.Utilities.Converters
{
	/// <summary>
	/// Converter that will convert to/from booleans and visibility values.  Also includes the ability to invert (meaning swap direction of 
	/// visibility and boolean) and Not, meaning swith true for false.
	/// </summary>
	public class BetterBooleanToVisibilityConverter : IValueConverter, IMultiValueConverter
	{

		#region Member Variables

		private bool _inverted;		//indicates which direction the conversion should go.  True (default) will go from Bool to Visibility, False will go from Visibility to Bool
		private bool _not;			//indicates whether we are comparing against "Not" true.
		private Visibility _notVisibility = Visibility.Collapsed;		//The visibility to use when the not case is true
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether to use the ^ operand (logical-exclusive-or)
		/// </summary>
		public bool Inverted
		{
			get { return _inverted; }
			set { _inverted = value; }
		}

		/// <summary>
		/// Gets or sets whether this converter should "Not" the boolean value.  If Not = True, True = Visibility.Collapsed and False = Visibility.Visible.  If
		/// Inverted = True, then switch boolean for visibility in above statement.
		/// </summary>
		public bool Not
		{
			get { return _not; }
			set { _not = value; }
		}

		public Visibility NotShownVisibility
		{
			get { return _notVisibility; }
			set { _notVisibility = value; }
		}

		/// <summary>
		/// Gets or sets whether this converter will use Null/Not Null as the check.  If true, this converter
		/// will check for null (false) or not null.  If not null, and not a boolean, it not null will equate to true,
		/// otherwise if not null and a boolean, the boolean value will be evaluated.
		/// </summary>
		public bool HandleNulls { get; set; }
		#endregion

		#region Methods

		/// <summary>
		/// Converts a visibility value to a boolean value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private object VisibilityToBool(object value)
		{
			if (!(value is Visibility))
				return DependencyProperty.UnsetValue;

			return (((Visibility)value) == Visibility.Visible) ^ Not;
		}

		/// <summary>
		/// Converts a boolean value to a visibility
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private object BoolToVisibility(object value)
		{
			if (!(value is bool))
			{
				if (HandleNulls)		//In this case, check against null/not null for the visibility
				{
					value = (value != null);
					return BoolToVisibility(value);
				}
				else
					return DependencyProperty.UnsetValue;
			}

			return ((bool)value ^ Not) ? Visibility.Visible
				: NotShownVisibility;
		}

		#endregion

		#region IValueConverter Interface Implementation

		/// <summary>
		/// Converts a boolean to a visibility or vice-versa, depending on the settings.
		/// </summary>
		/// <param name="value">The value to convert from</param>
		/// <param name="targetType">The type to convert to</param>
		/// <param name="parameter">Not used</param>
		/// <param name="culture">The culture</param>
		/// <returns>a boolean or a visibility, depending on the Inverted setting.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (parameter != null)
            {
                bool result;
                if (bool.TryParse(parameter.ToString(), out result))
                    Inverted = result;
            }

		    return Inverted ? VisibilityToBool(value)
				: BoolToVisibility(value);
		}

		/// <summary>
		/// Converts back from a boolean to a visibility or vice-versa, depending on the settings.
		/// </summary>
		/// <param name="value">The value to convert back from</param>
		/// <param name="targetType">The type to convert to</param>
		/// <param name="parameter">Not used</param>
		/// <param name="culture">The culture</param>
		/// <returns>a boolean or a visibility, depending on the Inverted setting.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (parameter != null)
            {
                bool result;
                if (bool.TryParse(parameter.ToString(), out result))
                    Inverted = result;
            }

            return Inverted ? BoolToVisibility(value)
				: VisibilityToBool(value);
		}

		#endregion

		#region IMultiValueConverter Members

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter != null)
			{
				bool result;
				if (bool.TryParse(parameter.ToString(), out result))
					Inverted = result;
			}

			bool value = true;
			foreach (bool val in values)
				value &= val;

			return Inverted ? VisibilityToBool(value)
				: BoolToVisibility(value);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}


}
