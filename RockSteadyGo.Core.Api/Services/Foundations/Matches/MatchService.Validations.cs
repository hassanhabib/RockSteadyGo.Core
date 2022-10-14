// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public partial class MatchService
    {
        private void ValidateMatchOnAdd(Match match)
        {
            ValidateMatchIsNotNull(match);
        }

        private static void ValidateMatchIsNotNull(Match match)
        {
            if (match is null)
            {
                throw new NullMatchException();
            }
        }
    }
}