using Orleans.Concurrency;
using Orleans.Runtime;
using QuoVadis.Common.Services;
using QuoVadis.Common.ValueObjects;
using QuoVadis.GrainInterfaces;
using System.Collections.Immutable;

namespace QuoVadis.Grains
{
    [StatelessWorker]
    public class AreaResolverStatelessWorkerGrain : IAreaResolverStatelessWorkerGrain, IGrainBase
    {
        private readonly IAreaResolverService areaResolverService;
        private readonly IGrainFactory grainFactory;

        public IGrainContext GrainContext { get; }

        public AreaResolverStatelessWorkerGrain(
            IGrainContext grainContext,
            IGrainFactory grainFactory,
            IAreaResolverService areaResolverService) // Regular dependency injection works in Orleans
        {
            GrainContext = grainContext;

            this.grainFactory = grainFactory;
            this.areaResolverService = areaResolverService;
        }

        public async Task<IAreaGrain?> GetAreaForLocation(Location location)
        {
            var areaId = await areaResolverService.GetAreaIdentifier(location.Latitude, location.Longitude);

            if (areaId is null)
                return null;

            return grainFactory.GetGrain<IAreaGrain>(areaId);
        }

        public async Task<ImmutableList<string>> GetAllAreas()
            => (await areaResolverService.GetAllAreaIdentifiers()).ToImmutableList();
    }
}
