﻿namespace Lazy.Authentication.OAuth2.Dal.Interfaces
{
	public interface IOAuthSecurity
	{
		string GetHash(string clientSecret);
	}
}