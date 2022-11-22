using QuoVadis.Common.ValueObjects;
using QuoVadis.Contracts;

namespace QuoVadis.GrainInterfaces;

public interface IVehicleGrain : IGrainWithStringKey
{
    [Transaction(TransactionOption.CreateOrJoin)]
    Task RegisterVehicle(string model, decimal costPerKilometer, double latitude, double longitude);

    Task<VehicleInfo> GetVehicleInfo();

    Task<Location> GetLocation();

    [Transaction(TransactionOption.CreateOrJoin)]
    Task<IUserGrain?> GetCurrentRentee();

    [Transaction(TransactionOption.Join)]
    Task BeginRent(IUserGrain user);

    [Transaction(TransactionOption.Join)]
    Task<decimal> EndRent();

    [Transaction(TransactionOption.CreateOrJoin)]
    Task UpdateLocation(double latitude, double longitude);
}