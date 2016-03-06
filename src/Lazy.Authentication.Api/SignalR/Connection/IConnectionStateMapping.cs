using Lazy.Authentication.Dal.Models.Enums;
using Microsoft.AspNet.SignalR.Hubs;

namespace Lazy.Authentication.Api.SignalR.Connection
{
	public interface IConnectionStateMapping
	{
		int Count { get; }
		ConnectionState AddOrGet(HubCallerContext context);
        ConnectionState Get(HubCallerContext context);
	    ConnectionState Remove(string connectionId);
	    ConnectionState Reconnect(HubCallerContext context);
	    bool IsAuthorized(ConnectionState connectionState, Activity[] activities);
	}
}