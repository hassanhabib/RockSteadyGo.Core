// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public partial class MoveService
    {
        private delegate ValueTask<Move> ReturningMoveFunction();

        private async ValueTask<Move> TryCatch(ReturningMoveFunction returningMoveFunction)
        {
            try
            {
                return await returningMoveFunction();
            }
            catch (NullMoveException nullMoveException)
            {
                throw CreateAndLogValidationException(nullMoveException);
            }
        }

        private MoveValidationException CreateAndLogValidationException(Xeption exception)
        {
            var moveValidationException =
                new MoveValidationException(exception);

            this.loggingBroker.LogError(moveValidationException);

            return moveValidationException;
        }
    }
}