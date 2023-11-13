using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;

namespace MjmlVisualizer.Windows
{
    public sealed class MjmlVisualizerToolWindow : BaseToolWindow<MjmlVisualizerToolWindow>
    {
        public override string GetTitle(int toolWindowId) => "MJML Visualizer";

        public override Type PaneType => typeof(Pane);

        public override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
            => new MjmlVisualizerControl
            {
                Margin = new Thickness(6)
            };

        [Guid("bbcfac6c-8b94-4d75-8a32-da42568b0e53")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                // Set an image icon for the tool window
                BitmapImageMoniker = KnownMonikers.EmailAddressViewer;
            }
        }
    }
}