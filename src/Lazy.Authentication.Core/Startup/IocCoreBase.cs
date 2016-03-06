using Autofac;
using FluentValidation;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Core.MessageUtil;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;
using Lazy.Authentication.Dal.Validation;
using Lazy.Authentication.OAuth2.Dal.Interfaces;
using IValidatorFactory = Lazy.Authentication.Dal.Validation.IValidatorFactory;

namespace Lazy.Authentication.Core.Startup
{
	public abstract class IocCoreBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
            SetupMongoDb(builder);
		    SetupManagers(builder);
			SetupTools(builder);
            SetupValidation(builder);
		}

	    protected virtual void SetupMongoDb(ContainerBuilder builder)
	    {
	        builder.Register(GetInstanceOfIGeneralUnitOfWorkFactory).SingleInstance();
	        builder.Register(x => x.Resolve<IGeneralUnitOfWorkFactory>().GetConnection());
	    }

	    private static void SetupManagers(ContainerBuilder builder)
		{
            builder.RegisterType<BaseManagerArguments>();
            builder.RegisterType<ApplicationManager>().As<IApplicationManager>();
            builder.RegisterType<OAuthDataManager>().As<IOAuthDataManager>();
            builder.RegisterType<ProjectManager>().As<IProjectManager>();
            builder.RegisterType<RoleManager>().As<IRoleManager>();
            builder.RegisterType<UserManager>().As<IUserManager>();
		}

	    private static void SetupValidation(ContainerBuilder builder)
	    {
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>();
	        builder.RegisterType<UserValidator>().As<IValidator<User>>();
	        builder.RegisterType<ProjectValidator>().As<IValidator<Project>>();
	        builder.RegisterType<UserValidator>().As<IValidator<User>>();
	    }

	    private void SetupTools(ContainerBuilder builder)
		{
			builder.Register((x) => Messenger.Default).As<IMessenger>();
		}

        protected abstract IGeneralUnitOfWorkFactory GetInstanceOfIGeneralUnitOfWorkFactory(IComponentContext arg);
	}
}
