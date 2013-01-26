using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Drawing.Imaging;

namespace Anythink.Wpf.Utilities.MarkupExtensions
{
	/// <summary>
	/// This is a Markup Extension that will allow for simplified access to images/icons from the resouces of an application.
	/// with this extension, you can insert images in your XAML easily.
	/// </summary>
	[MarkupExtensionReturnType(typeof(BitmapFrame))]
	public class ImageResourceExtension : MarkupExtension
	{
		private ImageFormat _imageFormat = ImageFormat.Png;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			Assembly caller = Assembly.GetEntryAssembly();
			var obj = caller.GetType(ClassName).GetProperty(ResourceName, BindingFlags.Static | BindingFlags.NonPublic);
			if (obj != null)
			{
				var val = obj.GetValue(obj, null);
				if (val != null)
				{
					MemoryStream mstr = new MemoryStream();
					BitmapDecoder decoder = null;

					if (val is Icon)
					{
						((Icon)val).Save(mstr);
						decoder = new IconBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);

					}
					else if (val is Bitmap)
					{
						Bitmap bmp = val as Bitmap;
						if (ImageFormat == ImageFormat.Png)
						{
							bmp.Save(mstr, ImageFormat.Png);
							decoder = new PngBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);
						}
						else if (ImageFormat == ImageFormat.Tiff)
						{
							bmp.Save(mstr, ImageFormat.Tiff);
							decoder = new TiffBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);
						}
						else if (ImageFormat == ImageFormat.Gif)
						{
							bmp.Save(mstr, ImageFormat.Gif);
							decoder = new GifBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);
						}
						else if (ImageFormat == ImageFormat.Jpeg)
						{
							bmp.Save(mstr, ImageFormat.Jpeg);
							decoder = new JpegBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);
						}
						else if (ImageFormat == ImageFormat.Bmp)
						{
							bmp.Save(mstr, ImageFormat.Bmp);
							decoder = new BmpBitmapDecoder(mstr, BitmapCreateOptions.None, BitmapCacheOption.None);
						}
					}

					if (decoder != null)
					{
						return decoder.Frames[0];
					}
				}

				return null;
			}
			else
			{
				return null;
			}
		}

		#region Properties

		/// <summary>
		/// Gets or sets the fully-qualified class name of the Resource File where the resource is located.
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the name of the Resource to retrieve
		/// </summary>
		public string ResourceName { get; set; }

		/// <summary>
		/// Gets or sets the image format for this image
		/// </summary>
		public ImageFormat ImageFormat { get { return _imageFormat; } set { _imageFormat = value; } }

		#endregion
	}
}
