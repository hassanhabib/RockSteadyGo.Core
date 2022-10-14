// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.MoveTypes;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Models.Moves
{
    public class Move
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public MoveTypeEnum Type { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}
