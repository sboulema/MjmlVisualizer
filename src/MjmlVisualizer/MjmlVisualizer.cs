using Microsoft.VisualStudio.DebuggerVisualizers;
using MjmlVisualizer.Repositories;
using MjmlVisualizer.ViewModels;
using MjmlVisualizer.Windows;
using System;
using System.Windows;

[assembly: System.Diagnostics.DebuggerVisualizer(
   typeof(MjmlVisualizer.MjmlVisualizer),
   typeof(VisualizerObjectSource),
   Target = typeof(string),
   Description = "MJML Visualizer")]

namespace MjmlVisualizer
{
    public class MjmlVisualizer : DialogDebuggerVisualizer
    {
        public MjmlVisualizer() : base(FormatterPolicy.Json)
        { 
        }

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            {
                var debugObject = (objectProvider as IVisualizerObjectProvider3).GetDeserializableObject();

                var response = new MjmlRepository().GenerateHTML(debugObject.ToObject<string>());

                if (response == null)
                {
                    MessageBox.Show("String could not be converted");
                    return;
                }

                var viewModel = new MjmlVisualizerViewModel
                {
                    MJML = response.MJML.Trim(),
                    HTML = response.HTML.Trim()
                };

                new MjmlVisualizerWindow(viewModel).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting string: {ex.Message}");
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(MjmlVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}