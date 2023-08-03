using MjmlVisualizer.ViewModels;
using System.Windows;

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
    }
}
