using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MjmlVisualizer.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace MjmlVisualizer.Vsix
{
    [Guid(PackageGuids.MjmlVisualizerString)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MjmlVisualizerToolWindow.Pane))]
    public sealed class MjmlVisualizerPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            this.RegisterToolWindows();

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await this.RegisterCommandsAsync();

            await this.RegisterDebugVisualizers(cancellationToken);
        }

        private async Task RegisterDebugVisualizers(CancellationToken cancellationToken)
        {
            var fileNames = new List<string> { "MjmlVisualizer.dll", "Newtonsoft.Json.dll" };

            try
            {
                // The Visualizer dll is in the same folder than the package because its project is
                // added as reference to this project, so it is included inside the .vsix file.
                // We only need to deploy it to the correct destination folder.
                var sourceFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                var destinationFolderPath = await GetVisualizersDirectory(cancellationToken);

                foreach (var fileName in fileNames)
                {
                    var sourceFileFullName = Path.Combine(sourceFolderPath, fileName);
                    var destinationFileFullName = Path.Combine(destinationFolderPath, fileName);

                    CopyFileIfNewerVersion(sourceFileFullName, destinationFileFullName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private static void CopyFileIfNewerVersion(string sourceFileFullName, string destinationFileFullName)
        {
            bool copy;

            if (File.Exists(destinationFileFullName))
            {
                var sourceVersion = GetVersion(sourceFileFullName);
                var destinationVersion = GetVersion(destinationFileFullName);

                copy = sourceVersion > destinationVersion;
            }
            else
            {
                // First time
                copy = true;
            }

            if (copy)
            {
                File.Copy(sourceFileFullName, destinationFileFullName, true);
            }
        }

        private static Version GetVersion(string path)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(path);
            return new Version(fileVersionInfo.FileVersion);
        }

        private async Task<string> GetVisualizersDirectory(CancellationToken cancellationToken)
        {
            if (!(await GetServiceAsync(typeof(SVsShell)) is IVsShell shell))
            {
                return string.Empty;
            }

            // Get the destination folder for visualizers
            shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out var documentsFolderFullNameObject);

            return Path.Combine(documentsFolderFullNameObject.ToString(), "Visualizers");
        }
    }
}
