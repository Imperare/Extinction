using ProgExtinction.Helpers;
using ProgExtinction.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgExtinction.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            timer.Elapsed += new ElapsedEventHandler(MiseAJourTitle);
            timer.Interval = 1000;
            timer.Enabled = true;

            MiseAJourTitle(null, null);
        }

        private void MiseAJourTitle(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => Title = DateTime.Now.ToString("HH:mm:ss")));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Sélection sur focus
            TextBoxHeures.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(ComponentsHelper.ToutSelectionnerSurFocus));
            TextBoxMinutes.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(ComponentsHelper.ToutSelectionnerSurFocus));
            TextBoxSecondes.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(ComponentsHelper.ToutSelectionnerSurFocus));

            // Focus sans sélection
            TextBoxHeures.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ComponentsHelper.FocusSansClic));
            TextBoxMinutes.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ComponentsHelper.FocusSansClic));
            TextBoxSecondes.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ComponentsHelper.FocusSansClic));
        }

        private void TextBoxHeures_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Si Entrer est pressé, on valide la date
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                TextBoxMinutes.Focus();
            }
            else if (e.Key == Key.Delete)
            {
                TextBoxHeures.Text = "0";
            }
            else if (e.Key == Key.Right && TextBoxHeures.CaretIndex == TextBoxHeures.Text.Length)
            {
                TextBoxMinutes.Focus();
                TextBoxMinutes.CaretIndex = 0;
            }
            else if (e.Key == Key.Up)
            {
                TextBoxHeures.Text = UpDown(TextBoxHeures.Text, 1, 23);
            }
            else if (e.Key == Key.Down)
            {
                TextBoxHeures.Text = UpDown(TextBoxHeures.Text, -1, 23);
            }
        }

        private void TextBoxMinutes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Si Entrer est pressé, on valide la date
            if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Tab)
            {
                buttonProgrammerExtinction.Focus();
            }
            else if (e.Key == Key.Delete)
            {
                TextBoxMinutes.Text = "0";
            }
            else if (e.Key == Key.Left && TextBoxMinutes.CaretIndex == 0)
            {
                TextBoxHeures.Focus();
                TextBoxHeures.CaretIndex = TextBoxHeures.Text.Length;
            }
            else if (e.Key == Key.Right && TextBoxMinutes.CaretIndex == TextBoxMinutes.Text.Length)
            {
                TextBoxSecondes.Focus();
                TextBoxSecondes.CaretIndex = TextBoxSecondes.Text.Length;
            }
            else if (e.Key == Key.Up)
            {
                TextBoxMinutes.Text = UpDown(TextBoxMinutes.Text, 1, 59);
            }
            else if (e.Key == Key.Down)
            {
                TextBoxMinutes.Text = UpDown(TextBoxMinutes.Text, -1, 59);
            }
        }

        private void TextBoxSecondes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Si Entrer est pressé, on valide la date
            if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Tab)
            {
                buttonProgrammerExtinction.Focus();
            }
            else if (e.Key == Key.Delete)
            {
                TextBoxSecondes.Text = "0";
            }
            else if (e.Key == Key.Left && TextBoxSecondes.CaretIndex == 0)
            {
                TextBoxMinutes.Focus();
                TextBoxMinutes.CaretIndex = TextBoxMinutes.Text.Length;
            }
            else if (e.Key == Key.Up)
            {
                TextBoxSecondes.Text = UpDown(TextBoxSecondes.Text, 1, 59);
            }
            else if (e.Key == Key.Down)
            {
                TextBoxSecondes.Text = UpDown(TextBoxSecondes.Text, -1, 59);
            }
        }

        private string UpDown(string valeur, int variation, int max)
        {
            return Math.Max(0, Math.Min(Convert.ToInt32(valeur) + variation, max)).ToString();
        }

        /// <summary>
        /// Evenement TextInput sur la textbox des minutes
        /// </summary>
        private void TextBoxHeures_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Suppression du texte sélectionné
            string texte;
            var textBox = (sender as TextBox);
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);

            texte = textBox.Text == "0" ? string.Empty : textBox.Text;

            // On regarde si le texte prévisionnel est compatible avec le format des heures
            // Le texte prévisionnel est composé selon la position du curseur dans la Textbox (CaretIndex)
            string textePrevisionnel = textBox.CaretIndex == 1 ? texte + e.Text : e.Text + texte;
            e.Handled = !Regex.IsMatch(textePrevisionnel, "^[0-9]$|^[0-1][0-9]$|^2[0-3]$"); // Autorise entre 0 et 9 | 00 et 19 OU entre 20 et 23
        }

        /// <summary>
        /// Changement automatique du focus des heures aux minutes
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // S'il n'y a aucune valeur, on considère que c'est 0 heure
            if (string.IsNullOrEmpty(TextBoxHeures.Text))
            {
                TextBoxHeures.Text = "0";
                TextBoxHeures.CaretIndex = 1;
            }

            // S'il n'y a aucune valeur, on considère que c'est 0 minute
            if (string.IsNullOrEmpty(TextBoxMinutes.Text))
            {
                TextBoxMinutes.Text = "0";
                TextBoxHeures.CaretIndex = 1;
            }

            // S'il n'y a aucune valeur, on considère que c'est 0 minute
            if (string.IsNullOrEmpty(TextBoxSecondes.Text))
            {
                TextBoxSecondes.Text = "0";
                TextBoxSecondes.CaretIndex = 1;
            }
        }

        /// <summary>
        /// Evenement TextInput sur la textbox des minutes
        /// </summary>
        private void TextBox59_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Suppression du texte sélectionné
            var textBox = (sender as TextBox);
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);

            // On regarde si le texte prévisionnel est compatible avec le format des minutes
            // Le texte prévisionnel est composé selon la position du curseur dans la Textbox (CaretIndex)
            string textePrevisionnel = textBox.CaretIndex == 1 ? textBox.Text + e.Text : e.Text + textBox.Text;
            e.Handled = !Regex.IsMatch(textePrevisionnel, "^[0-9]$|^[0-5][0-9]$"); // Autorise entre 00 et 59
        }

        private void TextBoxHeures_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBoxHeures.Text = UpDown(TextBoxHeures.Text, e.Delta / 120, 23);
            CommandManager.InvalidateRequerySuggested();
        }

        private void TextBoxMinutes_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBoxMinutes.Text = UpDown(TextBoxMinutes.Text, e.Delta / 120, 59);
            CommandManager.InvalidateRequerySuggested();
        }

        private void TextBoxSecondes_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBoxSecondes.Text = UpDown(TextBoxSecondes.Text, e.Delta / 120, 59);
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
