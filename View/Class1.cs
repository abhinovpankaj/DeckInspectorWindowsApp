using EnvDTE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UI.Code.View
{
    
    public class CachedImage : System.Windows.Controls.Image
    {
        public static string AppCacheDirectory { get; set; }
        static CachedImage()
        {
            AppCacheDirectory = String.Format("{0}\\", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
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
            try
            {
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
            catch{

            }
            
        }

        private static void SetSource(System.Windows.Controls.Image inst, String path)
        {
           
            using (var objImage = System.Drawing.Image.FromFile(path))
            {
                try
                {
                    //if (!File.Exists(path))
                   // {
                        FixImage(objImage, path);
                        objImage.Save(path);
                    //}
                }
                catch (Exception ex)
                {

                }
                inst.Source = new BitmapImage(new Uri(path));


            }

         
        }

        private static void FixImage(System.Drawing.Image img, string path)
        {
            if (Array.IndexOf(img.PropertyIdList, 274) > -1)
            {
                var orientation = (int)img.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        img.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
               //This EXIF data is now invalid and should be removed.
                img.RemovePropertyItem(274);
                                  
            }
        }

       

    }
}
