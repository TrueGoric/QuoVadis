using QuoVadis.Common.ValueObjects;

namespace QuoVadis.Contracts;

[GenerateSerializer]
public record VehicleInfo(
    string Model,
    string RegistrationNumber,
    decimal CostPerKilometer,
    Location Location);