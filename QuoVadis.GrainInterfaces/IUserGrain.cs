using QuoVadis.Contracts;

namespace QuoVadis.GrainInterfaces;

public interface IUserGrain : IGrainWithStringKey
{
    Task RegisterUser(UserRegistrationData userData);

    Task<bool> ValidateLogin(UserLoginData userData);

    [Transaction(TransactionOption.CreateOrJoin)]
    Task<IAccountGrain> GetAccount();

    [Transaction(TransactionOption.CreateOrJoin)]
    Task RentVehicle(string registrationNumber);

    [Transaction(TransactionOption.CreateOrJoin)]
    Task<decimal> ReturnVehicle();

    [Transaction(TransactionOption.CreateOrJoin)]
    Task<VehicleInfo?> GetCurrentlyRentedVehicle();
}