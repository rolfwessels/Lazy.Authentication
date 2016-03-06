using Microsoft.AspNet.SignalR.Client;

namespace Lazy.Authentication.Sdk.SignalrClient.Base
{
    public abstract class BaseHubClient
    {
        protected readonly IHubProxy _hub;

        public BaseHubClient(HubConnection hubConnection, string hubName)
        {
            _hub = hubConnection.CreateHubProxy(hubName);
        }

    }
}