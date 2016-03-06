using System;
using Lazy.Authentication.Shared.Models.Interfaces;

namespace Lazy.Authentication.Shared.Models.Base
{
    public abstract class BaseModel : IBaseModel
    {
        public Guid Id { get; set; }
    }
}