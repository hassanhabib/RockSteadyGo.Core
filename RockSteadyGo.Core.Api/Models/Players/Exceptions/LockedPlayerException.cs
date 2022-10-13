using System;
using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class LockedPlayerException : Xeption
    {
        public LockedPlayerException(Exception innerException)
            : base(message: "Locked player record exception, please try again later", innerException)
        {
        }
    }
}