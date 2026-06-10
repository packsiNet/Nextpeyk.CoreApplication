using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreApplication.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxSize",
                columns: table => new
                {
                    BoxSizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BoxLength = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    BoxWidth = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    BoxHeight = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxSize", x => x.BoxSizeId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boundary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courier",
                columns: table => new
                {
                    CourierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SupportPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupportFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SystemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EconomicCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreightCollectPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumCapacityPerDay = table.Column<int>(type: "int", nullable: false),
                    MaximumShipmentWeight = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MaximumValueOfTheShipment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumParcelsInOneOrder = table.Column<int>(type: "int", nullable: false),
                    HasCOD = table.Column<bool>(type: "bit", nullable: false),
                    HasFMCG = table.Column<bool>(type: "bit", nullable: false),
                    HasFreightCollect = table.Column<bool>(type: "bit", nullable: false),
                    HasPackaging = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courier", x => x.CourierId);
                });

            migrationBuilder.CreateTable(
                name: "FleetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FleetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Sender",
                columns: table => new
                {
                    SenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EconomicCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContractStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContractEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sender", x => x.SenderId);
                });

            migrationBuilder.CreateTable(
                name: "SlaGlobalConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BenchmarkCostPerParcel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeightSuccessRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    WeightOnTimeDelivery = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    WeightReturnRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    WeightDeliveryTime = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    WeightCost = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlaGlobalConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourierActivityTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierActivityTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierActivityTimes_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierBoxSize",
                columns: table => new
                {
                    CourierBoxSizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    BoxSizeId = table.Column<int>(type: "int", nullable: false),
                    PackagingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierBoxSize", x => x.CourierBoxSizeId);
                    table.ForeignKey(
                        name: "FK_CourierBoxSize_BoxSize_BoxSizeId",
                        column: x => x.BoxSizeId,
                        principalTable: "BoxSize",
                        principalColumn: "BoxSizeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourierBoxSize_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierDailyCapacity",
                columns: table => new
                {
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UsedCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierDailyCapacity", x => new { x.CourierId, x.Date });
                    table.ForeignKey(
                        name: "FK_CourierDailyCapacity_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    IsGroupAcceptance = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierSettings_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierSlaConfig",
                columns: table => new
                {
                    CourierSlaConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    SlaHours = table.Column<int>(type: "int", nullable: false),
                    SuccessRateMin = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    OnTimeMin = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    ReturnRateMax = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierSlaConfig", x => x.CourierSlaConfigId);
                    table.ForeignKey(
                        name: "FK_CourierSlaConfig_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierZone",
                columns: table => new
                {
                    CourierZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    ZoneType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Boundary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CenterPoint = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Radius = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierZone", x => x.CourierZoneId);
                    table.ForeignKey(
                        name: "FK_CourierZone_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourierZone_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcel",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParcelType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Length = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SenderFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenderLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenderPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SenderLatitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    SenderLongitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReceiverFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiverLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceiverLatitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    ReceiverLongitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HasCOD = table.Column<bool>(type: "bit", nullable: false),
                    HasFMCG = table.Column<bool>(type: "bit", nullable: false),
                    HasFreightCollect = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcel", x => x.ParcelId);
                    table.ForeignKey(
                        name: "FK_Parcel_Sender_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Sender",
                        principalColumn: "SenderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SenderApiCredential",
                columns: table => new
                {
                    SenderApiCredentialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApiSecretHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderApiCredential", x => x.SenderApiCredentialId);
                    table.ForeignKey(
                        name: "FK_SenderApiCredential_Sender_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Sender",
                        principalColumn: "SenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UserAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CourierId = table.Column<int>(type: "int", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserAccountId);
                    table.ForeignKey(
                        name: "FK_UserAccount_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAccount_Sender_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Sender",
                        principalColumn: "SenderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarrierSlaSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    SlaGlobalConfigId = table.Column<int>(type: "int", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAssigned = table.Column<int>(type: "int", nullable: false),
                    TotalDelivered = table.Column<int>(type: "int", nullable: false),
                    TotalOnTime = table.Column<int>(type: "int", nullable: false),
                    TotalReturned = table.Column<int>(type: "int", nullable: false),
                    AvgDeliveryHours = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    AvgCostPerParcel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SuccessRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    OnTimeDelivery = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    ReturnRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    DeliveryTimeScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    CostEfficiencyScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    SlaScore = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    HasMissingKpi = table.Column<bool>(type: "bit", nullable: false),
                    MissingKpiFlags = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrierSlaSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrierSlaSnapshot_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarrierSlaSnapshot_SlaGlobalConfig_SlaGlobalConfigId",
                        column: x => x.SlaGlobalConfigId,
                        principalTable: "SlaGlobalConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelCourier",
                columns: table => new
                {
                    ParcelCourierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcelId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    LNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CollectFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CollectLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CollectPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CollectLatitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    CollectLongitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    CollectAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DistributeFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributeLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributePhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DistributeLatitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    DistributeLongitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    DistributeAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PromisedDeliveryAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelCourier", x => x.ParcelCourierId);
                    table.ForeignKey(
                        name: "FK_ParcelCourier_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelCourier_Parcel_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcel",
                        principalColumn: "ParcelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fleets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    FleetTypeId = table.Column<int>(type: "int", nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrivingLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fleets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fleets_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fleets_FleetTypes_FleetTypeId",
                        column: x => x.FleetTypeId,
                        principalTable: "FleetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fleets_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlaAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    AlertType = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlaAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlaAlerts_CarrierSlaSnapshot_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "CarrierSlaSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlaAlerts_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParcelCost",
                columns: table => new
                {
                    ParcelCostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcelCourierId = table.Column<int>(type: "int", nullable: false),
                    PickupCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DistributionCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelCost", x => x.ParcelCostId);
                    table.ForeignKey(
                        name: "FK_ParcelCost_ParcelCourier_ParcelCourierId",
                        column: x => x.ParcelCourierId,
                        principalTable: "ParcelCourier",
                        principalColumn: "ParcelCourierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParcelCourierFleet",
                columns: table => new
                {
                    ParcelCourierFleetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcelCourierId = table.Column<int>(type: "int", nullable: false),
                    FleetId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDailySettlement = table.Column<bool>(type: "bit", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AuthType = table.Column<int>(type: "int", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PODCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReturnReason = table.Column<int>(type: "int", nullable: true),
                    ReturnNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelCourierFleet", x => x.ParcelCourierFleetId);
                    table.ForeignKey(
                        name: "FK_ParcelCourierFleet_Fleets_FleetId",
                        column: x => x.FleetId,
                        principalTable: "Fleets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParcelCourierFleet_ParcelCourier_ParcelCourierId",
                        column: x => x.ParcelCourierId,
                        principalTable: "ParcelCourier",
                        principalColumn: "ParcelCourierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParcelCourierFleetImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcelCourierFleetId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelCourierFleetImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcelCourierFleetImages_ParcelCourierFleet_ParcelCourierFleetId",
                        column: x => x.ParcelCourierFleetId,
                        principalTable: "ParcelCourierFleet",
                        principalColumn: "ParcelCourierFleetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "CreatedAt", "CreatedByUserId", "IsActive", "IsDeleted", "ModifiedByUserId", "ModifiedDateTime", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, true, false, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, true, false, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CourierManager" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, true, false, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CourierOperator" },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, true, false, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Driver" },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, true, false, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sender" }
                });

            migrationBuilder.InsertData(
                table: "SlaGlobalConfig",
                columns: new[] { "Id", "BenchmarkCostPerParcel", "CreatedAt", "CreatedByUserId", "EffectiveFrom", "WeightCost", "WeightDeliveryTime", "WeightOnTimeDelivery", "WeightReturnRate", "WeightSuccessRate" },
                values: new object[] { 1, 100000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.10m, 0.10m, 0.30m, 0.20m, 0.30m });

            migrationBuilder.CreateIndex(
                name: "IX_CarrierSlaSnapshot_CourierId_PeriodType_PeriodStart",
                table: "CarrierSlaSnapshot",
                columns: new[] { "CourierId", "PeriodType", "PeriodStart" });

            migrationBuilder.CreateIndex(
                name: "IX_CarrierSlaSnapshot_SlaGlobalConfigId",
                table: "CarrierSlaSnapshot",
                column: "SlaGlobalConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierActivityTimes_CourierId",
                table: "CourierActivityTimes",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierBoxSize_BoxSizeId",
                table: "CourierBoxSize",
                column: "BoxSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierBoxSize_CourierId",
                table: "CourierBoxSize",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierSettings_CourierId",
                table: "CourierSettings",
                column: "CourierId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourierSlaConfig_CourierId",
                table: "CourierSlaConfig",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierZone_CityId",
                table: "CourierZone",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierZone_CourierId",
                table: "CourierZone",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Fleets_CourierId",
                table: "Fleets",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Fleets_FleetTypeId",
                table: "Fleets",
                column: "FleetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fleets_UserAccountId",
                table: "Fleets",
                column: "UserAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtpCode_PhoneNumber_IsUsed",
                table: "OtpCode",
                columns: new[] { "PhoneNumber", "IsUsed" });

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_Barcode",
                table: "Parcel",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_SenderId",
                table: "Parcel",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCost_ParcelCourierId",
                table: "ParcelCost",
                column: "ParcelCourierId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCourier_CourierId",
                table: "ParcelCourier",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCourier_ParcelId",
                table: "ParcelCourier",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCourierFleet_FleetId",
                table: "ParcelCourierFleet",
                column: "FleetId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCourierFleet_ParcelCourierId",
                table: "ParcelCourierFleet",
                column: "ParcelCourierId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelCourierFleetImages_ParcelCourierFleetId",
                table: "ParcelCourierFleetImages",
                column: "ParcelCourierFleetId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SenderApiCredential_ApiKey",
                table: "SenderApiCredential",
                column: "ApiKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SenderApiCredential_SenderId",
                table: "SenderApiCredential",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SlaAlerts_CourierId",
                table: "SlaAlerts",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_SlaAlerts_SnapshotId",
                table: "SlaAlerts",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_CourierId",
                table: "UserAccount",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_SenderId",
                table: "UserAccount",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_UserName",
                table: "UserAccount",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserAccountId",
                table: "UserRole",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourierActivityTimes");

            migrationBuilder.DropTable(
                name: "CourierBoxSize");

            migrationBuilder.DropTable(
                name: "CourierDailyCapacity");

            migrationBuilder.DropTable(
                name: "CourierSettings");

            migrationBuilder.DropTable(
                name: "CourierSlaConfig");

            migrationBuilder.DropTable(
                name: "CourierZone");

            migrationBuilder.DropTable(
                name: "OtpCode");

            migrationBuilder.DropTable(
                name: "ParcelCost");

            migrationBuilder.DropTable(
                name: "ParcelCourierFleetImages");

            migrationBuilder.DropTable(
                name: "SenderApiCredential");

            migrationBuilder.DropTable(
                name: "SlaAlerts");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "BoxSize");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ParcelCourierFleet");

            migrationBuilder.DropTable(
                name: "CarrierSlaSnapshot");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Fleets");

            migrationBuilder.DropTable(
                name: "ParcelCourier");

            migrationBuilder.DropTable(
                name: "SlaGlobalConfig");

            migrationBuilder.DropTable(
                name: "FleetTypes");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "Parcel");

            migrationBuilder.DropTable(
                name: "Courier");

            migrationBuilder.DropTable(
                name: "Sender");
        }
    }
}
