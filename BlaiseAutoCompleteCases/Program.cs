using System.ServiceProcess;
using log4net;

namespace BlaiseAutoCompleteCases
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        ///         // Instantiate logger.
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            // Call the service class if run in debug mode so no need to install service for testing.
#if DEBUG
            Log.Info("Blaise Auto Complete Cases service starting in DEBUG mode");
            BlaiseAutoCompleteCases blaiseAutoCompleteCases = new BlaiseAutoCompleteCases();
            blaiseAutoCompleteCases.OnDebug();

#else
            Log.Info("Blaise Case Creator service starting in RELEASE mode.");
            ServiceBase[] ServicesToRun;
            var servicesToRun = new ServiceBase[]
            {
                new BlaiseAutoCompleteCases()
            };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
