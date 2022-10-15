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
                (Rule: IsInvalid(move.CreatedDate), Parameter: nameof(Move.CreatedDate)),
                (Rule: IsNotRecent(move.CreatedDate), Parameter: nameof(Move.CreatedDate)));
        }

        private void ValidateMoveOnModify(Move move)
        {
            ValidateMoveIsNotNull(move);
        }

        private static void ValidateMoveIsNotNull(Move move)
        {
            if (move is null)
            {
                throw new NullMoveException();
            }
        }

        public void ValidateMoveId(Guid moveId) =>
            Validate((Rule: IsInvalid(moveId), Parameter: nameof(Move.Id)));

        private static void ValidateStorageMove(Move maybeMove, Guid moveId)
        {
            if (maybeMove is null)
            {
                throw new NotFoundMoveException(moveId);
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

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

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