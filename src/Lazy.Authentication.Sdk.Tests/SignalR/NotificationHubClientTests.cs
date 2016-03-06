using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using log4net;
using Lazy.Authentication.Api.AppStartup;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Core.Tests.Helpers;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Models.Reference;
using Lazy.Authentication.Sdk.SignalrClient;
using Lazy.Authentication.Sdk.Tests.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Enums;
using Microsoft.AspNet.SignalR.Client;
using NUnit.Framework;

namespace Lazy.Authentication.Sdk.Tests.SignalR
{
    [TestFixture]
	[Category("Integration")]
    public class NotificationHubClientTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private HubConnection _hubConnection;
        private NotificationHubClient _notificationHubClient;

        #region Setup/Teardown

        protected void Setup()
		{

            _hubConnection = CreateHubConnection();
			_notificationHubClient = new NotificationHubClient(_hubConnection);
			_hubConnection.Start().Wait();
			_log.Info(string.Format("Connection made {0}", SignalRUri));
		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

        [Test]
        public void SubscriptToUpdateService_GivenConnectionDetails_ShouldRegisterToProjectUpdates()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            var projectManager = IocApi.Instance.Resolve<IProjectManager>();
            var responses = new List<ValueUpdateModel<ProjectReference>>();
            // action
            _notificationHubClient.Subscribe<ProjectReference>("Project", (name, value) => responses.Add(value)).Wait();
            projectManager.Insert(project).Wait();
            projectManager.Delete(project.Id).Wait();
            // assert
            responses.WaitFor(x => x.Count > 1);
            responses.Should().HaveCount(2);
            responses.First().Value.ShouldBeEquivalentTo(project);
            responses.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Inserted);
            responses.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Removed);
        }

        [Test]
        public void Unsubscribe_GivenConnectionDetails_ShouldRegisterToProjectUpdates()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            var projectManager = IocApi.Instance.Resolve<IProjectManager>();
            var responses = new List<ValueUpdateModel<ProjectReference>>();
            // action
            _notificationHubClient.Subscribe<ProjectReference>("Project", (name, value) => responses.Add(value)).Wait();
            projectManager.Insert(project).Wait();
            _notificationHubClient.Unsubscribe("Project").Wait();
            projectManager.Delete(project.Id).Wait();
            // assert
            responses.WaitFor(x => x.Count > 1);
            responses.Should().HaveCount(1);
            responses.First().Value.ShouldBeEquivalentTo(project);
        }

        protected HubConnection CreateHubConnection()
        {
            var queryString = new Dictionary<string, string> { { AdminToken.TokenType, AdminToken.AccessToken } };
            return new HubConnection(SignalRUri, queryString);
        }

	}

    
}