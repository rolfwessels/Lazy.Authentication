﻿using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using Lazy.Authentication.Shared.Models;

namespace Lazy.Authentication.Api.Tests.Helper
{
    public static class AddingValidDataHelper
    {
        

        public static ISingleObjectBuilder<T> WithValidModelData<T>(this ISingleObjectBuilder<T> builder)
        {
            return builder.With(WithValidDataAdded);
        }

        public static IOperable<T> WithValidModelData<T>(this IOperable<T> operable)
        {
            return operable.With(WithValidDataAdded);
        }

        private static T WithValidDataAdded<T>(T buildingObject)
        {
            var user = buildingObject as UserCreateUpdateModel;
            if (user != null)
            {
                user.Email = GetRandom.Email().ToLower();
            }
            return buildingObject;
        }
    }
}