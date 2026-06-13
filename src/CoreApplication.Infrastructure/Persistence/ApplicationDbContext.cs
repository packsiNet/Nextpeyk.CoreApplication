using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Auth;
using CoreApplication.Domain.Entities.Courier;
using CoreApplication.Domain.Entities.Fleet;
using CoreApplication.Domain.Entities.Geography;
using CoreApplication.Domain.Entities.Sender;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Entities.Sla;
using CoreApplication.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    // Auth
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();

    // Courier
    public DbSet<Domain.Entities.Courier.Courier> Couriers => Set<Domain.Entities.Courier.Courier>();
    public DbSet<CourierSetting> CourierSettings => Set<CourierSetting>();
    public DbSet<CourierZone> CourierZones => Set<CourierZone>();
    public DbSet<CourierActivityTime> CourierActivityTimes => Set<CourierActivityTime>();
    public DbSet<BoxSize> BoxSizes => Set<BoxSize>();
    public DbSet<CourierBoxSize> CourierBoxSizes => Set<CourierBoxSize>();
    public DbSet<CourierSlaConfig> CourierSlaConfigs => Set<CourierSlaConfig>();
    public DbSet<CourierDailyCapacity> CourierDailyCapacities => Set<CourierDailyCapacity>();

    // Fleet
    public DbSet<FleetType> FleetTypes => Set<FleetType>();
    public DbSet<FleetEntity> Fleets => Set<FleetEntity>();

    // Geography
    public DbSet<City> Cities => Set<City>();

    // Shipment
    public DbSet<Parcel> Parcels => Set<Parcel>();
    public DbSet<ParcelCourier> ParcelCouriers => Set<ParcelCourier>();
    public DbSet<ParcelCourierFleet> ParcelCourierFleets => Set<ParcelCourierFleet>();
    public DbSet<ParcelCourierFleetImage> ParcelCourierFleetImages => Set<ParcelCourierFleetImage>();
    public DbSet<ParcelCost> ParcelCosts => Set<ParcelCost>();

    // Sender
    public DbSet<Domain.Entities.Sender.Sender> Senders => Set<Domain.Entities.Sender.Sender>();
    public DbSet<SenderApiCredential> SenderApiCredentials => Set<SenderApiCredential>();

    // SLA
    public DbSet<SlaGlobalConfig> SlaGlobalConfigs => Set<SlaGlobalConfig>();
    public DbSet<CarrierSlaSnapshot> CarrierSlaSnapshots => Set<CarrierSlaSnapshot>();
    public DbSet<SlaAlert> SlaAlerts => Set<SlaAlert>();

    // Tracking
    public DbSet<CourierWorkSession> CourierWorkSessions => Set<CourierWorkSession>();
    public DbSet<CourierLocationTrack> CourierLocationTracks => Set<CourierLocationTrack>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
