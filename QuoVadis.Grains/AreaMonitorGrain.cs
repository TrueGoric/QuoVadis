using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;
using Orleans.Streams.Core;
using Orleans.Timers;
using Orleans.Utilities;
using QuoVadis.Common;
using QuoVadis.Common.ValueObjects;
using QuoVadis.Contracts.Events;
using QuoVadis.GrainInterfaces;
using QuoVadis.GrainInterfaces.Observers;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace QuoVadis.Grains
{
    [KeepAlive]
    [ImplicitStreamSubscription(Constants.AreaMonitorUpdatesStreamNamespace)]
    [ImplicitStreamSubscription(Constants.AreaMonitorRemovalsStreamNamespace)]
    public class AreaMonitorGrain : IAreaMonitorGrain, IGrainBase, IStreamSubscriptionObserver, IRemindable
    {
        #region TimedLocation helper class

        private class TimedLocation
        {
            public Location Location { get; set; }
            public DateTime LastUpdate { get; set; }

            public TimedLocation(Location location, DateTime lastUpdate)
            {
                Location = location;
                LastUpdate = lastUpdate;
            }
        }

        #endregion

        private static readonly TimeSpan observerExpiration = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan refreshTick = TimeSpan.FromMinutes(5);
        private const string refreshReminderName = "RefreshReminder";

        private readonly IGrainFactory grainFactory;
        private readonly IReminderRegistry reminderRegistry;

        private readonly Dictionary<string, TimedLocation> vehicles;
        private readonly ObserverManager<Guid, IAreaMonitorObserver> observers;

        public IGrainContext GrainContext { get; }

        public AreaMonitorGrain(
            IGrainContext grainContext, 
            IGrainFactory grainFactory, 
            IReminderRegistry reminderRegistry,
            ILogger<AreaMonitorGrain> logger)
        {
            GrainContext = grainContext;

            this.grainFactory = grainFactory;
            this.reminderRegistry = reminderRegistry;

            vehicles = new Dictionary<string, TimedLocation>();
            observers = new ObserverManager<Guid, IAreaMonitorObserver>(observerExpiration, logger);
        }

        public async Task OnActivateAsync(CancellationToken token)
        {
            await reminderRegistry.RegisterOrUpdateReminder(this.GetGrainId(), refreshReminderName, refreshTick, refreshTick);
        }

        public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
        {
            // This method is called by the runtime during grain activation
            // We manually register stream callbacks for subscribed streams
            switch (handleFactory.StreamId.GetNamespace())
            {
                case Constants.AreaMonitorUpdatesStreamNamespace:
                    var updatesHandle = handleFactory.Create<UpdateVehiclePositionEvent>();
                    await updatesHandle.ResumeAsync(OnVehicleUpdate);
                    break;

                case Constants.AreaMonitorRemovalsStreamNamespace:
                    var removalsHandle = handleFactory.Create<RemoveVehicleFromMonitorEvent>();
                    await removalsHandle.ResumeAsync(OnVehicleRemoval);
                    break;
            }
        }

        public Task<ImmutableDictionary<string, Location>> GetVehicles()
            => Task.FromResult(vehicles.ToImmutableDictionary(v => v.Key, v => v.Value.Location));

        public Task Subscribe(Guid observerId, IAreaMonitorObserver observer)
        {
            // Observers allow for simple (but unreliable) message passing to the clients
            observers.Subscribe(observerId, observer);

            return Task.CompletedTask;
        }

        public Task Unsubscribe(Guid observerId)
        {
            observers.Unsubscribe(observerId);

            return Task.CompletedTask;
        }

        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (reminderName == refreshReminderName)
                return OnVehicleRefresh();

            return Task.CompletedTask;
        }

        private async Task OnVehicleRefresh()
        {
            // Periodically check and possibly clear stale data for this area
            var now = DateTime.UtcNow;
            var staleVehicles = vehicles.Where(v => now - v.Value.LastUpdate >= refreshTick).ToList();

            var areaResolverGrain = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var thisArea = this.GetPrimaryKeyString();

            foreach (var vehicle in staleVehicles)
            {
                var vehicleGrain = grainFactory.GetGrain<IVehicleGrain>(vehicle.Key);
                var currentLocation = await vehicleGrain.GetLocation();
                var currentVehicleArea = (await areaResolverGrain.GetAreaForLocation(currentLocation)).GetPrimaryKeyString();

                if (currentVehicleArea != thisArea)
                {
                    vehicles.Remove(vehicle.Key, out _);

                    observers.Notify(o => o.OnVehicleRemoval(vehicle.Key));
                }
                else
                {
                    vehicles[vehicle.Key].Location = currentLocation;

                    observers.Notify(o => o.OnUpdateLocation(vehicle.Key, currentLocation));
                }
            }
        }

        private Task OnVehicleUpdate(UpdateVehiclePositionEvent @event, StreamSequenceToken sequenceToken)
        {
            if (string.IsNullOrEmpty(@event.RegistrationNumber))
                return Task.CompletedTask;

            if (vehicles.TryGetValue(@event.RegistrationNumber, out var timedLocation))
            {
                timedLocation.Location = @event.Location;
                timedLocation.LastUpdate = DateTime.UtcNow;
            }
            else
            {
                vehicles.Add(@event.RegistrationNumber, new TimedLocation(@event.Location, DateTime.UtcNow));
            }

            observers.Notify(o => o.OnUpdateLocation(@event.RegistrationNumber, @event.Location));

            return Task.CompletedTask;
        }

        private Task OnVehicleRemoval(RemoveVehicleFromMonitorEvent @event, StreamSequenceToken sequenceToken)
        {
            if (string.IsNullOrEmpty(@event.RegistrationNumber))
                return Task.CompletedTask;

            vehicles.Remove(@event.RegistrationNumber);

            observers.Notify(o => o.OnVehicleRemoval(@event.RegistrationNumber));

            return Task.CompletedTask;
        }
    }
}
