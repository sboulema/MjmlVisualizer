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

                new Window
                {
                    Title = "MJML Visualizer",
                    MinWidth = 500,
                    MinHeight = 500,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new MjmlVisualizerUserControl { DataContext = viewModel },
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch
                }.ShowDialog();
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