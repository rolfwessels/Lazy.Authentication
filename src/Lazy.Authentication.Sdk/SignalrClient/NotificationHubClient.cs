using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Lazy.Authentication.Sdk.SignalrClient.Base;
using Lazy.Authentication.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

namespace Lazy.Authentication.Sdk.SignalrClient
{

    public class NotificationHubClient : BaseHubClient
    {
        public NotificationHubClient(HubConnection hubConnection) : base(hubConnection, "NotificationHub")
        {
            
        }

        public async Task Subscribe<T>(string name, Action<string,ValueUpdateModel<T>> callback)
        {
            _hub.On("OnUpdate", callback);
            await _hub.Invoke("SubscribeToUpdates", name);
        }

        public async Task Unsubscribe(string name)
        {
            await _hub.Invoke("UnsubscribeFromUpdates", name);
        }
    }
}