﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Anythink.Wpf.Utilities.Helpers
{
	/// <summary>
	/// Provides some basic utility methods to help with WPF Controls
	/// </summary>
	public static class ControlHelper
	{
		private static bool? _isInDesignMode;

		public static bool IsInDesignMode
		{
			get
			{
				if (!_isInDesignMode.HasValue)
				{
					var prop = DesignerProperties.IsInDesignModeProperty;
					_isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
				}

				return _isInDesignMode.Value;
			}
		}

		#region Static Methods

		/// <summary>
		/// Will navigate down the VisualTree to find an element that is of the provided type.
		/// </summary>
		/// <typeparam name="T">The type of object to search for</typeparam>
		/// <param name="element">The element to start searching at</param>
		/// <returns></returns>
		public static T GetChild<T>(ContentControl element)
		{
			T child = default(T);

			while (element != null && element.HasContent)
			{
				if (element.Content is T)
				{
					child = (T)element.Content;
					break;
				}
				else
				{
					element = element.Content as ContentControl;
					if (element == null)
					{
						break;
					}
				}
			}

			return child;
		}

		/// <summary>
		/// Will navigate up the VisualTree to find an element that is of the provided type.
		/// </summary>
		/// <typeparam name="T">The type of object to search for</typeparam>
		/// <param name="element">The element to start searching at</param>
		/// <returns></returns>
		public static T GetParent<T>(FrameworkElement element)
		{
			T parent = default(T);

			while (element != null && element.Parent != null)
			{
				if (element.Parent is T)
				{
					parent = (T)(object)element.Parent;
					break;
				}
				else
				{
					element = element.Parent as FrameworkElement;
					if (element == null)
					{
						break;
					}
				}
			}

			return parent;
		}

		#endregion

	}
}
