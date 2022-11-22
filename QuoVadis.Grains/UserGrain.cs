using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;
using QuoVadis.Common;
using QuoVadis.Common.Exceptions;
using QuoVadis.Common.Extensions;
using QuoVadis.Contracts;
using QuoVadis.GrainInterfaces;
using QuoVadis.Grains.State;
using System.Security.Cryptography;

namespace QuoVadis.Grains
{
    [Reentrant]
    public class UserGrain : IUserGrain, IGrainBase
    {
        private readonly IGrainFactory grainFactory;
        private readonly IPersistentState<UserState> userState;
        private readonly ITransactionalState<UserRentState> rentState;

        public IGrainContext GrainContext { get; }

        public UserGrain(
            IGrainContext grainContext,
            IGrainFactory grainFactory,
            [PersistentState(Constants.UserStateName, Constants.SecureStorage)] IPersistentState<UserState> userState,
            [TransactionalState(Constants.UserRentStateName, Constants.TransactionalStorage)] ITransactionalState<UserRentState> rentState)
        {
            GrainContext = grainContext;

            this.grainFactory = grainFactory;
            this.userState = userState;
            this.rentState = rentState;
        }

        public async Task RegisterUser(UserRegistrationData userData)
        {
            // I think this goes without saying but please, don't use this piece of code in production!
            // It's unsafe in many, many ways!
            // Dogfood your custom identity solutions only if you know what you are doing!

            if (userState.RecordExists)
                throw new UsernameAlreadyExistsException();

            userState.State = new UserState();
            userState.State.PasswordSalt = GenerateSalt();
            userState.State.PasswordHash = HashAndSalt(userData.Password);
            userState.State.AccountId = Guid.NewGuid();

            await userState.WriteStateAsync();
        }

        public Task<bool> ValidateLogin(UserLoginData userData)
            => Task.FromResult(userState.RecordExists && HashAndSalt(userData.Password) == userState.State.PasswordHash);

        public Task<IAccountGrain> GetAccount()
        {
            CheckUser();

            return Task.FromResult(grainFactory.GetGrain<IAccountGrain>(userState.State.AccountId));
        }

        public async Task RentVehicle(string registrationNumber)
        {
            CheckUser();

            var vehicleGrain = grainFactory.GetGrain<IVehicleGrain>(registrationNumber);

            await rentState.PerformUpdate(s =>
            {
                if (s.CurrentlyRentedVehicle is not null)
                    throw new UserAlreadyRentingVehicleException();

                s.CurrentlyRentedVehicle = vehicleGrain;
            });

            // our vehicle grain will call the user back with GetAccount() - thankfully chain reentrancy
            // is supported (setting aside disputed whether it's a desirable design practice)
            await vehicleGrain.BeginRent(this);
        }

        public async Task<decimal> ReturnVehicle()
        {
            CheckUser();

            var currentVehicle = await rentState.PerformRead(s => s.CurrentlyRentedVehicle);

            if (currentVehicle is null)
                throw new NoVehicleCurrentlyRentedException();

            var amountDue = await currentVehicle.EndRent();
            await rentState.PerformUpdate(s => s.CurrentlyRentedVehicle = null);

            return amountDue;
        }

        public async Task<VehicleInfo?> GetCurrentlyRentedVehicle()
        {
            CheckUser();

            var vehicle = await rentState.PerformRead(s => s.CurrentlyRentedVehicle);

            if (vehicle is null)
                return null;

            return await vehicle.GetVehicleInfo();
        }

        private void CheckUser()
        {
            if (!userState.RecordExists)
                throw new UserDoesntExistException();
        }

        private string HashAndSalt(string password)
            => (password + userState.State.PasswordSalt).HashWithSha512();

        private string GenerateSalt()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
