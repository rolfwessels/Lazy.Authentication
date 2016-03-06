using AutoMapper;
using Lazy.Authentication.Core.MessageUtil.Models;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Enums;

namespace Lazy.Authentication.Api.Models.Mappers
{
	public static partial class MapApi
	{
        static MapApi()
        {
            MapUserModel();
            MapProjectModel();

        }

		
		public static ValueUpdateModel<TModel> ToValueUpdateModel<T, TModel>(this DalUpdateMessage<T> updateMessage)
		{
			return new ValueUpdateModel<TModel>(Mapper.Map<T, TModel>(updateMessage.Value), (UpdateTypeCodes) updateMessage.UpdateType);
		}
	}
}