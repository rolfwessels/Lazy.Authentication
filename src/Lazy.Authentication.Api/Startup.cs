using System.Web.Http.Dependencies;
using Lazy.Authentication.Api.AppStartup;
using Lazy.Authentication.Api.Models.Mappers;
using Lazy.Authentication.Api.SignalR;
using Lazy.Authentication.Api.Swagger;
using Lazy.Authentication.Api.WebApi;
using Owin;
using log4net.Config;

namespace Lazy.Authentication.Api
{
	public class Startup
	{
	    
	    public void Configuration(IAppBuilder appBuilder)
		{
		    XmlConfigurator.Configure();
	        CrossOrginSetup.UseCors(appBuilder);
			BootStrap.Initialize(appBuilder);
		    MapApi.Initialize();
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder, IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
			SwaggerSetup.Initialize(webApiSetup.Configuration);
            webApiSetup.Configuration.EnsureInitialized();
		}
	}
}