using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataShapes.Migrations
{
    /// <inheritdoc />
    public partial class DataShapeDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "DoseSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoseScheduleName = table.Column<string>(type: "text", nullable: true),
                    DoseRepeatPattern = table.Column<string>(type: "text", nullable: true),
                    IsPrn = table.Column<bool>(type: "boolean", nullable: false),
                    AlternatingRepeatDays = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientCare",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCare", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    WorkGroup = table.Column<string>(type: "text", nullable: true),
                    Team = table.Column<string>(type: "text", nullable: true),
                    ManagerName = table.Column<string>(type: "text", nullable: true),
                    AdminName = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    CVV2 = table.Column<string>(type: "text", nullable: true),
                    ExpDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accreditation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccredidationCode = table.Column<string>(type: "text", nullable: true),
                    AccredidationName = table.Column<string>(type: "text", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accreditation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodingSystem = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specimen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecimenTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DateCollected = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Request = table.Column<List<string>>(type: "text[]", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureId = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureDescription = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Codes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Codes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Specimen_Codes_SpecimenTypeId",
                        column: x => x.SpecimenTypeId,
                        principalTable: "Codes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    NameId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contact_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneId = table.Column<Guid>(type: "uuid", nullable: true),
                    SocialMedia = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    EmailId = table.Column<Guid>(type: "uuid", nullable: true),
                    IM = table.Column<string>(type: "text", nullable: true),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactMethod_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Phone_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phone",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactMethod_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DiagnosisName = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentLocation = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteType = table.Column<int>(type: "integer", nullable: false),
                    NoteId = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiagnosisEncounter",
                columns: table => new
                {
                    DiagnosesId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncountersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisEncounter", x => new { x.DiagnosesId, x.EncountersId });
                    table.ForeignKey(
                        name: "FK_DiagnosisEncounter_Diagnoses_DiagnosesId",
                        column: x => x.DiagnosesId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoseDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ThisDoseDay = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoseDay_DoseSchedule_DoseScheduleId",
                        column: x => x.DoseScheduleId,
                        principalTable: "DoseSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DoseEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MinmumCountOrDefault = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxmumCount = table.Column<decimal>(type: "numeric", nullable: false),
                    Instruction = table.Column<string>(type: "text", nullable: true),
                    Observation = table.Column<string>(type: "text", nullable: true),
                    DoseDayId = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoseEvent_DoseDay_DoseDayId",
                        column: x => x.DoseDayId,
                        principalTable: "DoseDay",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoseEvent_DoseSchedule_DoseScheduleId",
                        column: x => x.DoseScheduleId,
                        principalTable: "DoseSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EncounterIdString = table.Column<List<string>>(type: "text[]", nullable: false),
                    EncounterReasonString = table.Column<List<string>>(type: "text[]", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PractitionerIdentifier = table.Column<string>(type: "text", nullable: true),
                    PractitionerType = table.Column<int>(type: "integer", nullable: false),
                    LicensesAndQualifications = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    PrimaryLanguage = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practitioners_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Practitioners_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Practitioners_Encounters_EncounterId1",
                        column: x => x.EncounterId1,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonName",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Prefix = table.Column<List<string>>(type: "text[]", nullable: true),
                    GivenName = table.Column<List<string>>(type: "text[]", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    KnownByName = table.Column<string>(type: "text", nullable: true),
                    Suffix = table.Column<List<string>>(type: "text[]", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonName_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrimaryPatientIdString = table.Column<string>(type: "text", nullable: true),
                    AlternatePatientIdString = table.Column<string>(type: "text", nullable: true),
                    Accounts = table.Column<List<string>>(type: "text[]", nullable: false),
                    PatientClass = table.Column<string>(type: "text", nullable: true),
                    UsePatientInfo = table.Column<bool>(type: "boolean", nullable: false),
                    NameId = table.Column<Guid>(type: "uuid", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    NursingStation = table.Column<string>(type: "text", nullable: true),
                    Floor = table.Column<string>(type: "text", nullable: true),
                    Room = table.Column<string>(type: "text", nullable: true),
                    Bed = table.Column<string>(type: "text", nullable: true),
                    PatientCareId = table.Column<Guid>(type: "uuid", nullable: true),
                    AdmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DischargeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EncounterId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_PatientCare_PatientCareId",
                        column: x => x.PatientCareId,
                        principalTable: "PatientCare",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_PersonName_NameId",
                        column: x => x.NameId,
                        principalTable: "PersonName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Identifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdType = table.Column<string>(type: "text", nullable: true),
                    IdValue = table.Column<string>(type: "text", nullable: true),
                    IdUse = table.Column<string>(type: "text", nullable: true),
                    IdSource = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identifier_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Identifier_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LocationType = table.Column<int>(type: "integer", nullable: true),
                    Licenses = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    StarRating = table.Column<int>(type: "integer", nullable: false),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    EncounterId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Observations_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Observations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientPractitioner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Relationship = table.Column<int>(type: "integer", nullable: false),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPractitioner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPractitioner_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientPractitioner_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpokenLanguage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsEnglishLanguageName = table.Column<string>(type: "text", nullable: true),
                    NativeLanguageName = table.Column<string>(type: "text", nullable: true),
                    Fluency = table.Column<int>(type: "integer", nullable: false),
                    Use = table.Column<int>(type: "integer", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpokenLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpokenLanguage_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionName = table.Column<string>(type: "text", nullable: true),
                    RxType = table.Column<int>(type: "integer", nullable: false),
                    PriorPrescriptionName = table.Column<string>(type: "text", nullable: true),
                    ScripStatus = table.Column<int>(type: "integer", nullable: false),
                    StatusReason = table.Column<string>(type: "text", nullable: true),
                    Intent = table.Column<int>(type: "integer", nullable: false),
                    Catagory = table.Column<int>(type: "integer", nullable: false),
                    DoNotPerform = table.Column<bool>(type: "boolean", nullable: false),
                    FillingPharmacyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicationId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    DoseScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    SpecialInstructions = table.Column<List<string>>(type: "text[]", nullable: true),
                    MaximumRefills = table.Column<int>(type: "integer", nullable: false),
                    RemainingRefills = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_DoseSchedule_DoseScheduleId",
                        column: x => x.DoseScheduleId,
                        principalTable: "DoseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Locations_FillingPharmacyId",
                        column: x => x.FillingPharmacyId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreatmentId = table.Column<string>(type: "text", nullable: true),
                    TreatmentName = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DcDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DispenseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    LongDescription = table.Column<string>(type: "text", nullable: true),
                    Sig = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    WrittenQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    WrittenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treatments_DoseSchedule_DoseScheduleId",
                        column: x => x.DoseScheduleId,
                        principalTable: "DoseSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treatments_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treatments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Treatments_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ObservationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationReference = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationType = table.Column<int>(type: "integer", nullable: true),
                    CodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Interpretation = table.Column<List<string>>(type: "text[]", nullable: false),
                    Notes = table.Column<List<string>>(type: "text[]", nullable: false),
                    BodySiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    BodyStructure = table.Column<string>(type: "text", nullable: true),
                    MethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecimenId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_BodySiteId",
                        column: x => x.BodySiteId,
                        principalTable: "Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_CodeId",
                        column: x => x.CodeId,
                        principalTable: "Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Codes_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationItem_Observations_ObservationId",
                        column: x => x.ObservationId,
                        principalTable: "Observations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ObservationItem_Specimen_SpecimenId",
                        column: x => x.SpecimenId,
                        principalTable: "Specimen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_LocationId",
                table: "Accreditation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_LocationId",
                table: "Address",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PatientId",
                table: "Address",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PractitionerId",
                table: "Address",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_TenantId",
                table: "Address",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_DiagnosisId",
                table: "Codes",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_EncounterId",
                table: "Codes",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_AddressId",
                table: "Contact",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_LocationId",
                table: "Contact",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_NameId",
                table: "Contact",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_TenantId",
                table: "Contact",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_ContactId",
                table: "ContactMethod",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_EmailId",
                table: "ContactMethod",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_LocationId",
                table: "ContactMethod",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PatientId",
                table: "ContactMethod",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PhoneId",
                table: "ContactMethod",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_PractitionerId",
                table: "ContactMethod",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethod_TenantId",
                table: "ContactMethod",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientId",
                table: "Diagnoses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosisEncounter_EncountersId",
                table: "DiagnosisEncounter",
                column: "EncountersId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseDay_DoseScheduleId",
                table: "DoseDay",
                column: "DoseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseDay_TreatmentId",
                table: "DoseDay",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvent_DoseDayId",
                table: "DoseEvent",
                column: "DoseDayId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvent_DoseScheduleId",
                table: "DoseEvent",
                column: "DoseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_PrescriptionId",
                table: "Encounters",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_TreatmentId",
                table: "Encounters",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifier_PatientId",
                table: "Identifier",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifier_PractitionerId",
                table: "Identifier",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_DiagnosisId",
                table: "Locations",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EncounterId",
                table: "Locations",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PatientId",
                table: "Locations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PractitionerId",
                table: "Locations",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_DiagnosisId",
                table: "Note",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_BodySiteId",
                table: "ObservationItem",
                column: "BodySiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_CodeId",
                table: "ObservationItem",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_MethodId",
                table: "ObservationItem",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_ObservationId",
                table: "ObservationItem",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationItem_SpecimenId",
                table: "ObservationItem",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_DiagnosisId",
                table: "Observations",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_EncounterId",
                table: "Observations",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_PatientId",
                table: "Observations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioner_PatientId",
                table: "PatientPractitioner",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioner_PractitionerId",
                table: "PatientPractitioner",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EncounterId",
                table: "Patients",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NameId",
                table: "Patients",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCareId",
                table: "Patients",
                column: "PatientCareId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_TenantId",
                table: "PaymentMethod",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonName_PractitionerId",
                table: "PersonName",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_DiagnosisId",
                table: "Practitioners",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_EncounterId",
                table: "Practitioners",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_EncounterId1",
                table: "Practitioners",
                column: "EncounterId1");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DiagnosisId",
                table: "Prescriptions",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoseScheduleId",
                table: "Prescriptions",
                column: "DoseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_FillingPharmacyId",
                table: "Prescriptions",
                column: "FillingPharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicationId",
                table: "Prescriptions",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PractitionerId",
                table: "Prescriptions",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_FeatureId",
                table: "Specimen",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_RoleId",
                table: "Specimen",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_SpecimenTypeId",
                table: "Specimen",
                column: "SpecimenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguage_PatientId",
                table: "SpokenLanguage",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DiagnosisId",
                table: "Treatments",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DoseScheduleId",
                table: "Treatments",
                column: "DoseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_LocationId",
                table: "Treatments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientId",
                table: "Treatments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PractitionerId",
                table: "Treatments",
                column: "PractitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accreditation_Locations_LocationId",
                table: "Accreditation",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Locations_LocationId",
                table: "Address",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Patients_PatientId",
                table: "Address",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Practitioners_PractitionerId",
                table: "Address",
                column: "PractitionerId",
                principalTable: "Practitioners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Diagnoses_DiagnosisId",
                table: "Codes",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Encounters_EncounterId",
                table: "Codes",
                column: "EncounterId",
                principalTable: "Encounters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Locations_LocationId",
                table: "Contact",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_PersonName_NameId",
                table: "Contact",
                column: "NameId",
                principalTable: "PersonName",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Locations_LocationId",
                table: "ContactMethod",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Patients_PatientId",
                table: "ContactMethod",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethod_Practitioners_PractitionerId",
                table: "ContactMethod",
                column: "PractitionerId",
                principalTable: "Practitioners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Patients_PatientId",
                table: "Diagnoses",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosisEncounter_Encounters_EncountersId",
                table: "DiagnosisEncounter",
                column: "EncountersId",
                principalTable: "Encounters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoseDay_Treatments_TreatmentId",
                table: "DoseDay",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Prescriptions_PrescriptionId",
                table: "Encounters",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Treatments_TreatmentId",
                table: "Encounters",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Locations_FillingPharmacyId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Locations_LocationId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Patients_PatientId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Patients_PatientId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Practitioners_PractitionerId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Practitioners_PractitionerId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "Accreditation");

            migrationBuilder.DropTable(
                name: "ContactMethod");

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
                name: "PatientPractitioner");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "SpokenLanguage");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "DoseDay");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Specimen");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientCare");

            migrationBuilder.DropTable(
                name: "PersonName");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "DoseSchedule");
        }
    }
}
