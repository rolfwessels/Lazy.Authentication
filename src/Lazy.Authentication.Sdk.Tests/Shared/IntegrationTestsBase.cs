﻿using System;
using System.IO;
using System.Reflection;
using Lazy.Authentication.Api;
using Lazy.Authentication.Api.AppStartup;
using Lazy.Authentication.Sdk.OAuth;
using Lazy.Authentication.Sdk.RestApi;
using Lazy.Authentication.Shared.Models;
using Microsoft.Owin.Hosting;
using NCrunch.Framework;
using RestSharp;
using log4net;

namespace Lazy.Authentication.Sdk.Tests.Shared
{
    public class IntegrationTestsBase
    {
        public const string ClientId = "Lazy.Authentication.Api";
        public const string AdminPassword = "admin!";
        public const string AdminUser = "admin";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Lazy<string> _hostAddress;

        
        protected static Lazy<RestConnectionFactory> _defaultRequestFactory;
        protected static Lazy<RestConnectionFactory> _adminRequestFactory;
        private static TokenResponseModel _adminToken;

        static IntegrationTestsBase()
        {
            _hostAddress = new Lazy<string>(StartHosting);
            _defaultRequestFactory = new Lazy<RestConnectionFactory>(() => new RestConnectionFactory(_hostAddress.Value));
            _adminRequestFactory = new Lazy<RestConnectionFactory>(CreateAdminRequest);
        }

        

        public string SignalRUri
        {
            get { return _defaultRequestFactory.Value.GetClient().BuildUri(new RestRequest("signalr")).ToString(); }
        }

        public static TokenResponseModel AdminToken
        {
            get
            {
                if (_adminToken == null)
                {
                    _log.Info("Create admin request: " + _adminRequestFactory.Value);
                }

                return _adminToken;
            }

        }

        #region Private Methods

        private static string StartHosting()
        {
            int port = new Random().Next(9000, 9999);
            string address = string.Format("http://localhost:{0}/", port);
            _log.Info(string.Format("Starting api on [{0}]", address));
            WebApp.Start<Startup>(address);
            return address;
        }


        private static RestConnectionFactory CreateAdminRequest()
        {
            var restConnectionFactory = new RestConnectionFactory(_hostAddress.Value);
            
            var oAuthConnection = new OAuthApiClient(restConnectionFactory);
            _adminToken = oAuthConnection.GenerateToken(new TokenRequestModel
                {
                    UserName = AdminUser, ClientId = ClientId, Password = AdminPassword
                }).Result;
            
            return restConnectionFactory;
        }

        #endregion
    }
}