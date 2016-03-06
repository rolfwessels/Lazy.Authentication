using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Owin;

[assembly: AssemblyTitle("Lazy.Authentication.Web")]
[assembly: AssemblyDescription("Contains all Lazy.Authentication unit tests")]

[assembly: OwinStartup(typeof(Lazy.Authentication.Api.Startup))]

[assembly: Guid("7ca4fd21-e47f-4a37-b83d-b457c7ed8d47")]
[assembly: Lazy.Authentication.Utilities.Helpers.Log4NetInitialize("Lazy.Authentication.Api")]
[assembly: log4net.Config.XmlConfigurator]

