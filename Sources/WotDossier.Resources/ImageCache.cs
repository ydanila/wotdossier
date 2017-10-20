using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace WotDossier.Resources
{
    public class ImageCache
    {
        private static readonly Dictionary<Uri, BitmapSource> _cache = new Dictionary<Uri, BitmapSource>();

	    public static BitmapSource GetBitmapImage(string uriSource)
	    {
		    return GetBitmapImage(new Uri(uriSource));
	    }

	    public static BitmapSource GetBitmapImage(Uri uriSource)
        {
            if (!_cache.ContainsKey(uriSource))
            {
                BitmapSource bitmapImage = null;
                try
                {
                    bitmapImage = new BitmapImage(uriSource);
                }
                catch (Exception) { }

	            if (bitmapImage.DpiX != 96)
		            bitmapImage = ConvertBitmapTo96DPI(bitmapImage);


				_cache.Add(uriSource, bitmapImage);
            }
            return _cache[uriSource];
        }

	    public static BitmapSource GetBitmapImage(Uri uriSource, Point location, Size size)
	    {
		    var image = GetBitmapImage(uriSource);
		    if (image == null) return null;

			return new CroppedBitmap(image, new Int32Rect(location.X, location.Y, size.Width, size.Height));
	    }

	    public static BitmapSource GetBitmapImage(string uriSource, Point location, Size size)
	    {
		    return GetBitmapImage(new Uri(uriSource), location, size);
	    }

		public static BitmapSource ConvertBitmapTo96DPI(BitmapSource bitmapImage)
		{
			double dpi = 96;
			int width = bitmapImage.PixelWidth;
			int height = bitmapImage.PixelHeight;

			int stride = width * bitmapImage.Format.BitsPerPixel;
			byte[] pixelData = new byte[stride * height];
			bitmapImage.CopyPixels(pixelData, stride, 0);

			return BitmapSource.Create(width, height, dpi, dpi, bitmapImage.Format, null, pixelData, stride);
		}
	}
}
