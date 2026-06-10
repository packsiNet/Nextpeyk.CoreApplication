using CoreApplication.Domain.Entities.Auth;
using CoreApplication.Domain.Entities.Courier;
using CoreApplication.Domain.Entities.Fleet;
using CoreApplication.Domain.Entities.Geography;
using CoreApplication.Domain.Entities.Sender;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Entities.Sla;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Auth
    DbSet<UserAccount> UserAccounts { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<OtpCode> OtpCodes { get; }

    // Courier
    DbSet<Domain.Entities.Courier.Courier> Couriers { get; }
    DbSet<CourierSetting> CourierSettings { get; }
    DbSet<CourierZone> CourierZones { get; }
    DbSet<CourierActivityTime> CourierActivityTimes { get; }
    DbSet<BoxSize> BoxSizes { get; }
    DbSet<CourierBoxSize> CourierBoxSizes { get; }
    DbSet<CourierSlaConfig> CourierSlaConfigs { get; }
    DbSet<CourierDailyCapacity> CourierDailyCapacities { get; }

    // Fleet
    DbSet<FleetType> FleetTypes { get; }
    DbSet<FleetEntity> Fleets { get; }

    // Geography
    DbSet<City> Cities { get; }

    // Shipment
    DbSet<Parcel> Parcels { get; }
    DbSet<ParcelCourier> ParcelCouriers { get; }
    DbSet<ParcelCourierFleet> ParcelCourierFleets { get; }
    DbSet<ParcelCourierFleetImage> ParcelCourierFleetImages { get; }
    DbSet<ParcelCost> ParcelCosts { get; }

    // Sender
    DbSet<Domain.Entities.Sender.Sender> Senders { get; }
    DbSet<SenderApiCredential> SenderApiCredentials { get; }

    // SLA
    DbSet<SlaGlobalConfig> SlaGlobalConfigs { get; }
    DbSet<CarrierSlaSnapshot> CarrierSlaSnapshots { get; }
    DbSet<SlaAlert> SlaAlerts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
