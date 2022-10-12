// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Models.Players.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private void ValidatePlayerOnAdd(Player player)
        {
            ValidatePlayerIsNotNull(player);

            Validate(
                (Rule: IsInvalid(player.Id), Parameter: nameof(Player.Id)),
                (Rule: IsInvalid(player.Name), Parameter: nameof(Player.Name)),
                (Rule: IsInvalid(player.Username), Parameter: nameof(Player.Username)),
                (Rule: IsInvalid(player.CreatedDate), Parameter: nameof(Player.CreatedDate)));
        }

        private static void ValidatePlayerIsNotNull(Player player)
        {
            if (player is null)
            {
                throw new NullPlayerException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPlayerException = new InvalidPlayerException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPlayerException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPlayerException.ThrowIfContainsErrors();
        }
    }
}