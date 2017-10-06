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
        private static readonly Dictionary<Uri, BitmapImage> _cache = new Dictionary<Uri, BitmapImage>();

	    public static BitmapSource GetBitmapImage(string uriSource)
	    {
		    return GetBitmapImage(new Uri(uriSource));
	    }

	    public static BitmapSource GetBitmapImage(Uri uriSource)
        {
            if (!_cache.ContainsKey(uriSource))
            {
                BitmapImage bitmapImage = null;
                try
                {
                    bitmapImage = new BitmapImage(uriSource);
                }
                catch (Exception) { } 

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
	}
}
