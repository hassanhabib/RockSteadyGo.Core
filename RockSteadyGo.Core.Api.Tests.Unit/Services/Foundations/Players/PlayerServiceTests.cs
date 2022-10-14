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
using RockSteadyGo.Core.Api.Models.Players;
using RockSteadyGo.Core.Api.Services.Foundations.Players;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPlayerService playerService;

        public PlayerServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.playerService = new PlayerService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

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

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Player CreateRandomModifyPlayer(Guid id, DateTimeOffset createdDate, DateTimeOffset updatedDate)
        {
            Player randomPlayer = CreateRandomPlayer(updatedDate);
            randomPlayer.Id = id;
            randomPlayer.CreatedDate = createdDate;

            return randomPlayer;
        }

        private static IQueryable<Player> CreateRandomPlayers()
        {
            return CreatePlayerFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Player CreateRandomPlayer(DateTimeOffset dateTimeOffset) =>
            CreatePlayerFiller(dateTimeOffset).Create();

        private static Player CreateRandomPlayer() =>
            CreatePlayerFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<Player> CreatePlayerFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Player>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(match => match.Moves).IgnoreIt();

            return filler;
        }
    }
}
