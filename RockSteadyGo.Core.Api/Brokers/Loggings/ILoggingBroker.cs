using System;

namespace RockSteadyGo.Core.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
    }
}
