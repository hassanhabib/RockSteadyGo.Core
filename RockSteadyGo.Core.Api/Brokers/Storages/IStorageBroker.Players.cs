using System;
using System.Linq;
using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Players;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Player> InsertPlayerAsync(Player player);
    }
}
