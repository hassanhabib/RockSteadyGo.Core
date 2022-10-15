// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Models.Moves.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Moves
{
    public partial class MoveService
    {
        private void ValidateMoveOnAdd(Move move)
        {
            ValidateMoveIsNotNull(move);

            Validate(
                (Rule: IsInvalid(move.Id), Parameter: nameof(Move.Id)),
                (Rule: IsInvalid(move.MatchId), Parameter: nameof(Move.MatchId)),
                (Rule: IsInvalid(move.PlayerId), Parameter: nameof(Move.PlayerId)),
                (Rule: IsInvalid(move.LocationX, 0, 2), Parameter: nameof(Move.LocationX)),
                (Rule: IsInvalid(move.LocationY, 0, 2), Parameter: nameof(Move.LocationY)),
                (Rule: IsInvalid(move.CreatedDate), Parameter: nameof(Move.CreatedDate)));
        }

        private static void ValidateMoveIsNotNull(Move move)
        {
            if (move is null)
            {
                throw new NullMoveException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(int value, int rangeStart, int rangeEnd) => new
        {
            Condition = value < rangeStart || value > rangeEnd,
            Message = "Invalid value"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMoveException = new InvalidMoveException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMoveException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMoveException.ThrowIfContainsErrors();
        }
    }
}