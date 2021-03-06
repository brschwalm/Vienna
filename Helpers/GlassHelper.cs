using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Anythink.Wpf.Utilities.Helpers
{
	public class Colorization
	{
        public Color WindowManagerColor { get; set; }
        public bool Opaque { get; set; }
	}

    /// <summary>
    /// Wrapper for Interop code to Extend Vista Glass Frame into the application
    /// </summary>
    public static class GlassHelper
    {
        struct MARGINS
        {
            public MARGINS(Thickness t)
            {
                Left = (int)t.Left;
                Right = (int)t.Right;
                Top = (int)t.Top;
                Bottom = (int)t.Bottom;
            }
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs"), DllImport("dwmapi.dll", PreserveSig = false)]
		static extern bool DwmIsCompositionEnabled();
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs"), DllImport("dwmapi.dll")]
		static extern void DwmGetColorizationColor(out uint colorizationColor, out bool colorizationOpaqueBlend);

        public static Colorization GetColor()
        {
            Colorization clr = new Colorization();
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            if (vs.Major < 6)
            {
                clr.Opaque = false;
                clr.WindowManagerColor = Colors.Gray;
            }
            else
            {
                uint colorization;
                bool opaque;
                GlassHelper.DwmGetColorizationColor(out colorization, out opaque);
                clr.Opaque = opaque;

                clr.WindowManagerColor = Color.FromArgb((byte)(colorization >> 24), (byte)(colorization >> 16), (byte)(colorization >> 8), (byte)(colorization));
            }
            return clr;
        }

        public static bool ExtendGlassFrame(Window window, Thickness margin)
        {
            // Get the Operating System From Environment Class
            OperatingSystem os = Environment.OSVersion;

            // Get the version information
            Version vs = os.Version;

            if (vs.Major < 6)
                return false;

            if (!DwmIsCompositionEnabled())
                return false;

            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("Glass cannot be extended before the window is shown.");

            // Set the background to transparent from both the WPF and Win32 perspectives
            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

            MARGINS margins = new MARGINS(margin);
            DwmExtendFrameIntoClientArea(hwnd, ref margins);

            return true;
        }
    }
}
