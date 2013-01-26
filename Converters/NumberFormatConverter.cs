using System;
using System.Windows.Data;

namespace Anythink.Wpf.Utilities.Converters
{
	/// <summary>
	/// A converter that will convert numeric values into formatted strings based on a parameter or property
	/// for the string format.
	/// </summary>
	public class NumberFormatConverter : IValueConverter
	{
		#region Member Variables

		private string _format;		//The format to use for this instance
		private bool _useMask;		//Flag for using a mask value rather than 0/min value
		private string _mask = "-";		//the mask value to replace 0 and min value with

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the format for this converter instance
		/// </summary>
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}

		/// <summary>
		/// Gets or sets whether to replace 0 and MinValues with a Mask Value
		/// </summary>
		public bool UseMask
		{
			get { return _useMask; }
			set { _useMask = value; }
		}

		/// <summary>
		/// The mask value to replace 0 and MinValue with, if UseMask is True.
		/// </summary>
		public string Mask
		{
			get { return _mask; }
			set { _mask = value; }
		}

		/// <summary>
		/// Gets or sets whether or not to swap the sign for the value
		/// </summary>
		public bool SwapSign { get; set; }

		#endregion

		#region NumberFormatConverter Interface Implementation

		/// <summary>
		/// Converts the numeric value into a string value using the format provided in the parameter, or in the Format property.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			//Get the format.  The XamGrid ValueEditor passes itself in as a parameter, so make sure the param is a string
			string format = (parameter == null ? Format : (parameter is string ? parameter.ToString() : Format));

			if (parameter is Int16 || parameter is Int32 || parameter is Int64 || parameter is decimal || parameter is double)
			{
				_useMask = true;
				_mask = Convert(parameter, typeof(string), format, culture).ToString();
			}

            if (_useMask && value == null)
            {
                return _mask;
            }
            else if (value == null)
            {
                return string.Empty;
            }
            
            //If there's no format, just return the value
            if (format == null)
            {
				if (SwapSign)
				{
					if (value is int)
						return (int)value * -1;
					if (value is decimal)
						return (decimal)value * -1;
					if (value is double)
						return (double)value * -1;
				}
				else
					return value;
            }

			if (value is Int16 || value is Int32 || value is Int64)
				return NumberFormatConverter.FormatInteger(value, format, _useMask, _mask, SwapSign);

			else if (value is decimal)
				return NumberFormatConverter.FormatDecimal(value, format, _useMask, _mask, SwapSign);

			else if (value is double)
				return NumberFormatConverter.FormatDouble(value, format, _useMask, _mask, SwapSign);

			else
				return value;		//not a supported type

		}

		/// <summary>
		/// Not Implemented
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			
			string unformattedValue = value.ToString().Replace("$", "").Replace(",","").Trim();
			if (unformattedValue == string.Empty) return null;

			//Can we assume 0, or should we deal with MinValue as well.
			if (UseMask && unformattedValue == Mask)
			{
				if (targetType == typeof(decimal))
					return decimal.MinValue;
				else if (targetType == typeof(double))
					return double.MinValue;
				else if (targetType == typeof(Int32))
					return Int32.MinValue;
				else if (targetType == typeof(Int16))
					return Int16.MinValue;
				else if (targetType == typeof(Int64))
					return Int64.MinValue;
				else
					return 0;
			}

			int mult = SwapSign ? -1 : 1;

			if (targetType == typeof(decimal))
				return decimal.Parse(unformattedValue) * mult;
			else if (targetType == typeof(double))
				return double.Parse(unformattedValue) * mult;
			else if (targetType == typeof(Int32))
				return Int32.Parse(unformattedValue) * mult;
			else if (targetType == typeof(Int16))
				return Int16.Parse(unformattedValue) * mult;
			else if (targetType == typeof(Int64))
				return Int64.Parse(unformattedValue) * mult;
			else
				return 0;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Will format an integer value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string FormatInteger(object value, string format)
		{
			return NumberFormatConverter.FormatInteger(value, format, false, null, false);
		}

		/// <summary>
		/// Will format an integer value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <param name="useMask"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		public static string FormatInteger(object value, string format, bool useMask, string mask, bool swapSign)
		{
			int multi = swapSign ? -1 : 1;

			if (value is Int16)
			{
				Int16 val = System.Convert.ToInt16(value);
				if (useMask && val == Int16.MinValue)
					return mask;
				else
					return (val * multi).ToString(format);
			}
			else if (value is Int32)
			{
				Int32 val = System.Convert.ToInt32(value);
				if(useMask && val == Int32.MinValue)
					return mask;
				else
					return (val * multi).ToString(format);
			}
			else if (value is Int64)
			{
				Int64 val = System.Convert.ToInt64(value);
				if (useMask && val == Int64.MinValue)
					return mask;
				else
					return (val * multi).ToString(format);
			}
			else
				throw new ArgumentException("Invalid type, not an integer", "value");
		}

		/// <summary>
		/// Formats a Decimal Value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string FormatDecimal(object value, string format)
		{
			return NumberFormatConverter.FormatDecimal(value, format, false, null, false);
		}

		/// <summary>
		/// Formats a Decimal Value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <param name="mask"></param>
		/// <param name="useMask"></param>
		/// <returns></returns>
		public static string FormatDecimal(object value, string format, bool useMask, string mask, bool swapSign)
		{
			int multi = swapSign ? -1 : 1;

			if (value is decimal)
			{
				decimal val = System.Convert.ToDecimal(value);
				if(useMask && val == decimal.MinValue	)
					return mask;
				else
					return (val * multi).ToString(format);
			}
			else
				throw new ArgumentException("Invalid type, not a decimal", "value");
		}

		/// <summary>
		/// Formats a Double value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string FormatDouble(object value, string format)
		{
			return NumberFormatConverter.FormatDouble(value, format, false, null, false);
		}

		/// <summary>
		/// Formats a Double value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <param name="useMask"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		public static string FormatDouble(object value, string format, bool useMask, string mask, bool swapSign)
		{
			int multi = swapSign ? -1 : 1;
			
			if (value is double)
			{	
				double val = System.Convert.ToDouble(value);
				if(useMask && val == double.MinValue)
					return mask;
				else
					return (val * multi).ToString(format);
			}
			else
				throw new ArgumentException("Invalid type, not a decimal", "value");
		}


		public static decimal UnformatDecimal(string value)
		{
			string unformattedValue = value.ToString().Replace("$", "").Replace(",","").Trim();
			if (unformattedValue == string.Empty || unformattedValue == ".") return decimal.MinValue;

			return decimal.Parse(unformattedValue);
		}

		public static double UnformatDouble(string value)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty || unformattedValue == ".") return double.MinValue;

			return double.Parse(unformattedValue);
		}

		public static Int32 UnformatInt32(string value)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty) return Int32.MinValue;

			return Int32.Parse(unformattedValue);
		}

		public static Int16 UnformatInt16(string value)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty) return Int16.MinValue;

			return Int16.Parse(unformattedValue);
		}

		public static Int64 UnformatInt64(string value)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty) return Int64.MinValue;

			return Int64.Parse(unformattedValue);
		}

		#region Try Unformat Methods

		public static bool TryUnformatDecimal(string value, out decimal result)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty)
			{
				result = decimal.MinValue;
				return false;
			}
			else
				return decimal.TryParse(unformattedValue, out result);

		}

		public static bool TryUnformatDouble(string value, out double result)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty)
			{
				result = double.MinValue;
				return false;
			}
			else
				return double.TryParse(unformattedValue, out result);

		}

		public static bool TryUnformatInt64(string value, out Int64 result)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty)
			{
				result = Int64.MinValue;
				return false;
			}
			else
				return Int64.TryParse(unformattedValue, out result);

		}

		public static bool TryUnformatInt32(string value, out Int32 result)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty)
			{
				result = Int32.MinValue;
				return false;
			}
			else
				return Int32.TryParse(unformattedValue, out result);

		}

		public static bool TryUnformatInt16(string value, out Int16 result)
		{
			string unformattedValue = value.ToString().Replace("$","").Replace(",","").Trim();
			if (unformattedValue == string.Empty)
			{
				result = Int16.MinValue;
				return false;
			}
			else
				return Int16.TryParse(unformattedValue, out result);

		}
		
		#endregion


		#endregion
	}
}
