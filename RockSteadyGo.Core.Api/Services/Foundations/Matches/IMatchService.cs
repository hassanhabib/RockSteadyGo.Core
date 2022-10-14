// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Matches;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public interface IMatchService
    {
        ValueTask<Match> AddMatchAsync(Match match);
    }
}