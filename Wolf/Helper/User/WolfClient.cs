using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

using UIEngine.Helper.Define.Variable;
using UIEngine.Helper.Site;
using UIEngine.Helper.Site.User;
using UIEngine.Memory.Helper;
using Wolf.Helper.Enum;
using ButtonWpf = System.Windows.Controls.Button;

namespace Wolf.Helper.User
{
    public struct WolfClient
    {
        #region Properties
        public static string Login { get; private set; }

        public static string Password { get; private set; }

        public static string KeyAccess { get; private set; }
        #endregion

        private WolfClient(string CLogin, string CPassword, string CAccessKey)
        {
            Login = CLogin;
            Password = CPassword;
            KeyAccess = CAccessKey;
        }

        public static BOOLEAN ClientAuthorize(string Url, string Login, string Password)
        {
            string GetRespone = new Request(Url)
            {
                ["login"] = Login,
                ["password"] = Password,
                ["hwid"] = Client.GetComputerID().GetMD5()
            }.GetRespone();

            if (GetRespone.Split('|')[0] == "True")
            {
                new WolfClient(Login, Password, GetRespone.Split('|')[1]);

                return true;
            }

            return false;
        }

        #region Download File
        public static async void DownloadFileAsync(string TypeFile, string Company, string SubFolder, BOOLEAN FileDialog, ButtonWpf ButtonWpf, TypeFile TypeFileEnum, bool Run, params string[] FileName)
        {
            Request Request = new Request("http://wolf.wolfproject.ru/UIUser/Download/")
            {
                ["login"] = Login,
                ["password"] = Password,
                ["keyaccess"] = KeyAccess,
                ["typefile"] = TypeFile,
                ["company"] = Company,
                ["subfolder"] = SubFolder
            };

            string GetRespone = await Request.GetResponeAsync();
            string Path(string GetFile) => TypeFile + "/" + Company + "/" + SubFolder + "/" + GetFile;
            string PathFileInfo(string GetFile) => TypeFile + "/" + Company + "/" + SubFolder + "/" + GetFile + ".info";
            string PathDirectory = TypeFile + "/" + Company + "/" + SubFolder;

            using WebClient WebClient = new WebClient();

            if (GetRespone != "False")
            {
                if (!Directory.Exists(PathDirectory))
                {
                    Directory.CreateDirectory(PathDirectory);
                }

                ButtonWpf.Content = "Загружается";
                ButtonWpf.IsEnabled = false;

                DownloadFileInfo(GetRespone, SubFolder, PathFileInfo(SubFolder), WebClient);

                switch (TypeFileEnum)
                {
                    case Enum.TypeFile.Library:
                        if (FileDialog)
                        {
                            using (SaveFileDialog SaveFile = new SaveFileDialog
                            {
                                Title = "Сохранить файл",
                                FileName = FileName[0],
                                Filter = "Все файлы (*.*) | *.*"
                            })
                            {
                                if (SaveFile.ShowDialog() == DialogResult.OK)
                                {
                                    await WebClient.DownloadFileTaskAsync(new Uri(GetRespone + "/" + FileName[0]), SaveFile.FileName);
                                }
                            }
                        }
                        else
                        {
                            await WebClient.DownloadFileTaskAsync(new Uri(GetRespone + "/" + FileName[0]), Path(FileName[0]));

                            if (Run)
                            {
                                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + PathDirectory);
                            }
                        }

                        ButtonWpf.Content = "Скачать";
                        ButtonWpf.IsEnabled = true;
                        break;

                    case Enum.TypeFile.Hack:
                        if (!HMemory.IsProcessActive(FileName[0]))
                        {
                            foreach (var GetFile in FileName)
                            {
                                await WebClient.DownloadFileTaskAsync(new Uri(GetRespone + "/" + GetFile), Path(GetFile));
                            }

                            Process.Start(Path(FileName[0]));

                        }
                        else
                        {
                            MessageBox.Show("Процесс уже запущен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        ButtonWpf.Content = "Запустить";
                        ButtonWpf.IsEnabled = true;
                        break;
                }
            }
        }
        #endregion

        #region Get Local File Info
        public static Dictionary<string, string> GetOnlineFileInfo(string TypeFile, string Company, string SubFolder, string FileName)
        {
            Dictionary<string, string> GetFileInfoDictionary = new Dictionary<string, string>();

            Request Request = new Request("http://wolf.wolfproject.ru/UIUser/Download/")
            {
                ["login"] = Login,
                ["password"] = Password,
                ["keyaccess"] = KeyAccess,
                ["typefile"] = TypeFile,
                ["company"] = Company,
                ["subfolder"] = SubFolder,
                ["filename"] = FileName
            };
            string GetRespone = Request.GetRespone();

            using WebClient WebClient = new WebClient();

            if (GetRespone != "False")
            {
                string FileInfo = WebClient.DownloadString(GetRespone);

                foreach (var GetInfo in FileInfo.Split('\n'))
                {
                    GetFileInfoDictionary.Add(GetInfo.Split(':')[0].TrimEnd(' '), GetInfo.Split(':')[1]);
                }

                return GetFileInfoDictionary;
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<string, string> GetLocalFileInfo(string TypeFile, string Company, string SubFolder, string FileName)
        {
            Dictionary<string, string> GetFileInfoDictionary = new Dictionary<string, string>();

            string Path = TypeFile + "/" + Company + "/" + SubFolder + "/" + FileName;

            if (File.Exists(Path))
            {
                using StreamReader StreamReader = new StreamReader(Path);

                string FileInfo = StreamReader.ReadToEnd();

                foreach (var GetInfo in FileInfo.Split('\n'))
                {
                    GetFileInfoDictionary.Add(GetInfo.Split(':')[0], GetInfo.Split(':')[1]);
                }

                return GetFileInfoDictionary;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region File Exists
        public static BOOLEAN IsExistsFile(string TypeFile, string Company, string SubFolder, string FileName, ButtonWpf ButtonWpf) => File.Exists(TypeFile + "/" + Company + "/" + SubFolder + "/" + FileName) ? true : false;
        #endregion

        #region Check Update
        public static void CheckUpdate(string TypeFile, string Company, string SubFolder, string FileName, ButtonWpf ButtonWpf, string ButtonText = "Скачать")
        {
            string Path = TypeFile + "/" + Company + "/" + SubFolder + "/" + FileName;

            if (File.Exists(Path))
            {
                string OnlineVersion = GetOnlineFileInfo(TypeFile, Company, SubFolder, FileName)?["Version"];
                string LocalVersion = GetLocalFileInfo(TypeFile, Company, SubFolder, FileName)?["Version"];

                bool LocalVersionEquals = LocalVersion?.Equals(OnlineVersion) ?? true;

                if (LocalVersionEquals)
                {
                    ButtonWpf.Content = ButtonText;
                }
                else
                {
                    ButtonWpf.Content = "Обновить";
                }
            }
        }

        public static BOOLEAN CheckUpdate(string TypeFile, string Company, string SubFolder, string FileName)
        {
            string Path = TypeFile + "/" + Company + "/" + SubFolder + "/" + FileName;

            if (File.Exists(Path))
            {
                string OnlineVersion = GetOnlineFileInfo(TypeFile, Company, SubFolder, FileName)?["Version"];
                string LocalVersion = GetLocalFileInfo(TypeFile, Company, SubFolder, FileName)?["Version"];

                return LocalVersion?.Equals(OnlineVersion) ?? true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        private static void DownloadFileInfo(string GetRespone, string FileName, string Path, WebClient WebClient) => WebClient.DownloadFile(new Uri(GetRespone + "/" + FileName + ".info"), Path.Split('.')[0] + ".info");
        #endregion
    }
}