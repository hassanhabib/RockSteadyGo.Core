// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Match> InsertMatchAsync(Match match);
        IQueryable<Match> SelectAllMatches();
        ValueTask<Match> SelectMatchByIdAsync(Guid matchId);
        ValueTask<Match> UpdateMatchAsync(Match match);
        ValueTask<Match> DeleteMatchAsync(Match match);
    }
}
