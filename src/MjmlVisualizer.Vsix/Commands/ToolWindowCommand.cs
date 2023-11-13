using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using MjmlVisualizer.Windows;
using System.Threading.Tasks;

namespace MjmlVisualizer.Vsix
{
    [Command(PackageIds.ToolWindow)]
    internal sealed class ToolWindowCommand : BaseCommand<ToolWindowCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
            => MjmlVisualizerToolWindow.ShowAsync();
    }
}
