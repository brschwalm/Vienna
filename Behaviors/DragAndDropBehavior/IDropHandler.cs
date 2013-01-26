
namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// An interface that defines the methods necessary to handle drop events
	/// </summary>
	public interface IDropHandler
	{
		bool CanDrop(DragDropArguments args);
		void Drop(DragDropArguments args);
	}
}
