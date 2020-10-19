using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Wolf.Helper.User;

namespace Wolf.Store
{
    /// <summary>
    /// Логика взаимодействия для LibrarySection.xaml
    /// </summary>
    public partial class LibrarySection : UserControl
    {
        public LibrarySection() => InitializeComponent();

        private void LibrarySection_Loaded(object sender, RoutedEventArgs e)
        {
            #region UIEngine
            OnlineVersionLabel.Content += WolfClient.GetOnlineFileInfo("Library", "Wolf Project", "UIEngine", "UIEngine.info")?["Version"];
            LocalVersionLabel.Content += WolfClient.GetLocalFileInfo("Library", "Wolf Project", "UIEngine", "UIEngine.info")?["Version"];
            #endregion
        }

        private void UIEngineDownloadButton_Click(object sender, RoutedEventArgs e) => WolfClient.DownloadFileAsync("Library", "Wolf Project", "UIEngine", Properties.Settings.Default.UsingFileDialog, UIEngineDownloadButton, Helper.Enum.TypeFile.Library,false, "UIEngine.dll");
    }
}