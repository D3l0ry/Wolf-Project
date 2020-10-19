using System.ServiceProcess;

namespace WolfService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WolfUpdateService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}