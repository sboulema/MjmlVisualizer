using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MjmlVisualizer.Helpers
{
    public static class WebBrowserHelper
    {
        private static WebBrowser _webBrowser;
        private static string _outputFile;
        private static double _width;

        public static void TakeScreenshot(string outputFile, string html, double width)
        {
            _outputFile = outputFile;
            _width = width;

            _webBrowser = new WebBrowser();
            _webBrowser.ProgressChanged += wb_ProgressChanged;
            _webBrowser.ScriptErrorsSuppressed = true;
            _webBrowser.ScrollBarsEnabled = false;

            var tempPath = SaveHtmlToTempFile(html);

            _webBrowser.Navigate(tempPath);
        }

        private static string SaveHtmlToTempFile(string html)
        {
            var path = Path.Combine(Path.GetTempPath(), "index.html");
            File.WriteAllText(path, html);
            return path;
        }

        private static void wb_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.CurrentProgress != e.MaximumProgress)
            {
                return;
            }

            _webBrowser.ProgressChanged -= wb_ProgressChanged;

            try
            {
                var scrollHeight = _webBrowser.Document.Body.ScrollRectangle.Height;
                _webBrowser.Size = new Size(Convert.ToInt32(_width), scrollHeight);

                var bitmap = new Bitmap(_webBrowser.Width, _webBrowser.Height);

                for (var x = 0; x < bitmap.Width; x++)
                {
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }
                }

                _webBrowser.DrawToBitmap(bitmap, new Rectangle(0, 0, _webBrowser.Width, _webBrowser.Height));

                bitmap.Save(_outputFile, ImageFormat.Jpeg);
            }
            catch { }
        }
    }
}
