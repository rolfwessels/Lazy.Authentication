using System;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using log4net;
using Lazy.Authentication.Api.SignalR.Attributes;
using Lazy.Authentication.Api.SignalR.Connection;
using Lazy.Authentication.Core.MessageUtil;
using Lazy.Authentication.Core.MessageUtil.Models;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Models.Enums;
using Lazy.Authentication.Dal.Models.Reference;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Enums;
using Lazy.Authentication.Utilities.Helpers;
using Microsoft.AspNet.SignalR.Hubs;
using ReflectionHelper = Lazy.Authentication.Utilities.Helpers.ReflectionHelper;

namespace Lazy.Authentication.Api.SignalR.Hubs
{
    
    public class NotificationHub : BaseHub
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMessenger _messenger;

        public NotificationHub(IConnectionStateMapping connectionStateMapping , IMessenger messenger)
            : base(connectionStateMapping)
        {
            _messenger = messenger;
        }

        

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.Subscribe)]
        public Task SubscribeToUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var connection = _connectionsState.AddOrGet(Context);
                var makeGenericType = GetTypeFromName(typeName);
                _log.Debug("NotificationHub:SubscribeToUpdates " + typeName);
                _messenger.Register(makeGenericType, connection, (obj) => CallBackToClient(typeName,obj));
            });
        }


        [HubAuthorizeActivity(Activity.Subscribe)]
        public Task UnsubscribeFromUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var makeGenericType = GetTypeFromName(typeName);
                var connection = _connectionsState.AddOrGet(Context);
                _log.Debug("NotificationHub:UnsubscribeFromUpdates " + typeName);
                _messenger.Unregister(makeGenericType, connection);
            });
        }

        #region Overrides of BaseHub

        public override Task OnDisconnected(bool stopCalled)
        {
            var connection = _connectionsState.AddOrGet(Context);
            _messenger.Unregister(connection); 
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #endregion

        private static Type GetTypeFromName(string typeName)
        {
            var type = ReflectionHelper.FindOfType(typeof(Project).Assembly, typeName);
            var makeGenericType = ReflectionHelper.MakeGenericType(typeof(DalUpdateMessage<>), type);
            return makeGenericType;
        }

        private void CallBackToClient(string typeName,object obj)
        {
            _log.Debug("NotificationHub:CallBackToClient " + obj.GetType().FullName);
            Clients.Caller.OnUpdate(typeName,obj);
        }

        
    }
}