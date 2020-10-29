using System;
using System.Configuration;
using System.ServiceProcess;
using Blaise.Case.Auto.Populate.Interfaces.Mappers;
using Blaise.Case.Auto.Populate.MessageHandler;
using Blaise.Case.Auto.Populate.Interfaces.Providers;
using Blaise.Case.Auto.Populate.Interfaces.Services;
using Blaise.Case.Auto.Populate.Mappers;
using Blaise.Case.Auto.Populate.Providers;
using Blaise.Case.Auto.Populate.Services;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.PubSub.Api;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using log4net;
using Unity;

namespace Blaise.Case.Auto.Populate
{
    public partial class BlaiseCaseAutoPopulate : ServiceBase
    {
        #region Variables

        // Instantiate logger.
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IInitialiseService _initialiseService;

        #endregion

        /// <summary>
        /// Class constructor for initializing the service.
        /// </summary>    
        public BlaiseCaseAutoPopulate()
        {
            InitializeComponent();
            IUnityContainer unityContainer = new UnityContainer();
            
            unityContainer.RegisterSingleton<IFluentQueueApi, FluentQueueApi>();
            unityContainer.RegisterFactory<ILog>(f => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));

            //mappers
            unityContainer.RegisterType<IModelMapper, ModelMapper>();

            //handlers
            unityContainer.RegisterType<IMessageHandler, PopulateCaseMessageHandler>();

            //services   
            unityContainer.RegisterType<IPopulateCaseService, PopulateCaseService>();
            unityContainer.RegisterType<IPopulateCasesService, PopulateCasesService>();

            //queue service
            unityContainer.RegisterType<IQueueService, QueueService>();

            //blaise services
            unityContainer.RegisterType<IFluentBlaiseApi, FluentBlaiseApi>();

            //main service
            unityContainer.RegisterType<IInitialiseService, InitialiseService>();

            //allow access to PubSub whilst in debug mode

#if DEBUG
            var credentialKey = ConfigurationManager.AppSettings["GOOGLE_APPLICATION_CREDENTIALS"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialKey);

            unityContainer.RegisterType<IConfigurationProvider, LocalConfigurationProvider>();
#else
            unityContainer.RegisterType<IConfigurationProvider, ConfigurationProvider>();
#endif

            //resolve all dependencies as CaseCreationService is the entry point
            _initialiseService = unityContainer.Resolve<IInitialiseService>();
        }

        /// <summary>
        /// This method is our entry point when debugging. It allows us to use the service
        /// without running the installation steps.
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Blaise Auto Populate Cases service started.");
            _initialiseService.Start();
        }

        protected override void OnStop()
        {
            _initialiseService.Stop();
            Log.Info("Blaise Auto Populate Cases service stopped.");
        }
    }
}
