using System;
using System.IO;

namespace MjmlVisualizer.Test
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            var mjml = File.ReadAllText("test.mjml");

            MjmlVisualizer.TestShowVisualizer(mjml);
        }
    }
}
