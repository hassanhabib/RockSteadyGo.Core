// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using RockSteadyGo.Core.Api.Tests.Acceptance.Models.MoveTypes;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Models.Moves
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
    }
}
