using System.Windows;
using System.Windows.Controls;

namespace Swsu.BattleFieldMonitor.Common
{
    public class ToolButton : Button
    {
        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(nameof(ImageTemplate),
            typeof(ControlTemplate), typeof(ToolButton), new PropertyMetadata(null));

        public ControlTemplate ImageTemplate
        {
            get { return (ControlTemplate)GetValue(ImageTemplateProperty); }
            set { SetValue(ImageTemplateProperty, value); }
        }
    }
}