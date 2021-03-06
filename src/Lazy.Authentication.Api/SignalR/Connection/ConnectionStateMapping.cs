using System.Collections.Concurrent;
using System.Reflection;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using log4net;
using Lazy.Authentication.Api.SignalR.Attributes;
using Lazy.Authentication.Dal.Models.Enums;
using Microsoft.AspNet.SignalR.Hubs;

namespace Lazy.Authentication.Api.SignalR.Connection
{
	public class ConnectionStateMapping : IConnectionStateMapping
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IUserManager _userManager;
		private ConcurrentDictionary<string, ConnectionState> _connections;
		
		public ConnectionStateMapping(IUserManager userManager)
		{
			_userManager = userManager;
			_connections = new ConcurrentDictionary<string, ConnectionState>();
		}

		public int Count
		{
			get { return _connections.Count; }
		}

	

		public ConnectionState AddOrGet(HubCallerContext context )
		{
			return _connections.GetOrAdd(context.ConnectionId, (t) =>
				{
					var principal = context.Request.GetPrincipal();
					var userByEmail = _userManager.GetUserByEmail(principal.Identity.Name).Result;
					var connectionState = new ConnectionState(context.ConnectionId, principal, userByEmail);
					return connectionState;
				});
		}

	    public ConnectionState Get(HubCallerContext context)
	    {
	        ConnectionState value;
	        _connections.TryGetValue(context.ConnectionId, out value);
	        return value;
	    }

	    public ConnectionState Remove(string connectionId)
		{
			ConnectionState removed;
			_connections.TryRemove(connectionId, out removed);
			return removed;
		}

		public ConnectionState Reconnect(HubCallerContext context)
		{
			_log.Info("Reconnect.... " + _connections.ContainsKey(context.ConnectionId));
			return AddOrGet(context);
		}

		public bool IsAuthorized(ConnectionState connectionState, Activity[] activities)
		{
			return connectionState.IsAuthorized(activities);
		}
	}
}
