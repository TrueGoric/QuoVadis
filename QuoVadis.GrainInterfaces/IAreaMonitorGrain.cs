using QuoVadis.Common.ValueObjects;
using QuoVadis.GrainInterfaces.Observers;
using System.Collections.Immutable;

namespace QuoVadis.GrainInterfaces
{
    public interface IAreaMonitorGrain : IGrainWithStringKey
    {
        Task<ImmutableDictionary<string, Location>> GetVehicles();

        Task Subscribe(Guid observerId, IAreaMonitorObserver observer);

        Task Unsubscribe(Guid observerId);
    }
}
