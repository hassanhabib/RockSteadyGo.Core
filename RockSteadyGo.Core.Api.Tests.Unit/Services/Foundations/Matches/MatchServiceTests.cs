// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using RockSteadyGo.Core.Api.Brokers.DateTimes;
using RockSteadyGo.Core.Api.Brokers.Loggings;
using RockSteadyGo.Core.Api.Brokers.Storages;
using RockSteadyGo.Core.Api.Services.Foundations.Matches;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Match = RockSteadyGo.Core.Api.Models.Matches.Match;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Matches
{
    public partial class MatchServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMatchService matchService;

        public MatchServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.matchService = new MatchService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<Match> CreateRandomMatches()
        {
            return CreateMatchFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Match CreateRandomModifyMatch(Guid id, DateTimeOffset createdDate, DateTimeOffset updatedDate)
        {
            Match randomMatch = CreateRandomMatch(updatedDate);
            randomMatch.Id = id;
            randomMatch.CreatedDate = createdDate;

            return randomMatch;
        }

        private static Match CreateRandomMatch() =>
            CreateMatchFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Match CreateRandomMatch(DateTimeOffset dateTimeOffset) =>
            CreateMatchFiller(dateTimeOffset).Create();

        private static Filler<Match> CreateMatchFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Match>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(match => match.Moves).IgnoreIt();

            return filler;
        }
    }
}