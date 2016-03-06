using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Core.MessageUtil;
using Lazy.Authentication.Dal.Persistance;
using Lazy.Authentication.Dal.Validation;
using Moq;
using NUnit.Framework;

namespace Lazy.Authentication.Core.Tests.Managers
{
    [TestFixture]
    public class BaseManagerTests
    {
        protected BaseManagerArguments _baseManagerArguments;
        protected MemoryGeneralUnitOfWork _fakeGeneralUnitOfWork;
        protected Mock<IMessenger> _mockIMessenger;
        protected Mock<IValidatorFactory> _mockIValidatorFactory;

        #region Setup/Teardown

        public virtual void Setup()
        {
            _mockIMessenger = new Mock<IMessenger>();
            _mockIValidatorFactory = new Mock<IValidatorFactory>();
            _fakeGeneralUnitOfWork = new MemoryGeneralUnitOfWork();
            _baseManagerArguments = new BaseManagerArguments(_fakeGeneralUnitOfWork, _mockIMessenger.Object,
                                                             _mockIValidatorFactory.Object);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockIValidatorFactory.VerifyAll();
            _mockIMessenger.VerifyAll();
        }

        #endregion
    }

}