using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Courier;

public class Courier : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? SupportPhoneNumber { get; set; }
    public string? SupportFullName { get; set; }
    public string? Website { get; set; }
    public string? SystemName { get; set; }
    public string? EconomicCode { get; set; }
    public string? NationalCode { get; set; }
    public decimal BasePrice { get; set; }
    public decimal CodPrice { get; set; }
    public decimal FreightCollectPrice { get; set; }
    public int MaximumCapacityPerDay { get; set; }
    public decimal MaximumShipmentWeight { get; set; }
    public decimal MaximumValueOfTheShipment { get; set; }
    public int MinimumParcelsInOneOrder { get; set; }
    public bool HasCOD { get; set; }
    public bool HasFMCG { get; set; }
    public bool HasFreightCollect { get; set; }
    public bool HasPackaging { get; set; }

    public CourierSetting? Setting { get; set; }
    public ICollection<CourierZone> Zones { get; set; } = [];
    public ICollection<CourierActivityTime> ActivityTimes { get; set; } = [];
    public ICollection<CourierBoxSize> BoxSizes { get; set; } = [];
    public ICollection<CourierSlaConfig> SlaConfigs { get; set; } = [];
    public ICollection<Fleet.FleetEntity> Fleets { get; set; } = [];
    public ICollection<Auth.UserAccount> UserAccounts { get; set; } = [];
}
