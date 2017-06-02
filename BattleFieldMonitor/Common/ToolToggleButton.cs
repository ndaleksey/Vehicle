using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Swsu.BattleFieldMonitor.Common
{
    public class ToolToggleButton : ToggleButton
    {
        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(nameof(ImageTemplate),
            typeof(ControlTemplate), typeof(ToolToggleButton), new PropertyMetadata(null));

        public ControlTemplate ImageTemplate
        {
            get { return (ControlTemplate)GetValue(ImageTemplateProperty); }
            set { SetValue(ImageTemplateProperty, value); }
        }
    }
}
