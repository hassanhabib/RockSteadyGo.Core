// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private void ValidatePlayerOnAdd(Player player)
        {
            ValidatePlayerIsNotNull(player);
        }

        private static void ValidatePlayerIsNotNull(Player player)
        {
            if (player is null)
            {
                throw new NullPlayerException();
            }
        }
    }
}