﻿using System;
using System.Windows;
using System.Windows.Input;

namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// This is a behavior that can be applied to a window, and will allow you to set the
	/// control that has focus when the window is first activated.
	/// </summary>
	public static class WindowFocusBehavior
	{
		public static string GetWindowFocusedElement(Window window)
		{
			return (string)window.GetValue(WindowFocusedElementProperty);
		}

		public static void SetWindowFocusedElement(Window window, string elementName)
		{
			window.SetValue(WindowFocusedElementProperty, elementName);
		}

		public static readonly DependencyProperty WindowFocusedElementProperty = DependencyProperty.RegisterAttached("WindowFocusedElement", typeof(string), typeof(Window), new UIPropertyMetadata(OnWindowFocusedElementChanged));

		private static void OnWindowFocusedElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				string elementName = e.NewValue as string;
				if (elementName == null)
				{
					window.Activated -= OnWindowActivated;
				}
				else
				{
					window.Activated += OnWindowActivated;
				}
			}
		}

		private static void OnWindowActivated(object sender, EventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				string elementName = window.GetValue(WindowFocusedElementProperty) as string;
				if (elementName != null)
				{
					UIElement element = window.FindName(elementName) as UIElement;
					if (element != null)
					{
						element.Focus();
						Keyboard.Focus(element);

						//Unsubscribe so I don't move focus on subsequent times the window is activated
						window.Activated -= OnWindowActivated;
					}
				}
			}
		}
	}
}
