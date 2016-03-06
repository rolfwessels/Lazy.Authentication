using FizzWare.NBuilder;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;
using NUnit.Framework;

namespace Lazy.Authentication.Core.Tests.Managers
{
    [TestFixture]
    public class ApplicationManagerTests : BaseTypedManagerTests<Application>
    {
        private ApplicationManager _userManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _userManager = new ApplicationManager(_baseManagerArguments);
        }

        #endregion

        protected override IRepository<Application> Repository
        {
            get { return _fakeGeneralUnitOfWork.Applications; }
        }

        protected override Application SampleObject
        {
            get { return Builder<Application>.CreateNew().Build(); }
        }

        protected override BaseManager<Application> Manager
        {
            get { return _userManager; }
        }
    }
}