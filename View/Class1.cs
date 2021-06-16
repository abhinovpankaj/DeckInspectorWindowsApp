using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace UI.Code.View
{
    //public class CachedImage : Image
    //{
    //    private string _imageUrl;

    //    static CachedImage()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(CachedImage), new FrameworkPropertyMetadata(typeof(CachedImage)));
    //    }

    //    public string ImageUrl
    //    {
    //        get
    //        {
    //            return _imageUrl;
    //        }
    //        set
    //        {
    //            if (value != _imageUrl)
    //            {
    //                Source = new BitmapImage(new Uri(FileCache.FromUrl(value)));
    //                _imageUrl = value;
    //            }
    //        }
    //    }
    //}

    //public class FileCache
    //{
    //    public static string AppCacheDirectory { get; set; }

    //    static FileCache()
    //    {
    //        // default cache directory, can be changed in de app.xaml.
    //        AppCacheDirectory = String.Format("{0}/Cache/", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
    //    }

    //    public static string FromUrl(string url)
    //    {
    //        //Check to see if the directory in AppData has been created 
    //        if (!Directory.Exists(AppCacheDirectory))
    //        {
    //            //Create it 
    //            Directory.CreateDirectory(AppCacheDirectory);
    //        }

    //        //Cast the string into a Uri so we can access the image name without regex 
    //        var uri = new Uri(url);
    //        var localFile = String.Format("{0}{1}", AppCacheDirectory, uri.Segments[uri.Segments.Length - 1]);

    //        if (!File.Exists(localFile))
    //        {
    //            HttpHelper.GetAndSaveToFile(url, localFile);
    //        }

    //        //The full path of the image on the local computer 
    //        return localFile;
    //    }
    //}
    //public class HttpHelper
    //{
    //    public static byte[] Get(string url)
    //    {
    //        WebRequest request = HttpWebRequest.Create(url);
    //        WebResponse response = request.GetResponse();

    //        return response.ReadToEnd();
    //    }

    //    public static void GetAndSaveToFile(string url, string filename)
    //    {
    //        using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
    //        {
    //            byte[] data = Get(url);
    //            stream.Write(data, 0, data.Length);
    //        }
    //    }
    //}

    //public static class WebResponse_extension
    //{
    //    public static byte[] ReadToEnd(this WebResponse webresponse)
    //    {
    //        Stream responseStream = webresponse.GetResponseStream();

    //        using (MemoryStream memoryStream = new MemoryStream((int)webresponse.ContentLength))
    //        {
    //            responseStream.CopyTo(memoryStream);
    //            return memoryStream.ToArray();
    //        }
    //    }
    //}
    public class CachedImage : Image
    {
        public static string AppCacheDirectory { get; set; }
        static CachedImage()
        {
            AppCacheDirectory = String.Format("{0}/", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CachedImage), new FrameworkPropertyMetadata(typeof(CachedImage)));
        }

        public readonly static DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl", typeof(string), typeof(CachedImage), new PropertyMetadata("", ImageUrlPropertyChanged));

        public string ImageUrl
        {
            get
            {
                return (string)GetValue(ImageUrlProperty);
            }
            set
            {
                SetValue(ImageUrlProperty, value);
            }
        }

        private static readonly object SafeCopy = new object();

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = (String)e.NewValue;
            if (String.IsNullOrEmpty(url))
                return;

            var uri = new Uri(url);
            var localFile = String.Format(Path.Combine(AppCacheDirectory, uri.Segments[uri.Segments.Length - 1]));
            var tempFile = String.Format(Path.Combine(AppCacheDirectory, Guid.NewGuid().ToString()));

            if (File.Exists(localFile))
            {
                SetSource((CachedImage)obj, localFile);
            }
            else
            {
                var webClient = new WebClient();
                webClient.DownloadFileCompleted += (sender, args) =>
                {
                    if (args.Error != null)
                    {
                        File.Delete(tempFile);
                        return;
                    }
                    if (File.Exists(localFile))
                        return;
                    lock (SafeCopy)
                    {
                        File.Move(tempFile, localFile);
                    }
                    SetSource((CachedImage)obj, localFile);
                };

                webClient.DownloadFileAsync(uri, tempFile);
            }
        }

        private static void SetSource(Image inst, String path)
        {
            try
            {
                //var image = new BitmapImage(new Uri(path));
               // TransformedBitmap transformBmp = new TransformedBitmap();
                inst.Source = new BitmapImage(new Uri(path));
            }
            catch(Exception ex)
            {

            }
        }
    }
}
