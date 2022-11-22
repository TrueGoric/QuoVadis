using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using QuoVadis.Common.Exceptions;
using QuoVadis.Contracts;
using QuoVadis.Extensions;
using QuoVadis.GrainInterfaces;
using QuoVadis.Proto;
using System.Globalization;
using System.Security.Claims;

namespace QuoVadis.RpcServices
{
    public class UserService : User.UserBase
    {
        private readonly IGrainFactory grainFactory;

        public UserService(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            // grains are not instantiated, so we can safely get a reference
            // to a grain that probably was never called - it will be
            // activated by the cluster
            var userGrain = grainFactory.GetGrain<IUserGrain>(request.Username);
            var userData = new UserRegistrationData(request.Username, request.Password);

            try
            {
                await userGrain.RegisterUser(userData);
            }
            catch (UsernameAlreadyExistsException)
            {
                return new RegisterResponse() { Result = RegistrationResult.UsernameInUse };
            }

            return new RegisterResponse() { Result = RegistrationResult.SuccessfullyRegistered };
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var userGrain = grainFactory.GetGrain<IUserGrain>(request.Username);
            var userData = new UserLoginData(request.Username, request.Password);
            var loginResult = await userGrain.ValidateLogin(userData);

            if (loginResult)
            {
                await SignIn(userData.Username, context);
            }

            return new LoginResponse() { Success = loginResult };
        }

        [Authorize(Roles = "User")]
        public override async Task<CheckBalanceResponse> CheckBalance(CheckBalanceRequest request, ServerCallContext context)
        {
            var userGrain = GetUserGrain(context);
            var accountGrain = await userGrain.GetAccount();

            var balance = await accountGrain.GetBalance();

            return new CheckBalanceResponse { Available = balance.Available.ToString("F2", CultureInfo.InvariantCulture), Total = balance.Total.ToString("F2", CultureInfo.InvariantCulture) };
        }

        [Authorize(Roles = "User")]
        public override async Task<AddFundsResponse> AddFunds(AddFundsRequest request, ServerCallContext context)
        {
            var userGrain = GetUserGrain(context);
            var accountGrain = await userGrain.GetAccount();

            if (!decimal.TryParse(request.Amount, out var amount) || amount < 0M)
                return new AddFundsResponse { Result = AddFundsResult.InvalidAmount };

            await accountGrain.AddBalance(amount);

            return new AddFundsResponse { Result = AddFundsResult.SuccessfullyAdded };
        }

        private async Task SignIn(string username, ServerCallContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.GetHttpContext().SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private IUserGrain GetUserGrain(ServerCallContext context)
        {
            var username = context.GetIdentityUsername() ?? throw new InvalidDataException();
            return grainFactory.GetGrain<IUserGrain>(username);
        }
    }
}
