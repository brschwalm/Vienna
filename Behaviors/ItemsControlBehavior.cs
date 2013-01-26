using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// This is a behavior that will allow you to apply alternating styles or background colors
	/// to items in an ItemsControl.
	/// </summary>
	public class ItemsControlBehavior
	{
		#region Alternate Item Container Style

		public static readonly DependencyProperty AlternateItemContainerStyleProperty = DependencyProperty.RegisterAttached(
			"AlternateItemContainerStyle", 
			typeof(Style), 
			typeof(ItemsControlBehavior), 
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,new PropertyChangedCallback(OnAlternateItemContainerStyleChanged)));

		public static void SetAlternateItemContainerStyle(DependencyObject element, Style value)
		{
			element.SetValue(AlternateItemContainerStyleProperty, value);
		}

		public static Style GetAlternateItemContainerStyle(DependencyObject element)
		{
			return (Style)element.GetValue(AlternateItemContainerStyleProperty);
		}

		private static void OnAlternateItemContainerStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl control = sender as ItemsControl;
			if (e.NewValue != null && control != null)
			{
				if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					SetAlternateItemContainerStyle(control, (Style)e.NewValue);
				}
				else
				{
					control.ItemContainerGenerator.StatusChanged += delegate
					{
						if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
						{
							SetAlternateItemContainerStyle(control, (Style)e.NewValue);
						}
					};
				}
			}
		}

		private static void SetAlternateItemContainerStyle(ItemsControl control, Style alternateStyle)
		{
			if (control.Items != null && control.Items.Count > 0)
			{
				for (Int32 i = 0; i < control.Items.Count; i++)
				{
					if (i % 2 != 0)
					{
						FrameworkElement container = control.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
						if (container != null) container.Style = alternateStyle;
					}
				}
			}
		}

		#endregion

		#region Alternate Item Background Style

		public static readonly DependencyProperty AlternateItemBackgroundProperty = DependencyProperty.RegisterAttached(
			"AlternateItemBackground",
			typeof(Brush),
			typeof(ItemsControlBehavior),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnAlternateItemBackgroundChanged)));

		public static void SetAlternateItemBackground(DependencyObject element, Brush value)
		{
			element.SetValue(AlternateItemBackgroundProperty, value);
		}

		public static Brush GetAlternateItemBackground(DependencyObject element)
		{
			return (Brush)element.GetValue(AlternateItemBackgroundProperty);
		}

		private static void OnAlternateItemBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl control = sender as ItemsControl;
			if (e.NewValue != null && control != null)
			{
				if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					SetAlternateItemBackground(control, (Brush)e.NewValue);
				}
				else
				{
					control.ItemContainerGenerator.StatusChanged += delegate
					{
						if (control.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
						{
							SetAlternateItemBackground(control, (Brush)e.NewValue);
						}
					};
				}
			}
		}

		private static void SetAlternateItemBackground(ItemsControl control, Brush alternateBrush)
		{
			if (control.Items != null && control.Items.Count > 0)
			{
				for (Int32 i = 0; i < control.Items.Count; i++)
				{
					if (i % 2 != 0)
					{
						Control container = control.ItemContainerGenerator.ContainerFromIndex(i) as Control;
						if (container != null) container.Background = alternateBrush;
					}
				}
			}
		}

		#endregion
	}
}
