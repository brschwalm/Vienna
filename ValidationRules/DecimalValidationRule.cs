﻿using System.Globalization;
using System.Windows.Controls;

namespace Anythink.Wpf.Utilities.ValidationRules
{
	/// <summary>
	/// A Validation Rule for validating decimal values.  Will allow you to assign a Minimum and Maximum value, and an error message
	/// that is displayed when the rule is broken.
	/// </summary>
	public class DecimalValidationRule : ValidationRule
	{
		private decimal _minValue = decimal.MinValue;
		private decimal _maxValue = decimal.MaxValue;
		private string _errorMessage = null;

		/// <summary>
		/// This method is called to Validate the value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			decimal dValue = decimal.MinValue;
			bool isDecimal = false;

			if (value is decimal)
			{
				dValue = (decimal)value;
				isDecimal = true;
			}
			else
			{
				//Try to strip off any extra notation
				string val = value.ToString().Replace("$", "");
				isDecimal = decimal.TryParse(val, out dValue);
			}

			if (!isDecimal || dValue < MinValue || dValue > MaxValue)
			{
				if (ErrorMessage == null)
					ErrorMessage = string.Format(_errorMessage, MinValue.ToString("N"), MaxValue.ToString("N"));

				result = new ValidationResult(false, this.ErrorMessage);
			}

			return result;
		}

		/// <summary>
		/// Gets or Sets the maximum allowable value for the decimal.
		/// </summary>
		public decimal MaxValue { get { return _maxValue; } set { _maxValue = value; } }

		/// <summary>
		/// Gets or sets the minimum allowable value for the decimal.
		/// </summary>
		public decimal MinValue { get { return _minValue; } set { _minValue = value; } }

		/// <summary>
		/// Gets or sets the error message that is returned when the rule is broken.
		/// </summary>
		public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }
	}
}
