using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Lazy.Authentication.Api.Common;
using Lazy.Authentication.Api.Models.Mappers;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Core.Tests.Helpers;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Shared.Models.Base;
using Moq;
using NUnit.Framework;

namespace Lazy.Authentication.Api.Tests.Common
{
    public abstract class BaseCommonControllerTests<TDal, TModel, TReferenceModel, TDetailModel, TManager>
        where TDal : BaseDalModelWithId
        where TModel : BaseModel
        where TManager : class, IBaseManager<TDal>
    {
        protected Mock<TManager> _mockManager;
        protected BaseCommonController<TDal, TModel, TReferenceModel, TDetailModel> _commonController;
        


        public virtual void Setup()
        {
            MapApi.Initialize();
            _commonController = GetCommonController();
            _mockManager = GetManager();
        }

        protected virtual TDal SampleItem
        {
            get { return Builder<TDal>.CreateNew().Build(); }
        }

        protected abstract Mock<TManager> GetManager();

        protected abstract BaseCommonController<TDal, TModel, TReferenceModel, TDetailModel> GetCommonController();


        [TearDown]
        public virtual void TearDown()
        {
            _mockManager.VerifyAll();
        }


        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _commonController.Should().NotBeNull();
        }

        [Test]
        public void Get_GivenRequest_ShouldReturnProjectReferenceModels()
        {
            // arrange
            Setup();
            var reference = Builder<TDal>.CreateListOfSize(2).Build();
            _mockManager.Setup(mc => mc.Query()).Returns(reference.ToList().AsQueryable());
            // action
            var result = _commonController.Get().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void GetDetail_GivenRequest_ShouldReturnProjectModel()
        {
            // arrange
            Setup();
            var reference = Builder<TDal>.CreateListOfSize(2).Build();
            _mockManager.Setup(mc => mc.Query())
                               .Returns(reference.ToList().AsQueryable());
            // action
            var result = _commonController.GetDetail().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void Get_GivenProjectId_ShouldCallGetProject()
        {
            // arrange
            Setup();
            var dal = SampleItem;
            _mockManager.Setup(mc => mc.Get(dal.Id)).Returns(dal);
            // action
            var result = _commonController.Get(dal.Id).Result;
            // assert
            result.Id.Should().Be(dal.Id);
        }

        [Test]
        public void Put_GivenProjectId_ShouldUpdateAGivenProject()
        {
            // arrange
            Setup();
            var dal = SampleItem;
            _mockManager.Setup(mc => mc.Get(dal.Id))
                               .Returns(dal);
            _mockManager.Setup(mc => mc.Update(dal))
                               .Returns(dal);
            var model = Builder<TDetailModel>.CreateNew().Build();
            AddAdditionalMappings(model, dal);
            // action
            var result = _commonController.Update(dal.Id, model).Result;
            // assert
            result.Id.Should().Be(dal.Id);
        }

        [Test]
        public void Post_GivenProjectId_ShouldAddAProject()
        {
            // arrange
            Setup();
            var dal = SampleItem;
            var model = Builder<TDetailModel>.CreateNew().Build();
            _mockManager.Setup(mc => mc.Insert(It.IsAny<TDal>())).Returns(dal);
            AddAdditionalMappings(model, dal);
            // action
            var result = _commonController.Insert(model).Result;
            // assert
            result.Id.Should().Be(dal.Id);
        }

        protected virtual void AddAdditionalMappings(TDetailModel model, TDal dal)
        {
            
        }


        [Test]
        public void Delete_GivenProjectId_ShouldRemoveProject()
        {
            // arrange
            Setup();
            var project = SampleItem;
            _mockManager.Setup(mc => mc.Delete(project.Id)).Returns(project);
            // action
            var result = _commonController.Delete(project.Id).Result;
            // assert
            result.Should().Be(true);
        }

    }
}