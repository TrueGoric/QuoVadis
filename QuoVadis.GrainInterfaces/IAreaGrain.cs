using QuoVadis.Contracts;
using System.Collections.Immutable;

namespace QuoVadis.GrainInterfaces;

public interface IAreaGrain : IGrainWithStringKey
{
    [Transaction(TransactionOption.CreateOrJoin)]
    Task<ImmutableList<VehicleInfo>> GetVehicles();

    [Transaction(TransactionOption.Join)]
    Task RegisterVehicle(VehicleInfo vehicle);

    [Transaction(TransactionOption.Join)]
    Task UnregisterVehicle(IVehicleGrain vehicle);
}