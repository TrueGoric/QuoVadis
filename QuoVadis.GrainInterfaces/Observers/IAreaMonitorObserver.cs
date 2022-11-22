using QuoVadis.Common.ValueObjects;

namespace QuoVadis.GrainInterfaces.Observers
{
    public interface IAreaMonitorObserver : IGrainObserver
    {
        void OnUpdateLocation(string RegistrationNumber, Location location);

        void OnVehicleRemoval(string RegistrationNumber);
    }
}
