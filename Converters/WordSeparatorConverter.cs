using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Anythink.Wpf.Utilities.Converters
{
	/// <summary>
	/// Will separate the parts of a pascal-cased name into separate words.  Good for use with Enumeration values, and
	/// easily converting them to normal-language descriptors.
	/// </summary>
	public class WordSeparatorConverter : IValueConverter
	{
		#region IValueConverter Members

		/// <summary>
		/// Converts from a string that is merged together to a string that is split apart with spaces.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) 
				value = string.Empty;

			else if (value.GetType().Equals(typeof(string)))
			{
				value = WordSeparatorConverter.SplitWords(value.ToString()); //SplitWords(value.ToString());				
			}
			else if(value.GetType().BaseType == typeof(Enum))
			{
				value = WordSeparatorConverter.SplitWords(value.ToString()); //SplitWords(value.ToString());
			}

			return value;
		}

		/// <summary>
		/// Splits a word with MixedCase into words, like Mixed Case.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		//private string SplitWords(string sval)
		//{
		//    List<string> words = new List<string>();
		//    int wordCount = 0;
		//    int start = 0;
		//    for (int i = 1; i < sval.Length; i++)
		//    {
		//        if (char.IsUpper(sval[i]))
		//        {
		//            words.Add(sval.Substring(start, i - start));
		//            wordCount++;
		//            start = i;
		//        }
		//    }

		//    //Grab the last word, if necessary
		//    if (start > 0)
		//    {
		//        words.Add(sval.Substring(start, sval.Length - start));
		//        wordCount++;
		//    }

		//    if (wordCount == 0)
		//        return sval;
		//    else
		//    {
		//        string[] wordArr = new string[words.Count];
		//        words.CopyTo(wordArr);
		//        return string.Join(" ", wordArr).Trim();
		//    }
		//}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Statics

		/// <summary>
		/// Splits a word with MixedCase into words, like Mixed Case.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SplitWords(string multiWordString)
		{
			List<string> words = new List<string>();
			int wordCount = 0;
			int start = 0;
			for (int i = 1; i < multiWordString.Length; i++)
			{
				if (char.IsUpper(multiWordString[i]))
				{
					words.Add(multiWordString.Substring(start, i - start));
					wordCount++;
					start = i;
				}
			}

			//Grab the last word, if necessary
			if (start > 0)
			{
				words.Add(multiWordString.Substring(start, multiWordString.Length - start));
				wordCount++;
			}

			if (wordCount == 0)
				return multiWordString;
			else
			{
				string[] wordArr = new string[words.Count];
				words.CopyTo(wordArr);
				return string.Join(" ", wordArr).Trim();
			}
		}

		#endregion
	}
}
