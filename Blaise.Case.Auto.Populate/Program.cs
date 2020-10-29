using System.ServiceProcess;
using log4net;

namespace Blaise.Case.Auto.Populate
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
            Log.Info("Blaise Auto Populate Cases service starting in DEBUG mode");
            BlaiseCaseAutoPopulate blaiseCaseAutoPopulate = new BlaiseCaseAutoPopulate();
            blaiseCaseAutoPopulate.OnDebug();

#else
            Log.Info("Blaise Auto Populate Case service starting in RELEASE mode.");
            var servicesToRun = new ServiceBase[]
            {
                new BlaiseCaseAutoPopulate()
            };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
