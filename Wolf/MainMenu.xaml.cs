using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Wolf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();

            ProfileName.Content = Properties.Settings.Default.Login;
        }

        #region MainMenu
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            CheatSection.Visibility = Visibility.Hidden;
            LibrarySection.Visibility = Visibility.Visible;
        }

        private void HackButton_Click(object sender, RoutedEventArgs e)
        {
            LibrarySection.Visibility = Visibility.Hidden;
            CheatSection.Visibility = Visibility.Visible;
        }
        #endregion

        #region Exit Label
        private void ExitMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Environment.Exit(0);

        private void ExitLabel_MouseLeave(object sender, MouseEventArgs e) => ExitLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        private void ExitLabel_MouseEnter(object sender, MouseEventArgs e) => ExitLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        #endregion

        #region Collapse Label
        private void CollapseMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => WindowState = WindowState.Minimized;
        #endregion
    }
}