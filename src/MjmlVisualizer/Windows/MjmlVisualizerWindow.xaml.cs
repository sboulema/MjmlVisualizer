using MjmlVisualizer.Helpers;
using MjmlVisualizer.ViewModels;
using System.IO;
using System.Windows;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace MjmlVisualizer.Windows
{
    public partial class MjmlVisualizerWindow : Window
    {
        public MjmlVisualizerWindow(MjmlVisualizerViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
            => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
            => Close();

        private void OnMjmlSaveButtonClick(object sender, RoutedEventArgs e)
        {
            var safeFileDialog = new SaveFileDialog
            {
                Filter = "MJML Template|*.mjml",
                Title = "Save a MJML Template"
            };

            safeFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(safeFileDialog.FileName))
            {
                return;
            }

            File.WriteAllText(safeFileDialog.FileName, (DataContext as MjmlVisualizerViewModel).MJML);
        }

        private void OnHtmlSaveButtonClick(object sender, RoutedEventArgs e)
        {
            var safeFileDialog = new SaveFileDialog
            {
                Filter = "HTML Webpage|*.html",
                Title = "Save a HTML Webpage"
            };

            safeFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(safeFileDialog.FileName))
            {
                return;
            }

            File.WriteAllText(safeFileDialog.FileName, (DataContext as MjmlVisualizerViewModel).HTML);
        }

        private void OnPreviewSaveButtonClick(object sender, RoutedEventArgs e)
        {
            var safeFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg",
                Title = "Save a JPEG Image"
            };

            safeFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(safeFileDialog.FileName))
            {
                return;
            }

            WebBrowserHelper.TakeScreenshot(
                safeFileDialog.FileName,
                (DataContext as MjmlVisualizerViewModel).HTML,
                PreviewWebBrowser.ActualWidth);
        }       
    }
}
