// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Moq;
using RockSteadyGo.Core.Api.Brokers.DateTimes;
using RockSteadyGo.Core.Api.Brokers.Loggings;
using RockSteadyGo.Core.Api.Brokers.Storages;
using RockSteadyGo.Core.Api.Models.Moves;
using RockSteadyGo.Core.Api.Services.Foundations.Moves;
using Tynamix.ObjectFiller;
using Xeptions;

namespace RockSteadyGo.Core.Api.Tests.Unit.Services.Foundations.Moves
{
    public partial class MoveServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMoveService moveService;

        public MoveServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.moveService = new MoveService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomPosition() =>
            new IntRange(min: 0, max: 2).GetValue();

        private static Move CreateRandomMove(DateTimeOffset dateTimeOffset) =>
            CreateMoveFiller(dateTimeOffset).Create();

        private static Filler<Move> CreateMoveFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Move>();

            filler.Setup()
                .OnProperty(move => move.LocationX).Use(GetRandomPosition())
                .OnProperty(move => move.LocationY).Use(GetRandomPosition())
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(move => move.Match).IgnoreIt()
                .OnProperty(move => move.Player).IgnoreIt();

            return filler;
        }
    }
}