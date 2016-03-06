using FizzWare.NBuilder;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;
using NUnit.Framework;

namespace Lazy.Authentication.Core.Tests.Managers
{
    [TestFixture]
    public class ProjectManagerTests : BaseTypedManagerTests<Project>
    {
        private ProjectManager _projectManager;

        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            _projectManager = new ProjectManager(_baseManagerArguments);
        }

        #endregion

        protected override IRepository<Project> Repository
        {
            get { return _fakeGeneralUnitOfWork.Projects; }
        }

        protected override Project SampleObject
        {
            get { return Builder<Project>.CreateNew().Build(); }
        }

        protected override BaseManager<Project> Manager
        {
            get { return _projectManager; }
        }
    }
}