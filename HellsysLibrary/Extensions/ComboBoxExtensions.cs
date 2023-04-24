using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;

namespace HellsysLibrary.Extensions
{
    public static class ComboBoxExtensions
    {
        public static Border GetBorder(this ComboBox comboBox)
        {
            ToggleButton toggleButton = (ToggleButton)comboBox.Template?.FindName("toggleButton", comboBox);
            return (Border)toggleButton?.Template?.FindName("templateRoot", toggleButton);
        }

        public static void SetBorderBrush(this ComboBox comboBox, Brush brush)
        {
            Border border = comboBox.GetBorder();
            if (border != null)
                border.BorderBrush = brush;
        }

        public static void SetBackground(this ComboBox comboBox, Brush brush)
        {
            Border border = comboBox.GetBorder();
            if (border != null)
                border.Background = brush;

            TextBox textbox = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
            if (textbox != null)
            {
                Border parent = (Border)textbox.Parent;
                parent.Background = brush;
            }
        }
    }
}
