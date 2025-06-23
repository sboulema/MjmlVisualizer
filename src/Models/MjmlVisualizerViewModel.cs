using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using Mjml.Net;
using MjmlVisualizer.Converters;
using MjmlVisualizer.Resources;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace MjmlVisualizer.Models;

[DataContract]
public class MjmlVisualizerViewModel : NotifyPropertyChangedObject
{
    private readonly VisualStudioExtensibility _extensibility;

    public MjmlVisualizerViewModel(VisualStudioExtensibility extensibility)
    {
        _extensibility = extensibility;

        SaveMJMLCommand = new AsyncCommand(SaveMJML);
        SaveHTMLCommand = new AsyncCommand(SaveHTML);
        SaveScreenshotCommand = new AsyncCommand(SaveScreenshot);
    }

    private string _mjml = string.Empty;

    [DataMember]
    public string MJML
    {
        get => _mjml;
        set
        {
            SetProperty(ref _mjml, value);

            var result = new MjmlRenderer().Render(_mjml, new()
            {
                Beautify = true,
            });

            HTML = result.Html;

            TempHTMLPath = SaveHTMLToFile(HTML);
        }
    }

    private string _html = string.Empty;

    [DataMember]
    public string HTML
    { 
        get => _html;
        set => SetProperty(ref _html, value);
    }

    private double _width = 1024;

    [DataMember]
    public double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    private string _tempHtmlPath = string.Empty;

    [DataMember]
    public string TempHTMLPath
    {
        get => _tempHtmlPath;
        set => SetProperty(ref _tempHtmlPath, value);
    }

    #region Commands
    [DataMember]
    public IAsyncCommand SaveMJMLCommand
    {
        get;
    }

    [DataMember]
    public IAsyncCommand SaveHTMLCommand
    {
        get;
    }

    [DataMember]
    public IAsyncCommand SaveScreenshotCommand
    {
        get;
    }

    private async Task SaveMJML(object? commandParameter, CancellationToken cancellationToken)
    {
        var filename = await _extensibility.Shell()
            .ShowSaveAsFileDialogAsync(new()
            {
                Filters = new([ new("MJML Template", "*.mjml") ]),
                Title = "Save a MJML Template"
            }, cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(filename))
        {
            return;
        }

        File.WriteAllText(filename, MJML);
    }

    private async Task SaveHTML(object? commandParameter, CancellationToken cancellationToken)
    {
        var filename = await _extensibility.Shell()
            .ShowSaveAsFileDialogAsync(new()
            {
                Filters = new([ new("HTML Webpage", "*.html") ]),
                Title = "Save a HTML Webpage"
            }, cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(filename))
        {
            return;
        }

        File.WriteAllText(filename, HTML);
    }

    private async Task SaveScreenshot(object? commandParameter, CancellationToken cancellationToken)
    {
        var filename = await _extensibility.Shell()
            .ShowSaveAsFileDialogAsync(new()
            {
                Filters = new([new("JPEG Image", "*.jpg")]),
                Title = "Save a JPEG Image"
            }, cancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrEmpty(filename))
        {
            return;
        }

        await HtmlConverter
            .FromPath(TempHTMLPath, filename, cancellationToken)
            .ConfigureAwait(false);
    }
    #endregion

    #region Labels
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    [DataMember]
    public string TabItemHeaderMJML => MjmlVisualizerStrings.TabItemHeaderMJML;

    [DataMember]
    public string TabItemHeaderHTML => MjmlVisualizerStrings.TabItemHeaderHTML;

    [DataMember]
    public string TabItemHeaderPreview => MjmlVisualizerStrings.TabItemHeaderPreview;

    [DataMember]
    public string ButtonLabelSave => MjmlVisualizerStrings.ButtonLabelSave;

    [DataMember]
    public string CheckBoxLabelWordWrap => MjmlVisualizerStrings.CheckBoxLabelWordWrap;
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    #endregion

    private static string SaveHTMLToFile(string html)
    {
        var tempHtmlFolder = Path.Combine(Path.GetTempPath(), "MjmlVisualizer");
        Directory.CreateDirectory(tempHtmlFolder);
        var tempHtmlFilePath = Path.Combine(tempHtmlFolder, "index.html");

        File.WriteAllText(tempHtmlFilePath, html);

        return tempHtmlFilePath;
    }
}
