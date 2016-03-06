using System.Collections.Generic;
using FizzWare.NBuilder;
using Lazy.Authentication.Api.Tests.Helper;
using Lazy.Authentication.Sdk.RestApi;
using Lazy.Authentication.Sdk.Tests.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;
using NUnit.Framework;

namespace Lazy.Authentication.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class ProjectApiClientTests : CrudComponentTestsBase<ProjectModel, ProjectCreateUpdateModel, ProjectReferenceModel>
	{
		private ProjectApiClient _projectApiClient;

	    #region Setup/Teardown

	    protected override void Setup()
		{
            _projectApiClient = new ProjectApiClient(_adminRequestFactory.Value);
            SetRequiredData(_projectApiClient);
		}

	    [TearDown]
		public void TearDown()
		{

		}

	    protected override IList<ProjectCreateUpdateModel> GetExampleData()
	    {
	        var projectDetailModel = Builder<ProjectCreateUpdateModel>.CreateListOfSize(2).All().WithValidModelData().Build();
	        return projectDetailModel;
	    }

	    #endregion
	}

    
}