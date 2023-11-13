using MjmlVisualizer.Helpers;
using MjmlVisualizer.Repositories;
using MjmlVisualizer.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace MjmlVisualizer.Windows
{
    public sealed partial class MjmlVisualizerControl
    {
        public MjmlVisualizerControl()
        {
            InitializeComponent();
        }

        public bool IsReadOnly
        {
            get => MjmlTextBox.IsReadOnly;
            set => MjmlTextBox.IsReadOnly = value;
        }

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

            File.WriteAllText(safeFileDialog.FileName, MjmlTextBox.Text);
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

            File.WriteAllText(safeFileDialog.FileName, HtmlTextBox.Text);
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
                HtmlTextBox.Text,
                PreviewWebBrowser.ActualWidth);
        }

        private async void MjmlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var response = await MjmlRepository.GenerateHTML(((TextBox)sender).Text);

            if (response == null)
            {
                MessageBox.Show("String could not be converted");
                return;
            }

            Dispatcher.Invoke(() =>
            {
                MjmlTextBox.Text = response.MJML.Trim();
                HtmlTextBox.Text = response.HTML.Trim();
            });
        }
    }
}
