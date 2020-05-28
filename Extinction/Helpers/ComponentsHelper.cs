using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgExtinction.Helpers
{
    public static class ComponentsHelper
    {
        public static void ToutSelectionnerSurFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.SelectedText != textBox.Text)
            {
                textBox.SelectAll();
            }
        }

        public static void FocusSansClic(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !textBox.IsFocused && !textBox.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                textBox.Focus();
            }
        }
    }
}
