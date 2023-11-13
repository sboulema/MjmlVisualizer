using System.Windows;
using System.Windows.Controls;

namespace MjmlVisualizer.Behaviors
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(BrowserBehavior),
            new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var text = e.NewValue as string;

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var webBrowser = dependencyObject as WebBrowser;
            webBrowser?.NavigateToString(text);
        }
    }
}
