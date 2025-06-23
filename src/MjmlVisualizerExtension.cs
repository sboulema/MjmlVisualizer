using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace MjmlVisualizer;

[VisualStudioContribution]
internal class MjmlVisualizerExtension : Extension
{
    /// <inheritdoc/>
    public override ExtensionConfiguration ExtensionConfiguration => new()
    {
        Metadata = new(
            id: "MjmlVisualizer.771344e3-c441-46ee-88bd-2295144a2ef8",
            version: ExtensionAssemblyVersion,
            publisherName: "Samir Boulema",
            displayName: "MjmlVisualizer",
            description: "Adds a debug visualizer to easily see MJML, HTML source and rendered HTML.")
        {
            MoreInfo = "https://github.com/sboulema/MjmlVisualizer",
            ReleaseNotes = "https://github.com/sboulema/MjmlVisualizer/releases",
            Tags = ["Debug", "Visualizer", "MJML", "HTML", "Email"],
            // TODO: BUG? License file is missing
            //License = "Resources/License.txt",
            Icon = "Resources/Logo_90x90.png",
            PreviewImage = "Resources/ScreenshotPreview.png",
        },
    };

    /// <inheritdoc />
    protected override void InitializeServices(IServiceCollection serviceCollection)
    {
        base.InitializeServices(serviceCollection);

        // You can configure dependency injection here by adding services to the serviceCollection.
    }
}
