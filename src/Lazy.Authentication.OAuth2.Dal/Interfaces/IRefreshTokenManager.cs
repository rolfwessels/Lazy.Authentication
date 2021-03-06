﻿using System.Threading.Tasks;

namespace Lazy.Authentication.OAuth2.Dal.Interfaces
{
	public interface IRefreshTokenManager
	{
		IRefreshToken CreateRefresherToken();
		Task SaveRefreshToken(IRefreshToken token);
		Task<IRefreshToken> GetRefreshToken(string hashedTokenId);
		Task DeleteRefreshToken(string hashedTokenId);
	}
}