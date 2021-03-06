﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Anythink.Wpf.Utilities.Helpers
{
	/// <summary>
	/// A collection of utilities methods that supplement the built-in VisualTreeHelper of WPF.
	/// </summary>
	public class VisualTreeHelperEx
	{
		/// <summary>
		/// Finds the ancestor of an element by its type
		/// </summary>
		/// <param name="element"></param>
		/// <param name="ancestorType"></param>
		/// <returns></returns>
		public static DependencyObject FindAncestor(DependencyObject element, Type ancestorType)
		{
			//Make sure its not null (and isn't the item we're looking for)
			if (element == null || element.GetType() == ancestorType)
				return element;

			//Get the parent of this element
			DependencyObject parent = VisualTreeHelper.GetParent(element);

			//Check to see if the parent is the right type, if not, call this recursively
			if (parent.GetType() == ancestorType)
				return parent;
			else if (parent == null)
				return null;
			else
				return FindAncestor(parent, ancestorType);
		}

		/// <summary>
		/// Gets the Container for a visual element in an Items Control
		/// </summary>
		/// <param name="itemsControl"></param>
		/// <param name="visual"></param>
		/// <returns></returns>
		public static FrameworkElement FindItemContainer(ItemsControl itemsControl, Visual visual)
		{
			if (itemsControl == null)
				throw new ArgumentNullException("itemsControl");
			if (visual == null)
				throw new ArgumentNullException("visual");

			if (itemsControl.Items.Count < 1)
			{
				throw new ArgumentException("itemsControl", "Items Control must contain items");
			}

			FrameworkElement container = null;

			//Need to figure out what type of container the ItemsControl uses
			DependencyObject containerTypeContainer = itemsControl.ItemContainerGenerator.ContainerFromIndex(0);
			if (containerTypeContainer != null)
			{
				Type containerType = containerTypeContainer.GetType();
				container = visual.FindAncestor(containerType);

				//Confirm it belongs to the ItemsControl we passed in
				if (container != null && itemsControl.DataContext != null)
				{
					//Get the item container for the DataContext of this container to confirm it's the same as the one we already found
					FrameworkElement verifyContainer = itemsControl.ItemContainerGenerator.ContainerFromItem(container.DataContext) as FrameworkElement;
					if (container != verifyContainer)
					{
						container = null;
					}
				}
			}


			return container;

		}

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

		/// <summary>
		/// Finds the parent of a dependency object in the visual tree of a certain type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObject"></param>
		/// <returns></returns>
		public static T GetParentEx<T>(DependencyObject depObject) where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(depObject);

			while (depObject != null && parent != null && !(parent is T))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}

			return parent as T;
		}
	}

	/// <summary>
	/// A collection of Extension Methods that will help with dealing with the VisualTree
	/// </summary>
	public static class VisualExtensions
	{
		public static T FindAncestor<T>(this Visual visual) where T : class
		{
			return FindAncestor<T>(visual, 1);
		}

		public static T FindAncestor<T>(this Visual visual, int level) where T : class
		{
			Type ancestorType = typeof(T);
			return visual.FindAncestor(ancestorType, level) as T;
		}

		public static FrameworkElement FindAncestor(this Visual visual, Type ancestorType)
		{
			return visual.FindAncestor(ancestorType, 1);
		}

		public static FrameworkElement FindAncestor(this Visual visual, Type ancestorType, int level)
		{
			Visual vis = visual;

			while (level >= 1)
			{
				while (vis != null && !ancestorType.IsInstanceOfType(vis))
				{
					vis = (Visual)VisualTreeHelper.GetParent(vis);
				}

				level--;
			}

			return vis as FrameworkElement;
		}

	}
}
