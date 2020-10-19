using System.ComponentModel;
using System.Configuration.Install;

namespace WolfService
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : Installer
    {
        System.ServiceProcess.ServiceInstaller SystemServiceInstaller;
        System.ServiceProcess.ServiceProcessInstaller SystemProcessInstaller;

        public ServiceInstaller()
        {
            InitializeComponent();

            SystemServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            SystemProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();

            SystemProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;

            SystemServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            SystemServiceInstaller.ServiceName = "WolfUpdateService";

            Installers.Add(SystemServiceInstaller);
            Installers.Add(SystemProcessInstaller);
        }
    }
}