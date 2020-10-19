using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace WolfService
{
    partial class WolfUpdateService : ServiceBase
    {
        private static Thread Thread;

        public WolfUpdateService() => InitializeComponent();

        protected override void OnStart(string[] args)
        {
            Thread = new Thread(() => GetUpdate());
            Thread.Start();

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Thread.Abort();
            Thread = null;

            base.OnStop();
        }

        private static void GetUpdate()
        {
            while (true)
            {
                try
                {
                    if (!CheckVersionEquals(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + "Wolf Project" + @"\" + "Wolf.exe"))
                    {
                        Process[] ProcessWolf = Process.GetProcessesByName("Wolf");

                        string Url = "http://wolf.wolfproject.ru/Installer/";
                        string[] Files = { "Wolf.exe", "UIEngine.dll" };

                        string Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + "Wolf Project" + @"\" + Files[0];
                        string PathDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + "Wolf Project";

                        using WebClient WebClient = new WebClient();

                        if (!Directory.Exists(PathDirectory))
                        {
                            Directory.CreateDirectory(PathDirectory);
                        }

                        foreach (string File in Files)
                        {
                            WebClient.DownloadFile(Url + File, PathDirectory + @"\" + File);
                        }

                        Thread.Sleep(10_800_000);
                    }
                    else
                    {
                        Thread.Sleep(10_800_000);
                    }
                }
                catch
                {
                    Thread.Sleep(10_800_000);
                }
            }
        }

        public static bool CheckVersionEquals(string Path)
        {
            if (File.Exists(Path))
            {
                if (FileVersionInfo.GetVersionInfo(Path).FileVersion == new WebClient().DownloadString("http://wolf.wolfproject.ru/Installer/Version/Wolf.info"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}