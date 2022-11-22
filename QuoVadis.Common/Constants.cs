namespace QuoVadis.Common
{
    public static class Constants
    {
        // Persistent storage
        public const string RegularStorage = "RegularStorage";
        public const string SecureStorage = "SecureStorage"; // Disclaimer: it's not secure unless YOU secure it!

        public const string VehicleStateName = "Vehicle";
        public const string AreaStateName = "Area";
        public const string UserStateName = "User";

        // Transaction storage
        public const string TransactionalStorage = "TransactionStorage";

        public const string VehicleRentStateName = "VehicleRent";
        public const string UserRentStateName = "UserRent";
        public const string AccountStateName = "Account";

        // Streams
        public const string MemoryStreamProvider = "MemoryStreamProvider";

        public const string AreaMonitorUpdatesStreamNamespace = "AreaMonitorUpdatesStreamNamespace";
        public const string AreaMonitorRemovalsStreamNamespace = "AreaMonitorRemovalsStreamNamespace";
    }
}
