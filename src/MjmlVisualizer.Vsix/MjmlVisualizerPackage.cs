using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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
    [Guid(PackageGuidString)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class MjmlVisualizerPackage : AsyncPackage
    {
        public const string PackageGuidString = "ede82753-ab5b-4d93-b880-dacf3212fba5";

        public MjmlVisualizerPackage()
        {   
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var fileNames = new List<string> { "MjmlVisualizer.dll", "Newtonsoft.Json.dll" };

            try
            {
                await base.InitializeAsync(cancellationToken, progress);
           
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

        private void CopyFileIfNewerVersion(string sourceFileFullName, string destinationFileFullName)
        {
            var copy = false;

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
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

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
