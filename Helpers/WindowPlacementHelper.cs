using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Xml.Serialization;

namespace Anythink.Wpf.Utilities.Helpers
{
	#region Structures
	/// <summary>
	/// RectangleValue structure required by WindowPlacement structure
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct RectangleValue
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RectangleValue(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}
	}

	/// <summary>
	/// PointValue structure required by WindowPlacement structure
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct PointValue
	{
		public int X;
		public int Y;

		public PointValue(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	/// <summary>
	/// WindowPlacement stores the position, size, and state of a window
	/// </summary>
	/// <remarks>This struct is used with unmanaged code</remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct WindowPlacement
	{
		public int Length;
		public int Flag;
		public int ShowCommand;
		public PointValue MinimumPosition;
		public PointValue MaximumPosition;
		public RectangleValue NormalPosition;
	}

	#endregion

	/// <summary>
	/// Helper to facilitate the saving and restoring of a window to its last position
	/// </summary>
	public class WindowPlacementHelper
	{
		private static XmlSerializer _serializer;
		
		#region Win32 API Declarations
		[DllImport("user32.dll")]
		static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

		[DllImport("user32.dll")]
		static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

		const int WindowShowNormal = 1;
		const int WindowShowMinimized = 2;
		const int WindowShowMaximized = 3;
		#endregion

		#region Properties

		/// <summary>
		/// Property to facilitate on-demand creation and caching of a serializer for the WindowPlacement structure.
		/// </summary>
		private static XmlSerializer Serializer //XmlSerializer<WindowPlacement> Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new XmlSerializer(typeof(WindowPlacement));
				}

				return _serializer;
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Will position a window based on the Placement Info string
		/// </summary>
		/// <param name="placementInfo">a Serialized WindowPlacement structure</param>
		/// <param name="window">The window to position</param>
		public static void SetWindowPlacement(string placementInfo, Window window)
		{
			SetWindowPlacement(placementInfo, window, true, true);
		}

		/// <summary>
		/// Will position a window based on the Placement Info string.  Also, allows for overriding the position
		/// to prevent placement of minimzed and/or maximized.
		/// </summary>
		/// <param name="placementInfo">a Serialized WindowPlacement structure</param>
		/// <param name="window">The window to position</param>
		/// <param name="allowMaximized">Whether or not maximized windows are positioned as such</param>
		/// <param name="allowMinimized">Whether or not minimized windows are positioned as such</param>
		public static void SetWindowPlacement(string placementInfo, Window window, bool allowMinimized, bool allowMaximized)
		{
			WindowPlacement wp = Serializer.DeserializeFromString<WindowPlacement>(placementInfo);

			IntPtr windowHandle = new WindowInteropHelper(window).Handle;

			if ((!allowMinimized && wp.ShowCommand == WindowShowMinimized) || (!allowMaximized && wp.ShowCommand == WindowShowMaximized))
			{
				wp.ShowCommand = WindowShowNormal;
			}

			SetWindowPlacement(windowHandle, ref wp);
		}

		/// <summary>
		/// Sets the placement of a window
		/// </summary>
		/// <param name="placement">The position of the window</param>
		/// <param name="window">The window to position</param>
		public static void SetWindowPlacement(WindowPlacement placement, Window window)
		{
			SetWindowPlacement(placement, window, true, true);
		}

		/// <summary>
		/// Sets the placement of a window
		/// </summary>
		/// <param name="placement">The position of the window</param>
		/// <param name="window">The window to position</param>
		/// <param name="allowMaximized">Whether or not maximized windows are positioned as such</param>
		/// <param name="allowMinimized">Whether or not minimized windows are positioned as such</param>
		public static void SetWindowPlacement(WindowPlacement placement, Window window, bool allowMaximized, bool allowMinimized)
		{
			try
			{
				// Load window placement details for previous application session from application settings
				// Note - if window was closed on a monitor that is now disconnected from the computer,
				//        SetWindowPlacement will place the window onto a visible monitor.

				if (placement.ShowCommand != 0)
				{
					WindowPlacement windowPlacement = placement;
					windowPlacement.Length = Marshal.SizeOf(typeof(WindowPlacement));
					windowPlacement.Flag = 0;

					if ((!allowMinimized && placement.ShowCommand == WindowShowMinimized) || (!allowMaximized && placement.ShowCommand == WindowShowMaximized))
					{
						placement.ShowCommand = WindowShowNormal;
					}

					IntPtr windowHandle = new WindowInteropHelper(window).Handle;
					SetWindowPlacement(windowHandle, ref windowPlacement);
				}
			}
			catch { }
		}

		/// <summary>
		/// Gets the WindowPlacement structure for a window.
		/// </summary>
		/// <param name="window">The Window to get the placement info for.</param>
		/// <returns>The WindowPlacement structure with window placement info</returns>
		public static WindowPlacement GetWindowPlacement(Window window)
		{
			// Persist window placement details to application settings
			WindowPlacement windowPlacement = new WindowPlacement();
			IntPtr windowHandle = new WindowInteropHelper(window).Handle;
			GetWindowPlacement(windowHandle, out windowPlacement);

			return windowPlacement;
		}

		/// <summary>
		/// Gets the WindowPlacement for a window as an XML serialized string.
		/// </summary>
		/// <param name="window">The Window to get the placement info for.</param>
		/// <returns>A string with the serialized WindowPlacement structure</returns>
		public static string GetWindowPlacementString(Window window)
		{
			WindowPlacement wp = GetWindowPlacement(window);
			return Serializer.SerializeToString(wp);
		}

		#endregion

	}
}
