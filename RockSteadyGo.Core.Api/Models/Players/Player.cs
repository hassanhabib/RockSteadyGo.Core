// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Models.Players
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<Move> Moves { get; set; }
    }
}
