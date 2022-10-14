// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using RockSteadyGo.Core.Api.Models.Matches;
using RockSteadyGo.Core.Api.Models.Matches.Exceptions;

namespace RockSteadyGo.Core.Api.Services.Foundations.Matches
{
    public partial class MatchService
    {
        private void ValidateMatchOnAdd(Match match)
        {
            ValidateMatchIsNotNull(match);

            Validate(
                (Rule: IsInvalid(match.Id), Parameter: nameof(Match.Id)),
                (Rule: IsInvalid(match.CreatedDate), Parameter: nameof(Match.CreatedDate)),
                (Rule: IsNotRecent(match.CreatedDate), Parameter: nameof(Match.CreatedDate)));
        }

        public void ValidateMatchId(Guid matchId) =>
            Validate((Rule: IsInvalid(matchId), Parameter: nameof(Match.Id)));

        private static void ValidateStorageMatch(Match maybeMatch, Guid matchId)
        {
            if (maybeMatch is null)
            {
                throw new NotFoundMatchException(matchId);
            }
        }

        private static void ValidateAgainstStorageMatchOnModify(Match inputMatch, Match storageMatch)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputMatch.CreatedDate,
                    secondDate: storageMatch.CreatedDate,
                    secondDateName: nameof(Match.CreatedDate)),
                Parameter: nameof(Match.CreatedDate)));
        }

        private void ValidateMatchOnModify(Match match)
        {
            ValidateMatchIsNotNull(match);

            Validate(
                (Rule: IsInvalid(match.Id), Parameter: nameof(Match.Id)),
                (Rule: IsInvalid(match.CreatedDate), Parameter: nameof(Match.CreatedDate)));
        }

        private static void ValidateMatchIsNotNull(Match match)
        {
            if (match is null)
            {
                throw new NullMatchException();
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMatchException = new InvalidMatchException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMatchException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMatchException.ThrowIfContainsErrors();
        }
    }
}