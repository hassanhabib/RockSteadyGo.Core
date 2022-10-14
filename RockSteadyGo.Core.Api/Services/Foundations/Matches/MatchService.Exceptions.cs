// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;
using Xeptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public partial class MatchService
    {
        private delegate ValueTask<Match> ReturningMatchFunction();
        private delegate IQueryable<Match> ReturningMatchesFunction();

        private async ValueTask<Match> TryCatch(ReturningMatchFunction returningMatchFunction)
        {
            try
            {
                return await returningMatchFunction();
            }
            catch (NullMatchException nullMatchException)
            {
                throw CreateAndLogValidationException(nullMatchException);
            }
        }

        private MatchValidationException CreateAndLogValidationException(Xeption exception)
        {
            var matchValidationException =
                new MatchValidationException(exception);

            this.loggingBroker.LogError(matchValidationException);

            return matchValidationException;
        }
    }
}