using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using MjmlVisualizer.Models;
using MjmlVisualizer.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace MjmlVisualizer;

[VisualStudioContribution]
internal class MjmlDebuggerVisualizerProvider : DebuggerVisualizerProvider
{
    /// <inheritdoc/>
    public override DebuggerVisualizerProviderConfiguration DebuggerVisualizerProviderConfiguration
        => new("%MjmlVisualizer.MjmlDebuggerVisualizerProvider.DisplayName%", typeof(string));

    /// <inheritdoc/>
    public override async Task<IRemoteUserControl> CreateVisualizerAsync(VisualizerTarget visualizerTarget, CancellationToken cancellationToken)
        => new MjmlVisualizerUserControl(new MjmlVisualizerViewModel(Extensibility)
        {
            MJML = await visualizerTarget.ObjectSource.RequestDataAsync<string>(jsonSerializer: null, cancellationToken),
        });
}
