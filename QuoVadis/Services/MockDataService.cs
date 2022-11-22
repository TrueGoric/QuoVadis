using QuoVadis.GrainInterfaces;

namespace QuoVadis.Services
{
    public class MockDataService : IHostedService
    {
        private readonly IServiceProvider services;

        public MockDataService(IServiceProvider services)
        {
            this.services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var grainFactory = services.GetRequiredService<IGrainFactory>();
            var transactionClient = services.GetRequiredService<ITransactionClient>();

            await transactionClient.RunTransaction(TransactionOption.Create, async () =>
            {
                await grainFactory.GetGrain<IVehicleGrain>("WU12345").RegisterVehicle("Tuskla Model S", 15.99M, 52.23425028052469, 20.999284584849043);
                await grainFactory.GetGrain<IVehicleGrain>("WU00700").RegisterVehicle("Audi-o e-punk GT", 15.99M, 52.234350129519626, 20.99952416677028);
                await grainFactory.GetGrain<IVehicleGrain>("WX10101").RegisterVehicle("Sharp #1", 2.19M, 52.234405569728835, 20.99963950175872);
                await grainFactory.GetGrain<IVehicleGrain>("DW08132").RegisterVehicle("Dalmacia Autumn", 1.99M, 52.232761967792115, 20.996195835027667);
                await grainFactory.GetGrain<IVehicleGrain>("WMA36000").RegisterVehicle("Dalmacia Autumn", 1.99M, 52.23471109948102, 20.991049678195314);
                await grainFactory.GetGrain<IVehicleGrain>("WM42071").RegisterVehicle("Dalmacia Autumn", 1.99M, 52.239262831574585, 21.010155341948234);
                await grainFactory.GetGrain<IVehicleGrain>("WPI44332").RegisterVehicle("BWM X", 10.99M, 52.230179209027405, 21.013921126071942);
                await grainFactory.GetGrain<IVehicleGrain>("WPI4122").RegisterVehicle("BWM X", 10.99M, 52.21731439541951, 21.004120293032738);
                await grainFactory.GetGrain<IVehicleGrain>("WPI12322").RegisterVehicle("BWM X", 10.99M, 52.205752782464096, 21.03193563388014);
                await grainFactory.GetGrain<IVehicleGrain>("WULFL4F").RegisterVehicle("BWM X", 10.99M, 52.20285538503253, 20.968898820252186);
                await grainFactory.GetGrain<IVehicleGrain>("WGR433JD").RegisterVehicle("BWM X", 10.99M, 52.25073476689932, 21.039289928803395);
                await grainFactory.GetGrain<IVehicleGrain>("WX20202").RegisterVehicle("Sharp #1", 2.19M, 52.23497373650151, 21.037188701682464);
                await grainFactory.GetGrain<IVehicleGrain>("LUB3424").RegisterVehicle("Sharp #1", 2.19M, 52.19660924137198, 20.983187164674522);
                await grainFactory.GetGrain<IVehicleGrain>("LUB1234").RegisterVehicle("Sharp #1", 2.19M, 52.1966736390839, 20.963435629737766);
                await grainFactory.GetGrain<IVehicleGrain>("POW1N5A").RegisterVehicle("Sharp #1", 2.19M, 52.24963138544811, 20.95637273865581);
                await grainFactory.GetGrain<IVehicleGrain>("LL3434L").RegisterVehicle("Sharp #1", 2.19M, 52.27584147475735, 21.06645495861184);
                await grainFactory.GetGrain<IVehicleGrain>("RZ35424").RegisterVehicle("Sharp #1", 2.19M, 52.193696271611834, 21.09127489882402);
                await grainFactory.GetGrain<IVehicleGrain>("HPU6566").RegisterVehicle("Sharp #1", 2.19M, 52.189507997876184, 21.045970383141718);
                await grainFactory.GetGrain<IVehicleGrain>("UU44444").RegisterVehicle("Sharp #1", 2.19M, 52.23277198101075, 21.00831798066652);
                await grainFactory.GetGrain<IVehicleGrain>("WMAWMA1").RegisterVehicle("Sharp #1", 2.19M, 52.23209360887884, 21.008795925960914);
                await grainFactory.GetGrain<IVehicleGrain>("OO55335").RegisterVehicle("Sharp #1", 2.19M, 52.216478689084624, 21.015767523207472);
                await grainFactory.GetGrain<IVehicleGrain>("GG66666").RegisterVehicle("Sharp #1", 2.19M, 52.2065498104452, 20.98495137539595);

                // uncomment to see what happens when there's a vehicle that's located in the ocean
                // await grainFactory.GetGrain<IVehicleGrain>("UND3RW4T3R").RegisterVehicle("Phiat 400", 0.99M, 40.80993384035414, -37.6199818323569);
            });

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
