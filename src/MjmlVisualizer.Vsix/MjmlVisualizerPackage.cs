using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace MjmlVisualizer.Vsix
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class MjmlVisualizerPackage : AsyncPackage
    {
        public const string PackageGuidString = "ede82753-ab5b-4d93-b880-dacf3212fba5";

        public MjmlVisualizerPackage()
        {   
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                await base.InitializeAsync(cancellationToken, progress);

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

                // The Visualizer dll is in the same folder than the package because its project is
                // added as reference to this project, so it is included inside the .vsix file.
                // We only need to deploy it to the correct destination folder.
                var sourceFolderFullName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                // Get the destination folder for visualizers
                var shell = await GetServiceAsync(typeof(SVsShell)) as IVsShell;
                shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out var documentsFolderFullNameObject);
                var documentsFolderFullName = documentsFolderFullNameObject.ToString();
                var destinationFolderFullName = Path.Combine(documentsFolderFullName, "Visualizers");

                var sourceFileFullName = Path.Combine(sourceFolderFullName, "MjmlVisualizer.dll");
                var destinationFileFullName = Path.Combine(destinationFolderFullName, "MjmlVisualizer.dll");

                CopyFileIfNewerVersion(sourceFileFullName, destinationFileFullName);

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
                var sourceFileVersionInfo = FileVersionInfo.GetVersionInfo(sourceFileFullName);
                var destinationFileVersionInfo = FileVersionInfo.GetVersionInfo(destinationFileFullName);
                if (sourceFileVersionInfo.FileMajorPart > destinationFileVersionInfo.FileMajorPart)
                {
                    copy = true;
                }
                else if (sourceFileVersionInfo.FileMajorPart == destinationFileVersionInfo.FileMajorPart
                   && sourceFileVersionInfo.FileMinorPart > destinationFileVersionInfo.FileMinorPart)
                {
                    copy = true;
                }
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
    }
}
