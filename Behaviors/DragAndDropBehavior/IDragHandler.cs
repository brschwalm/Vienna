
namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// An interface defining an item that can be dragged with the DragDropBehavior
	/// </summary>
	public interface IDragHandler
	{
		bool CanDrag(DragDropArguments args);
	}
}
