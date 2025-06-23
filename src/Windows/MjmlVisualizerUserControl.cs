using Microsoft.VisualStudio.Extensibility.UI;
using MjmlVisualizer.Models;

namespace MjmlVisualizer.Windows;

internal class MjmlVisualizerUserControl(MjmlVisualizerViewModel dataContext) : RemoteUserControl(dataContext)
{
}
