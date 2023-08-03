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
    }
}
