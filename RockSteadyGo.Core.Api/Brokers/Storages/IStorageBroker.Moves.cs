using System.Threading.Tasks;
using RockSteadyGo.Core.Api.Models.Moves;

namespace RockSteadyGo.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Move> InsertMoveAsync(Move move);
    }
}
