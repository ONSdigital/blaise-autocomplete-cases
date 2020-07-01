using System;
using System.Configuration;
using System.ServiceProcess;
using Blaise.Nuget.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.PubSub.Api;
using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Interfaces.Mappers;
using BlaiseCaseAutoComplete.Interfaces.Providers;
using BlaiseCaseAutoComplete.Interfaces.Services;
using BlaiseCaseAutoComplete.Mappers;
using BlaiseCaseAutoComplete.MessageHandler;
using BlaiseCaseAutoComplete.Providers;
using BlaiseCaseAutoComplete.Services;
using log4net;
using Unity;

namespace BlaiseCaseAutoComplete
{
    public partial class BlaiseCaseAutoComplete : ServiceBase
    {
        #region Variables

        // Instantiate logger.
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IInitialiseService _caseAutoCompleteService;

        #endregion

        /// <summary>
        /// Class constructor for initializing the service.
        /// </summary>    
        public BlaiseCaseAutoComplete()
        {
            InitializeComponent();
            IUnityContainer unityContainer = new UnityContainer();

            //providers
            unityContainer.RegisterType<IConfigurationProvider, ConfigurationProvider>();

            unityContainer.RegisterSingleton<IFluentQueueApi, FluentQueueApi>();
            unityContainer.RegisterFactory<ILog>(f => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));

            //mappers
            unityContainer.RegisterType<IModelMapper, ModelMapper>();

            //handlers
            unityContainer.RegisterType<IMessageHandler, AutoCompleteCaseHandler>();

            //services   
            unityContainer.RegisterType<ICompleteCaseService, CompleteCaseService>();
            unityContainer.RegisterType<IAutoCompleteCasesService, AutoCompleteCasesService>();

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

#endif

            //resolve all dependencies as CaseCreationService is the entry point
            _caseAutoCompleteService = unityContainer.Resolve<IInitialiseService>();
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
            Log.Info("Blaise Auto Complete Cases service started.");
            _caseAutoCompleteService.Start();
        }

        protected override void OnStop()
        {
            _caseAutoCompleteService.Stop();
            Log.Info("Blaise Auto Complete Cases service stopped.");
        }
    }
}
