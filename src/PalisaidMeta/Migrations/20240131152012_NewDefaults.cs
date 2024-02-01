using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class NewDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "Collectors",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetName = table.Column<string>(type: "text", nullable: true),
                    TargetUrl = table.Column<string>(type: "text", nullable: true),
                    TargetUri = table.Column<string>(type: "text", nullable: true),
                    TargetIp = table.Column<string>(type: "text", nullable: true),
                    TargetPort = table.Column<string>(type: "text", nullable: true),
                    ConnectionString = table.Column<string>(type: "text", nullable: true),
                    TimerDurationMilliseconds = table.Column<decimal>(type: "numeric", nullable: false),
                    TimerDurationSeconds = table.Column<decimal>(type: "numeric", nullable: false),
                    TimerDurationMinutes = table.Column<decimal>(type: "numeric", nullable: false),
                    TimerDurationHours = table.Column<decimal>(type: "numeric", nullable: false),
                    TimerDurationDays = table.Column<decimal>(type: "numeric", nullable: false),
                    HttpHeaders = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    DataProtocolOut = table.Column<int>(type: "integer", nullable: false),
                    NetworkProtocolOut = table.Column<int>(type: "integer", nullable: false),
                    TransportPrototcolOut = table.Column<int>(type: "integer", nullable: false),
                    DataProtocolIn = table.Column<int>(type: "integer", nullable: false),
                    NetworkProtocolIn = table.Column<int>(type: "integer", nullable: false),
                    TransportPrototcolIn = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    apitoken = table.Column<string>(type: "text", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collectors", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "DoseSchedule",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoseScheduleName = table.Column<string>(type: "text", nullable: true),
                    DoseRepeatPattern = table.Column<string>(type: "text", nullable: true),
                    IsPrn = table.Column<bool>(type: "boolean", nullable: false),
                    AlternatingRepeatDays = table.Column<int>(type: "integer", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseSchedule", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Duration",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duration", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicationCode = table.Column<string>(type: "text", nullable: true),
                    IsGeneric = table.Column<bool>(type: "boolean", nullable: false),
                    IsOtc = table.Column<bool>(type: "boolean", nullable: false),
                    BrandName = table.Column<string>(type: "text", nullable: true),
                    GenericName = table.Column<string>(type: "text", nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    LotId = table.Column<string>(type: "text", nullable: true),
                    ExpireDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Strength = table.Column<string>(type: "text", nullable: true),
                    StrengthUnits = table.Column<string>(type: "text", nullable: true),
                    Route = table.Column<string>(type: "text", nullable: true),
                    Form = table.Column<string>(type: "text", nullable: true),
                    ShortVisualDescription = table.Column<string>(type: "text", nullable: true),
                    LongVisualDescription = table.Column<string>(type: "text", nullable: true),
                    SpecialInstructions = table.Column<string>(type: "text", nullable: true),
                    Schedule = table.Column<string>(type: "text", nullable: true),
                    RxCuiCode = table.Column<string>(type: "text", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "PatientCare",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CareId = table.Column<string>(type: "text", nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "text", nullable: true),
                    MedicareId = table.Column<string>(type: "text", nullable: true),
                    MedicaidId = table.Column<string>(type: "text", nullable: true),
                    PrimaryInsuranceName = table.Column<string>(type: "text", nullable: true),
                    PrimaryPolicyId = table.Column<string>(type: "text", nullable: true),
                    PrimaryGroupId = table.Column<string>(type: "text", nullable: true),
                    AlternateInsuranceName = table.Column<string>(type: "text", nullable: true),
                    AlternatePolicyId = table.Column<string>(type: "text", nullable: true),
                    AlternateGroupId = table.Column<string>(type: "text", nullable: true),
                    HipaaDataTransmitAuthentication = table.Column<bool>(type: "boolean", nullable: false),
                    CodeStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CodeStatusLimitedCodeOptions = table.Column<byte>(type: "smallint", nullable: false),
                    HipaaChangeAuthorLogin = table.Column<string>(type: "text", nullable: true),
                    HipaaChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCare", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "PersonName",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Prefix = table.Column<List<string>>(type: "text[]", nullable: true),
                    GivenName = table.Column<List<string>>(type: "text[]", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    KnownByName = table.Column<string>(type: "text", nullable: true),
                    Suffix = table.Column<List<string>>(type: "text[]", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonName", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    WorkGroup = table.Column<string>(type: "text", nullable: true),
                    Team = table.Column<string>(type: "text", nullable: true),
                    ManagerName = table.Column<string>(type: "text", nullable: true),
                    AdminName = table.Column<string>(type: "text", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ValueUnits = table.Column<string>(type: "text", nullable: true),
                    BottomRangeValue = table.Column<string>(type: "text", nullable: true),
                    TopRangeValue = table.Column<string>(type: "text", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    CVV2 = table.Column<string>(type: "text", nullable: true),
                    ExpDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accreditation",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccredidationCode = table.Column<string>(type: "text", nullable: true),
                    AccredidationName = table.Column<string>(type: "text", nullable: true),
                    LocationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accreditation", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Use = table.Column<int>(type: "integer", nullable: false),
                    AddressType = table.Column<int>(type: "integer", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Address3 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    HL7AddressCode = table.Column<string>(type: "text", nullable: false),
                    LocationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Address_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodingSystem = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Codesubname = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Units = table.Column<string>(type: "text", nullable: true),
                    System = table.Column<string>(type: "text", nullable: true),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    ObservationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Specimen",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecimenTypeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DateCollected = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Request = table.Column<List<string>>(type: "text[]", nullable: false),
                    RoleEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureDescription = table.Column<string>(type: "text", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimen", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_RoleEntityId",
                        column: x => x.RoleEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_SpecimenTypeEntityId",
                        column: x => x.SpecimenTypeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "UdiCarrier",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceIdentifier = table.Column<string>(type: "text", nullable: true),
                    Issuer = table.Column<string>(type: "text", nullable: true),
                    Juristiction = table.Column<string>(type: "text", nullable: true),
                    CarrierAIDC = table.Column<string>(type: "text", nullable: true),
                    CarrierHRF = table.Column<string>(type: "text", nullable: true),
                    EntryTypeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdiCarrier", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_UdiCarrier_Codes_EntryTypeEntityId",
                        column: x => x.EntryTypeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    NameEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Contact_Address_AddressEntityId",
                        column: x => x.AddressEntityId,
                        principalTable: "Address",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Contact_PersonName_NameEntityId",
                        column: x => x.NameEntityId,
                        principalTable: "PersonName",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Contact_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactMethod",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    SocialMedia = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    EmailEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    IM = table.Column<string>(type: "text", nullable: true),
                    ContactEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMethod", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ContactMethod_Contact_ContactEntityId",
                        column: x => x.ContactEntityId,
                        principalTable: "Contact",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Email_EmailEntityId",
                        column: x => x.EmailEntityId,
                        principalTable: "Email",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Phone_PhoneEntityId",
                        column: x => x.PhoneEntityId,
                        principalTable: "Phone",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<List<string>>(type: "text[]", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    UdiCarrierEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AvailabilityStatus = table.Column<string>(type: "text", nullable: true),
                    BiologicalSourceEvent = table.Column<string>(type: "text", nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    ManufactureDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpireyDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LotId = table.Column<string>(type: "text", nullable: true),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    DeviceName = table.Column<string>(type: "text", nullable: true),
                    DeviceTypeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    PartNumber = table.Column<string>(type: "text", nullable: true),
                    DeviceCategoryEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Versions = table.Column<List<string>>(type: "text[]", nullable: false),
                    ModeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Cycle = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Organization = table.Column<string>(type: "text", nullable: true),
                    Contact = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    DeviceCodeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Annotation = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Device_Codes_DeviceCategoryEntityId",
                        column: x => x.DeviceCategoryEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_Codes_DeviceCodeEntityId",
                        column: x => x.DeviceCodeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_Codes_DeviceTypeEntityId",
                        column: x => x.DeviceTypeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_Codes_ModeEntityId",
                        column: x => x.ModeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_Codes_StatusEntityId",
                        column: x => x.StatusEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_Duration_DurationEntityId",
                        column: x => x.DurationEntityId,
                        principalTable: "Duration",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Device_UdiCarrier_UdiCarrierEntityId",
                        column: x => x.UdiCarrierEntityId,
                        principalTable: "UdiCarrier",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiagnosisName = table.Column<string>(type: "text", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentLocation = table.Column<Guid>(type: "uuid", nullable: false),
                    DurationEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Duration_DurationEntityId",
                        column: x => x.DurationEntityId,
                        principalTable: "Duration",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteType = table.Column<int>(type: "integer", nullable: false),
                    NoteId = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Note_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "DiagnosisEncounter",
                columns: table => new
                {
                    DiagnosesEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncountersEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisEncounter", x => new { x.DiagnosesEntityId, x.EncountersEntityId });
                    table.ForeignKey(
                        name: "FK_DiagnosisEncounter_Diagnoses_DiagnosesEntityId",
                        column: x => x.DiagnosesEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoseDay",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThisDoseDay = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseDay", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_DoseDay_DoseSchedule_DoseScheduleEntityId",
                        column: x => x.DoseScheduleEntityId,
                        principalTable: "DoseSchedule",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "DoseEvent",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MinmumCountOrDefault = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxmumCount = table.Column<decimal>(type: "numeric", nullable: false),
                    Instruction = table.Column<string>(type: "text", nullable: true),
                    Observation = table.Column<string>(type: "text", nullable: true),
                    DoseDayEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseEvent", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_DoseEvent_DoseDay_DoseDayEntityId",
                        column: x => x.DoseDayEntityId,
                        principalTable: "DoseDay",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_DoseEvent_DoseSchedule_DoseScheduleEntityId",
                        column: x => x.DoseScheduleEntityId,
                        principalTable: "DoseSchedule",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncounterIdString = table.Column<List<string>>(type: "text[]", nullable: false),
                    EncounterReasonString = table.Column<List<string>>(type: "text[]", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PrescriptionEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdString = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RoleType = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Participant_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrimaryPatientIdString = table.Column<string>(type: "text", nullable: true),
                    AlternatePatientIdString = table.Column<string>(type: "text", nullable: true),
                    Accounts = table.Column<List<string>>(type: "text[]", nullable: false),
                    PatientClass = table.Column<string>(type: "text", nullable: true),
                    UsePatientInfo = table.Column<bool>(type: "boolean", nullable: false),
                    NameEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    NursingStation = table.Column<string>(type: "text", nullable: true),
                    Floor = table.Column<string>(type: "text", nullable: true),
                    Room = table.Column<string>(type: "text", nullable: true),
                    Bed = table.Column<string>(type: "text", nullable: true),
                    PatientCareEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AdmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DischargeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Patients_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Patients_PatientCare_PatientCareEntityId",
                        column: x => x.PatientCareEntityId,
                        principalTable: "PatientCare",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Patients_PersonName_NameEntityId",
                        column: x => x.NameEntityId,
                        principalTable: "PersonName",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRefering = table.Column<bool>(type: "boolean", nullable: false),
                    PractitionerIdentifier = table.Column<string>(type: "text", nullable: true),
                    PractitionerType = table.Column<int>(type: "integer", nullable: false),
                    LicensesAndQualifications = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    NameEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrimaryLanguage = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Practitioners_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Practitioners_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Practitioners_PersonName_NameEntityId",
                        column: x => x.NameEntityId,
                        principalTable: "PersonName",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlternateId = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Observations_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Observations_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Observations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "SpokenLanguage",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsEnglishLanguageName = table.Column<string>(type: "text", nullable: true),
                    NativeLanguageName = table.Column<string>(type: "text", nullable: true),
                    Fluency = table.Column<int>(type: "integer", nullable: false),
                    Use = table.Column<int>(type: "integer", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpokenLanguage", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_SpokenLanguage_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Identifier",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThisId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdType = table.Column<string>(type: "text", nullable: true),
                    IdValue = table.Column<string>(type: "text", nullable: true),
                    IdUse = table.Column<string>(type: "text", nullable: true),
                    IdSource = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifier", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Identifier_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Identifier_Practitioners_PractitionerEntityId",
                        column: x => x.PractitionerEntityId,
                        principalTable: "Practitioners",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LocationType = table.Column<int>(type: "integer", nullable: true),
                    Licenses = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    StarRating = table.Column<int>(type: "integer", nullable: false),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Locations_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Locations_Encounters_EncounterEntityId",
                        column: x => x.EncounterEntityId,
                        principalTable: "Encounters",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Locations_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Locations_Practitioners_PractitionerEntityId",
                        column: x => x.PractitionerEntityId,
                        principalTable: "Practitioners",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "PatientPractitioner",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Relationship = table.Column<int>(type: "integer", nullable: false),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPractitioner", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_PatientPractitioner_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_PatientPractitioner_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationItem",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationReference = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationType = table.Column<int>(type: "integer", nullable: true),
                    CodeEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Values = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Interpretation = table.Column<List<string>>(type: "text[]", nullable: false),
                    Notes = table.Column<List<string>>(type: "text[]", nullable: false),
                    BodySiteEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    BodyStructure = table.Column<string>(type: "text", nullable: true),
                    MethodEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecimenEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationItem", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_BodySiteEntityId",
                        column: x => x.BodySiteEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_CodeEntityId",
                        column: x => x.CodeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_MethodEntityId",
                        column: x => x.MethodEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Observations_ObservationEntityId",
                        column: x => x.ObservationEntityId,
                        principalTable: "Observations",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_ObservationItem_Specimen_SpecimenEntityId",
                        column: x => x.SpecimenEntityId,
                        principalTable: "Specimen",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionName = table.Column<string>(type: "text", nullable: true),
                    RxType = table.Column<int>(type: "integer", nullable: false),
                    PriorPrescriptionName = table.Column<string>(type: "text", nullable: true),
                    StatusReasons = table.Column<List<string>>(type: "text[]", nullable: true),
                    Catagories = table.Column<int[]>(type: "integer[]", nullable: false),
                    Intent = table.Column<int>(type: "integer", nullable: false),
                    Catagory = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DoNotPerform = table.Column<bool>(type: "boolean", nullable: false),
                    CodeEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    FillingPharmacyEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Patient = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sig = table.Column<string>(type: "text", nullable: true),
                    WrittenQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    WrittenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DispenseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DcDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RefillRequestDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RefillRequestType = table.Column<int>(type: "integer", nullable: false),
                    IsIsolated = table.Column<bool>(type: "boolean", nullable: false),
                    DoseScheduleEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    SpecialInstructions = table.Column<List<string>>(type: "text[]", nullable: true),
                    MaximumRefills = table.Column<int>(type: "integer", nullable: false),
                    RemainingRefills = table.Column<int>(type: "integer", nullable: false),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Codes_CodeEntityId",
                        column: x => x.CodeEntityId,
                        principalTable: "Codes",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_DoseSchedule_DoseScheduleEntityId",
                        column: x => x.DoseScheduleEntityId,
                        principalTable: "DoseSchedule",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Locations_FillingPharmacyEntityId",
                        column: x => x.FillingPharmacyEntityId,
                        principalTable: "Locations",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Medications_MedicationEntityId",
                        column: x => x.MedicationEntityId,
                        principalTable: "Medications",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Practitioners_PractitionerEntityId",
                        column: x => x.PractitionerEntityId,
                        principalTable: "Practitioners",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TreatmentId = table.Column<string>(type: "text", nullable: true),
                    TreatmentName = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DcDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DispenseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    LongDescription = table.Column<string>(type: "text", nullable: true),
                    Sig = table.Column<string>(type: "text", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    WrittenQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    WrittenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultTenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OriginId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Treatments_Diagnoses_DiagnosisEntityId",
                        column: x => x.DiagnosisEntityId,
                        principalTable: "Diagnoses",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Treatments_DoseSchedule_DoseScheduleEntityId",
                        column: x => x.DoseScheduleEntityId,
                        principalTable: "DoseSchedule",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Treatments_Locations_LocationEntityId",
                        column: x => x.LocationEntityId,
                        principalTable: "Locations",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Treatments_Patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "Patients",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_Treatments_Practitioners_PractitionerEntityId",
                        column: x => x.PractitionerEntityId,
                        principalTable: "Practitioners",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_LocationEntityId",
                table: "Accreditation",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_LocationEntityId",
                table: "Address",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PatientEntityId",
                table: "Address",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PractitionerEntityId",
                table: "Address",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_TenantId",
                table: "Address",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_DiagnosisEntityId",
                table: "Codes",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_EncounterEntityId",
                table: "Codes",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_ObservationEntityId",
                table: "Codes",
                column: "ObservationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_AddressEntityId",
                table: "Contact",
                column: "AddressEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_LocationEntityId",
                table: "Contact",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_NameEntityId",
                table: "Contact",
                column: "NameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_TenantId",
                table: "Contact",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_ContactEntityId",
                table: "ContactMethod",
                column: "ContactEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_EmailEntityId",
                table: "ContactMethod",
                column: "EmailEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_LocationEntityId",
                table: "ContactMethod",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PatientEntityId",
                table: "ContactMethod",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PhoneEntityId",
                table: "ContactMethod",
                column: "PhoneEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PractitionerEntityId",
                table: "ContactMethod",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_TenantId",
                table: "ContactMethod",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceCategoryEntityId",
                table: "Device",
                column: "DeviceCategoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceCodeEntityId",
                table: "Device",
                column: "DeviceCodeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceTypeEntityId",
                table: "Device",
                column: "DeviceTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DurationEntityId",
                table: "Device",
                column: "DurationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_ModeEntityId",
                table: "Device",
                column: "ModeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_PatientEntityId",
                table: "Device",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_StatusEntityId",
                table: "Device",
                column: "StatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_UdiCarrierEntityId",
                table: "Device",
                column: "UdiCarrierEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DurationEntityId",
                table: "Diagnoses",
                column: "DurationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientEntityId",
                table: "Diagnoses",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosisEncounter_EncountersEntityId",
                table: "DiagnosisEncounter",
                column: "EncountersEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseDay_DoseScheduleEntityId",
                table: "DoseDay",
                column: "DoseScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseDay_TreatmentEntityId",
                table: "DoseDay",
                column: "TreatmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvent_DoseDayEntityId",
                table: "DoseEvent",
                column: "DoseDayEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvent_DoseScheduleEntityId",
                table: "DoseEvent",
                column: "DoseScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_PrescriptionEntityId",
                table: "Encounters",
                column: "PrescriptionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_TreatmentEntityId",
                table: "Encounters",
                column: "TreatmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifier_PatientEntityId",
                table: "Identifier",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifier_PractitionerEntityId",
                table: "Identifier",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_DiagnosisEntityId",
                table: "Locations",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EncounterEntityId",
                table: "Locations",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PatientEntityId",
                table: "Locations",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PractitionerEntityId",
                table: "Locations",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_DiagnosisEntityId",
                table: "Note",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_BodySiteEntityId",
                table: "ObservationItem",
                column: "BodySiteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_CodeEntityId",
                table: "ObservationItem",
                column: "CodeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_MethodEntityId",
                table: "ObservationItem",
                column: "MethodEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_ObservationEntityId",
                table: "ObservationItem",
                column: "ObservationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_SpecimenEntityId",
                table: "ObservationItem",
                column: "SpecimenEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_DiagnosisEntityId",
                table: "Observations",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_EncounterEntityId",
                table: "Observations",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_PatientId",
                table: "Observations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_EncounterEntityId",
                table: "Participant",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioner_PatientEntityId",
                table: "PatientPractitioner",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioner_PractitionerId",
                table: "PatientPractitioner",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EncounterEntityId",
                table: "Patients",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NameEntityId",
                table: "Patients",
                column: "NameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCareEntityId",
                table: "Patients",
                column: "PatientCareEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_TenantId",
                table: "PaymentMethod",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_DiagnosisEntityId",
                table: "Practitioners",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_EncounterEntityId",
                table: "Practitioners",
                column: "EncounterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_NameEntityId",
                table: "Practitioners",
                column: "NameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_CodeEntityId",
                table: "Prescriptions",
                column: "CodeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DiagnosisEntityId",
                table: "Prescriptions",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoseScheduleEntityId",
                table: "Prescriptions",
                column: "DoseScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_FillingPharmacyEntityId",
                table: "Prescriptions",
                column: "FillingPharmacyEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicationEntityId",
                table: "Prescriptions",
                column: "MedicationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientEntityId",
                table: "Prescriptions",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PractitionerEntityId",
                table: "Prescriptions",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_FeatureEntityId",
                table: "Specimen",
                column: "FeatureEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_RoleEntityId",
                table: "Specimen",
                column: "RoleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_SpecimenTypeEntityId",
                table: "Specimen",
                column: "SpecimenTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguage_PatientEntityId",
                table: "SpokenLanguage",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DiagnosisEntityId",
                table: "Treatments",
                column: "DiagnosisEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DoseScheduleEntityId",
                table: "Treatments",
                column: "DoseScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_LocationEntityId",
                table: "Treatments",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientEntityId",
                table: "Treatments",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PractitionerEntityId",
                table: "Treatments",
                column: "PractitionerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UdiCarrier_EntryTypeEntityId",
                table: "UdiCarrier",
                column: "EntryTypeEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accreditation_Locations_LocationEntityId",
                table: "Accreditation",
                column: "LocationEntityId",
                principalTable: "Locations",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Locations_LocationEntityId",
                table: "Address",
                column: "LocationEntityId",
                principalTable: "Locations",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Patients_PatientEntityId",
                table: "Address",
                column: "PatientEntityId",
                principalTable: "Patients",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Practitioners_PractitionerEntityId",
                table: "Address",
                column: "PractitionerEntityId",
                principalTable: "Practitioners",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Diagnoses_DiagnosisEntityId",
                table: "Codes",
                column: "DiagnosisEntityId",
                principalTable: "Diagnoses",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Encounters_EncounterEntityId",
                table: "Codes",
                column: "EncounterEntityId",
                principalTable: "Encounters",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Observations_ObservationEntityId",
                table: "Codes",
                column: "ObservationEntityId",
                principalTable: "Observations",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Locations_LocationEntityId",
                table: "Contact",
                column: "LocationEntityId",
                principalTable: "Locations",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Locations_LocationEntityId",
                table: "ContactMethod",
                column: "LocationEntityId",
                principalTable: "Locations",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Patients_PatientEntityId",
                table: "ContactMethod",
                column: "PatientEntityId",
                principalTable: "Patients",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Practitioners_PractitionerEntityId",
                table: "ContactMethod",
                column: "PractitionerEntityId",
                principalTable: "Practitioners",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_Patients_PatientEntityId",
                table: "Device",
                column: "PatientEntityId",
                principalTable: "Patients",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Patients_PatientEntityId",
                table: "Diagnoses",
                column: "PatientEntityId",
                principalTable: "Patients",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosisEncounter_Encounters_EncountersEntityId",
                table: "DiagnosisEncounter",
                column: "EncountersEntityId",
                principalTable: "Encounters",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoseDay_Treatments_TreatmentEntityId",
                table: "DoseDay",
                column: "TreatmentEntityId",
                principalTable: "Treatments",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Prescriptions_PrescriptionEntityId",
                table: "Encounters",
                column: "PrescriptionEntityId",
                principalTable: "Prescriptions",
                principalColumn: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Treatments_TreatmentEntityId",
                table: "Encounters",
                column: "TreatmentEntityId",
                principalTable: "Treatments",
                principalColumn: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Locations_FillingPharmacyEntityId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Locations_LocationEntityId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Patients_PatientEntityId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Patients_PatientId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientEntityId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Patients_PatientEntityId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Practitioners_PractitionerEntityId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Practitioners_PractitionerEntityId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Diagnoses_DiagnosisEntityId",
                table: "Codes");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Diagnoses_DiagnosisEntityId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Diagnoses_DiagnosisEntityId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Diagnoses_DiagnosisEntityId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Encounters_EncounterEntityId",
                table: "Codes");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Encounters_EncounterEntityId",
                table: "Observations");

            migrationBuilder.DropTable(
                name: "Accreditation");

            migrationBuilder.DropTable(
                name: "Collectors");

            migrationBuilder.DropTable(
                name: "ContactMethod");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "DiagnosisEncounter");

            migrationBuilder.DropTable(
                name: "DoseEvent");

            migrationBuilder.DropTable(
                name: "Identifier");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "ObservationItem");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "PatientPractitioner");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "SpokenLanguage");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "UdiCarrier");

            migrationBuilder.DropTable(
                name: "DoseDay");

            migrationBuilder.DropTable(
                name: "Specimen");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientCare");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "PersonName");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Duration");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "DoseSchedule");

            migrationBuilder.DropTable(
                name: "Observations");
        }
    }
}
