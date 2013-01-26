using System.Windows;
using System.Windows.Controls;

namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// Represents the different arguments that are passed around for a drag drop event where an item is being moved
	/// around within a container
	/// </summary>
	public class DragDropArguments
	{
		public FrameworkElement DragElement { get; set; }
		public FrameworkElement DropElement { get; set; }

		public ItemsControl DragContainer { get; set; }
		public ItemsControl DropContainer { get; set; }
	}
}
