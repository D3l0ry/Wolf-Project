using System.Windows;
using System.Windows.Controls;

using Wolf.Helper.User;

namespace Wolf.Store
{
    /// <summary>
    /// Логика взаимодействия для CheatSection.xaml
    /// </summary>
    public partial class CheatSection : UserControl
    {
        public CheatSection() => InitializeComponent();

        private void RunWolfHack_Click(object sender, RoutedEventArgs e) => WolfClient.DownloadFileAsync("Hack", "Wolf Project", "Wolf Hack", false,RunWolfHack, Helper.Enum.TypeFile.Hack,true,"WolfHack.exe", "UIEngine.dll");
    }
}