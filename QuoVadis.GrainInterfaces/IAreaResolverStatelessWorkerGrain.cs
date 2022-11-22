using QuoVadis.Common.ValueObjects;
using System.Collections.Immutable;

namespace QuoVadis.GrainInterfaces
{
    public interface IAreaResolverStatelessWorkerGrain : IGrainWithIntegerKey
    {
        Task<IAreaGrain?> GetAreaForLocation(Location location);

        Task<ImmutableList<string>> GetAllAreas();
    }
}
