namespace QuoVadis.Common.Services
{
    public interface IAreaResolverService
    {
        Task<string?> GetAreaIdentifier(double latitude, double longitude);

        Task<IEnumerable<string>> GetAllAreaIdentifiers();
    }
}
