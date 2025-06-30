using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MjmlVisualizer.Converters;

public static class HtmlConverter
{
    // https://github.com/microsoft/VSExtensibility/issues/390
    public static async Task FromPath(string inputPath, string outputPath, CancellationToken cancellationToken)
    {
        var tempFolder = EnsureTempFolder();

        await EnsureWkHtmlToImage(tempFolder, cancellationToken);

        var process = Process.Start(new ProcessStartInfo(
            fileName: Path.Combine(tempFolder, "wkhtmltoimage.exe"),
            arguments: $"--quality 100 --width 1024 -f jpg {inputPath} \"{outputPath}\"")
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            WorkingDirectory = tempFolder,
            RedirectStandardError = true
        });
        process?.WaitForExit();

        // Not working:
        // UnauthorizedAccessException: Access to the path 'C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\ServiceHub\Hosts\ServiceHub.Host.Extensibility.amd64\wkhtmltoimage.exe' is denied.
        // - https://github.com/Partiolainen/NetCoreHTMLToImage
        // - https://github.com/andrei-m-code/net-core-html-to-image
        // - https://github.com/lonelylty/HtmltoImage
        //   https://www.nuget.org/packages/HtmlToImage
        // - https://github.com/faisalcse1/HtmlToImageMaster
        //   https://www.nuget.org/packages/HtmlToImageMaster

        // WebView2 Notes:
        // - Thread must be STA for WebView2, so use separate thread
        // - EnsureCore without environment, leads to "EnsureCoreWebView2Async cannot be used before the application's event loop has started running"
        // - EnsureCore with environment, leads to WebView2Loader.ddl not found
    }

    private static async Task EnsureWkHtmlToImage(string tempFolder, CancellationToken cancellationToken)
    {
        var wkhtmltoimagePath = Path.Combine(tempFolder, "wkhtmltoimage.exe");

        if (File.Exists(wkhtmltoimagePath))
        {
            return;
        }

        using var resourceStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream($"MjmlVisualizer.Files.wkhtmltoimage.exe");
        using var fileStream = File.OpenWrite(wkhtmltoimagePath);

        if (resourceStream == null)
        {
            return;
        }

        await resourceStream
            .CopyToAsync(fileStream, cancellationToken)
            .ConfigureAwait(false);
    }

    private static string EnsureTempFolder()
    {
        var tempHtmlFolder = Path.Combine(Path.GetTempPath(), "MjmlVisualizer");
        Directory.CreateDirectory(tempHtmlFolder);
        return tempHtmlFolder;
    }
}
