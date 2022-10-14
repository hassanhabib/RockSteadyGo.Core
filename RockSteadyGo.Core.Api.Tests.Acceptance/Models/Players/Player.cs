using System;

namespace RockSteadyGo.Core.Api.Tests.Acceptance.Models.Players
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
