using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Anythink.Wpf.Utilities.Helpers
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Serializes a value to string
		/// </summary>
		/// <param name="serializer"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SerializeToString(this XmlSerializer serializer, object value)
		{
			StringBuilder valueString = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(valueString);
			using (writer)
			{
				serializer.Serialize(writer, value);
			}

			return valueString.ToString();
		}

		/// <summary>
		/// Deserializes from a string to the desired type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer"></param>
		/// <param name="valueString"></param>
		/// <returns></returns>
		public static T DeserializeFromString<T>(this XmlSerializer serializer, string valueString)
		{
			StringReader sr = new StringReader(valueString);
			using (sr)
			{
				XmlTextReader reader = new XmlTextReader(sr);
				return (T)serializer.Deserialize(reader);
			}
		}
	}
}
