using IWshRuntimeLibrary;

using System;
using System.Configuration.Install;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Wolf_Installer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MenuInstaller : Window
    {
        public MenuInstaller() => InitializeComponent();

        private void DownloadButton_Click(object sender, RoutedEventArgs e) => DownloadFiles();

        private async void DownloadFiles()
        {
            try
            {
                DownloadButton.IsEnabled = false;
                DownloadButton.Content = "Устанавливаю";

                string Url = "http://wolf.wolfproject.ru/Installer/";
                string[] Files = { "Wolf.exe", "UIEngine.dll", "WolfService.exe" };

                string Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + "Wolf Project" + @"\" + Files[0];
                string PathDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + "Wolf Project";

                using WebClient WebClient = new WebClient();

                if (!Directory.Exists(PathDirectory))
                {
                    Directory.CreateDirectory(PathDirectory);
                }

                foreach (string File in Files)
                {
                    LabelProgress.Content = $"Идет загрузка: {File}";

                    await WebClient.DownloadFileTaskAsync(Url + File, PathDirectory + @"\" + File);
                }

                WshShell WshShell = new WshShell();
                IWshShortcut IWshShortcut = (IWshShortcut)WshShell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + "Wolf.lnk");

                IWshShortcut.TargetPath = Path;
                IWshShortcut.WorkingDirectory = PathDirectory;
                IWshShortcut.IconLocation = Path;
                IWshShortcut.Save();

            InstallService:
                try
                {
                    ManagedInstallerClass.InstallHelper(new[] { @$"{PathDirectory}\WolfService.exe" });
                }
                catch
                {
                    ManagedInstallerClass.InstallHelper(new[] { "/u", @$"{PathDirectory}\WolfService.exe" });

                    goto InstallService;
                }

                LabelProgress.Content = "Загрузка завершена!";
                LabelSleep.Content = "Я отдохну секундочку)";

                await Task.Delay(1000);

                LabelSleep.Content = "Выпускаю волка!";
                LabelProgress.Content = "Я отдохнул!";

                Environment.Exit(0);
            }
            catch (Exception EX)
            {
                if (MessageBox.Show(EX.Message + "\nПопробуйте повторить установку позже!", "Ошибка установки", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}