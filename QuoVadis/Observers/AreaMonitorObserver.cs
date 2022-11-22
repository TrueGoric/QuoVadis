using QuoVadis.Common.ValueObjects;
using QuoVadis.GrainInterfaces.Observers;
using System.Threading.Tasks.Dataflow;

namespace QuoVadis.Observers
{
    public class AreaMonitorObserver : IAreaMonitorObserver
    {
        private readonly BufferBlock<AreaMonitorEvent> events;

        public IReceivableSourceBlock<AreaMonitorEvent> Events => events;

        public AreaMonitorObserver()
        {
            var options = new DataflowBlockOptions()
            {
                EnsureOrdered = true
            };

            events = new BufferBlock<AreaMonitorEvent>(options);
        }

        public void OnUpdateLocation(string registrationNumber, Location location)
        {
            events.Post(new AreaMonitorEvent(registrationNumber, location));
        }

        public void OnVehicleRemoval(string registrationNumber)
        {
            events.Post(new AreaMonitorEvent(registrationNumber, null));
        }
    }
}
