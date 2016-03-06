using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using GoogleAnalyticsTracker.WebAPI2;
using Lazy.Authentication.Api.Common;
using Lazy.Authentication.Api.Database;
using Lazy.Authentication.Api.Properties;
using Lazy.Authentication.Api.SignalR.Connection;
using Lazy.Authentication.Api.SignalR.Hubs;
using Lazy.Authentication.Api.WebApi.Controllers;
using Lazy.Authentication.Core.Startup;
using Lazy.Authentication.Dal.Persistance;

namespace Lazy.Authentication.Api.AppStartup
{
	public class IocApi : IocCoreBase
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocApi _instance;
		private readonly IContainer _container;

	    public IocApi()
		{
		    
	        var builder = new ContainerBuilder();
			SetupCore(builder);
			SetupSignalr(builder);
            SetupCommonControllers(builder);
		    SetupTools(builder);
			WebApi(builder);
			SignalRHubs(builder);
			_container = builder.Build();
		    
		}


	    private void SetupSignalr(ContainerBuilder builder)
		{
			builder.RegisterType<ConnectionStateMapping>().As<IConnectionStateMapping>().SingleInstance();
		}

		private void SetupCommonControllers(ContainerBuilder builder)
		{
			builder.RegisterType<UserCommonController>();
			builder.RegisterType<ProjectCommonController>();
		}

        private void SetupTools(ContainerBuilder builder)
		{
            builder.Register(t => new Tracker(Settings.Default.GoogleAnyliticsId, Settings.Default.GoogleAnyliticsDomain));
		}

		#region Instance

		public static IocApi Instance
		{
			get
			{
				if (_isInitialized) return _instance;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_instance = new IocApi();
						_isInitialized = true;
					}
				}
				return _instance;
			}
		}

		public IContainer Container
		{
			get { return _container; }
		}


		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		#endregion

		#region Overrides of IocCoreBase

        protected override IGeneralUnitOfWorkFactory GetInstanceOfIGeneralUnitOfWorkFactory(IComponentContext arg)
		{
            return new GeneralUnitOfWorkFactory();
		}

		#endregion

		#region Private Methods

		private void WebApi(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(typeof(UserController).Assembly);
			builder.Register(t => new AutofacWebApiDependencyResolver(_container)).As<IDependencyResolver>();
		}

		private void SignalRHubs(ContainerBuilder builder)
		{
            builder.RegisterHubs(typeof(NotificationHub).Assembly);
			builder.Register(t => new AutofacDependencyResolver(_container)).As<Microsoft.AspNet.SignalR.IDependencyResolver>();
		}

		#endregion
	}
}