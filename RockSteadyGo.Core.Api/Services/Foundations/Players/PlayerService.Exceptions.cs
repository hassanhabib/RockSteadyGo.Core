// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private delegate ValueTask<Player> ReturningPlayerFunction();

        private async ValueTask<Player> TryCatch(ReturningPlayerFunction returningPlayerFunction)
        {
            try
            {
                return await returningPlayerFunction();
            }
            catch (NullPlayerException nullPlayerException)
            {
                throw CreateAndLogValidationException(nullPlayerException);
            }
        }

        private PlayerValidationException CreateAndLogValidationException(Xeption exception)
        {
            var playerValidationException =
                new PlayerValidationException(exception);

            this.loggingBroker.LogError(playerValidationException);

            return playerValidationException;
        }
    }
}