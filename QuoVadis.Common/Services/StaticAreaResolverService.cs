using H3;
using H3.Extensions;
using NetTopologySuite.Geometries;

namespace QuoVadis.Common.Services
{
    public class StaticAreaResolverService : IAreaResolverService
    {
        // we are using H3 geohashing at resolution 4 for reverse area lookups
        private static readonly Dictionary<ulong, string> areas = new Dictionary<ulong, string>
        {
            { 0x841f53dffffffff, "Warsaw, Poland" },
            { 0x841f523ffffffff, "Warsaw, Poland" },
            { 0x841f535ffffffff, "Warsaw, Poland" },
        };

        private const int H3Resolution = 4;

        public Task<string?> GetAreaIdentifier(double latitude, double longitude)
        {
            var coordinate = new Coordinate(longitude, latitude);
            var index = coordinate.ToH3Index(H3Resolution);

            if (index.IsValidCell && areas.TryGetValue(index, out var area))
                return Task.FromResult<string?>(area);

            return Task.FromResult<string?>(null);
        }

        public Task<IEnumerable<string>> GetAllAreaIdentifiers()
            => Task.FromResult(areas.Select(a => a.Value).Distinct());
    }
}
