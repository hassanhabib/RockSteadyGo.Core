using Xeptions;

namespace RockSteadyGo.Core.Api.Models.Players.Exceptions
{
    public class PlayerDependencyValidationException : Xeption
    {
        public PlayerDependencyValidationException(Xeption innerException)
            : base(message: "Player dependency validation occurred, please try again.", innerException)
        { }
    }
}