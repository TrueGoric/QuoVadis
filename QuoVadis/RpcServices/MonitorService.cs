using Grpc.Core;
using QuoVadis.Common.Extensions;
using QuoVadis.GrainInterfaces;
using QuoVadis.GrainInterfaces.Observers;
using QuoVadis.Observers;
using QuoVadis.Proto;
using System.Threading.Tasks.Dataflow;
using Monitor = QuoVadis.Proto.Monitor;

namespace QuoVadis.RpcServices
{
    public class MonitorService : Monitor.MonitorBase
    {
        private static readonly TimeSpan RefreshPeriod = TimeSpan.FromSeconds(60 * 4);
        private readonly IGrainFactory grainFactory;
        
        public MonitorService(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public override async Task MonitorLocation(MonitorLocationRequest request, IServerStreamWriter<MonitorLocationEvent> responseStream, ServerCallContext context)
        {
            var monitorGrain = grainFactory.GetGrain<IAreaMonitorGrain>(request.Area);
            var observerId = Guid.NewGuid();

            try
            {
                var observer = new AreaMonitorObserver();
                var observerRef = grainFactory.CreateObjectReference<IAreaMonitorObserver>(observer);
                var refreshTask = RefreshObserverTask(monitorGrain, observerId, observerRef, context.CancellationToken);

                await SendInitial(monitorGrain, responseStream, context.CancellationToken);

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    var update = await observer.Events.ReceiveAsync(context.CancellationToken);

                    if (context.CancellationToken.IsCancellationRequested)
                        break;

                    var ev = new MonitorLocationEvent
                    {
                        RegistrationNumber = update.RegistrationNumber,
                        Location = update.Location is not null
                            ? new Location
                            {
                                Latitude = update.Location.Latitude,
                                Longitude = update.Location.Longitude
                            }
                            : null
                    };

                    await responseStream.WriteAsync(ev, context.CancellationToken);
                }

                await refreshTask;
            }
            finally
            {
                await monitorGrain.Unsubscribe(observerId);
            }
        }

        private async Task SendInitial(IAreaMonitorGrain monitorGrain, IServerStreamWriter<MonitorLocationEvent> responseStream, CancellationToken token)
        {
            var vehicles = await monitorGrain.GetVehicles();

            foreach (var vehicle in vehicles.OrderBy(v => v.Key))
            {
                var ev = new MonitorLocationEvent
                {
                    RegistrationNumber = vehicle.Key,
                    Location = new Location
                    {
                        Latitude = vehicle.Value.Latitude,
                        Longitude = vehicle.Value.Longitude
                    }
                };

                await responseStream.WriteAsync(ev, token);
            }
        }

        private async Task RefreshObserverTask(IAreaMonitorGrain monitorGrain, Guid observerId, IAreaMonitorObserver observerRef, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await monitorGrain.Subscribe(observerId, observerRef);

                try
                {
                    await Task.Delay(RefreshPeriod, token);
                }
                finally
                { }
            }
        }
    }
}
