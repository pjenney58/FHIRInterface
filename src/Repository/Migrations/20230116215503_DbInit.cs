using System;
using System.Collections.Generic;
using Hl7Harmonizer.MetaTypes.Model;
using Hl7Harmonizer.Support.Model;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hl7Harmonizer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Icd10Code = table.Column<string>(type: "text", nullable: true),
                    Icd10ShortDescription = table.Column<string>(type: "text", nullable: true),
                    Icd10LongDescription = table.Column<string>(type: "text", nullable: true),
                    SNOWMEDCode = table.Column<string>(type: "text", nullable: true),
                    SNOWMEDDescription = table.Column<string>(type: "text", nullable: true),
                    SNOMEDUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => new { x.EntityID, x.OwnerID });
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Partition = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EntityReferenceName = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MetaType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => new { x.EntityID, x.OwnerID });
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    Group = table.Column<string>(type: "text", nullable: true),
                    Team = table.Column<string>(type: "text", nullable: true),
                    ManagerName = table.Column<string>(type: "text", nullable: true),
                    AdminName = table.Column<string>(type: "text", nullable: true),
                    PaymentMethods = table.Column<DisposableList<PaymentMethod>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Customers_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoseSchedules",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    DoseScheduleName = table.Column<string>(type: "text", nullable: true),
                    DoseRepeatPattern = table.Column<string>(type: "text", nullable: true),
                    IsPrn = table.Column<bool>(type: "boolean", nullable: false),
                    AlternatingRepeatDays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseSchedules", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_DoseSchedules_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Emails_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicationId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    Schedule = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Medications_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientCares",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
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
                    HipaaChangeDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCares", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_PatientCares_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Phones_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
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
                    CustomerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerEntityID_CustomerOwnerID",
                        columns: x => new { x.CustomerEntityID, x.CustomerOwnerID },
                        principalTable: "Customers",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "CareEvents",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    CareEventStatus = table.Column<int>(type: "integer", nullable: false),
                    CareEventType = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Codes = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Patients = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Practitioners = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Locations = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Diagnoses = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Prescription = table.Column<Guid>(type: "uuid", nullable: true),
                    Treatment = table.Column<Guid>(type: "uuid", nullable: true),
                    Observations = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareEvents", x => new { x.EntityID, x.OwnerID });
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    CodingSystem = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => new { x.EntityID, x.OwnerID });
                });

            migrationBuilder.CreateTable(
                name: "ObservationsItem",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationReference = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationType = table.Column<int>(type: "integer", nullable: true),
                    CodeEntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    CodeOwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationsItem", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_ObservationsItem_Codes_CodeEntityID_CodeOwnerID",
                        columns: x => new { x.CodeEntityID, x.CodeOwnerID },
                        principalTable: "Codes",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationsItem_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactMethods",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PhoneOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    SocialMedia = table.Column<Dictionary<string, string>>(type: "hstore", nullable: false),
                    EmailEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    EmailOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    IM = table.Column<string>(type: "text", nullable: true),
                    ContactEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMethods", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_ContactMethods_Customers_CustomerEntityID_CustomerOwnerID",
                        columns: x => new { x.CustomerEntityID, x.CustomerOwnerID },
                        principalTable: "Customers",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_ContactMethods_Emails_EmailEntityID_EmailOwnerID",
                        columns: x => new { x.EmailEntityID, x.EmailOwnerID },
                        principalTable: "Emails",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_ContactMethods_Phones_PhoneEntityID_PhoneOwnerID",
                        columns: x => new { x.PhoneEntityID, x.PhoneOwnerID },
                        principalTable: "Phones",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    NameEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    NameOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Contacts_Addresses_AddressEntityID_AddressOwnerID",
                        columns: x => new { x.AddressEntityID, x.AddressOwnerID },
                        principalTable: "Addresses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Contacts_Customers_CustomerEntityID_CustomerOwnerID",
                        columns: x => new { x.CustomerEntityID, x.CustomerOwnerID },
                        principalTable: "Customers",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentLocation = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Diagnoses_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteType = table.Column<int>(type: "integer", nullable: false),
                    NoteId = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Note_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Note_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PractitionerIdentifier = table.Column<string>(type: "text", nullable: true),
                    LicensesAndQualifications = table.Column<List<KeyValuePair<string, string>>>(type: "jsonb", nullable: false),
                    PrimaryLanguage = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Practitioners_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Practitioners_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonNames",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Prefix = table.Column<List<string>>(type: "text[]", nullable: true),
                    GivenName = table.Column<List<string>>(type: "text[]", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    KnownByName = table.Column<string>(type: "text", nullable: true),
                    Suffix = table.Column<List<string>>(type: "text[]", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonNames", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_PersonNames_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonNames_Practitioners_PractitionerEntityID_Practitioner~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PrimaryPatientIdentifier = table.Column<string>(type: "text", nullable: true),
                    AlternatePatientIdentifier = table.Column<string>(type: "text", nullable: true),
                    UsePatientInfo = table.Column<bool>(type: "boolean", nullable: false),
                    NameEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    NameOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeceased = table.Column<bool>(type: "boolean", nullable: false),
                    DeceasedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    NursingStation = table.Column<string>(type: "text", nullable: true),
                    Floor = table.Column<string>(type: "text", nullable: true),
                    Room = table.Column<string>(type: "text", nullable: true),
                    Bed = table.Column<string>(type: "text", nullable: true),
                    PatientCareEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientCareOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    AdmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DischargeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PrimaryLanguage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Patients_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_PatientCares_PatientCareEntityID_PatientCareOwnerID",
                        columns: x => new { x.PatientCareEntityID, x.PatientCareOwnerID },
                        principalTable: "PatientCares",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Patients_PersonNames_NameEntityID_NameOwnerID",
                        columns: x => new { x.NameEntityID, x.NameOwnerID },
                        principalTable: "PersonNames",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Identifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdType = table.Column<string>(type: "text", nullable: true),
                    IdValue = table.Column<string>(type: "text", nullable: true),
                    IdUse = table.Column<string>(type: "text", nullable: true),
                    IdSource = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identifiers_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Identifiers_Practitioners_PractitionerEntityID_Practitioner~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LocationType = table.Column<int>(type: "integer", nullable: true),
                    Licenses = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Locations_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Locations_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Locations_Practitioners_PractitionerEntityID_PractitionerOw~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Items = table.Column<List<KeyValuePair<ObservationType, ObservationItem>>>(type: "jsonb", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Observations_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Observations_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observations_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "PatientPractitioners",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Relationship = table.Column<int>(type: "integer", nullable: false),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPractitioners", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_PatientPractitioners_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPractitioners_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_PatientPractitioners_Practitioners_PractitionerEntityID_Pra~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionName = table.Column<string>(type: "text", nullable: true),
                    RxType = table.Column<int>(type: "integer", nullable: false),
                    PriorPrescriptionName = table.Column<string>(type: "text", nullable: true),
                    ScripStatus = table.Column<int>(type: "integer", nullable: false),
                    StatusReason = table.Column<string>(type: "text", nullable: true),
                    Intent = table.Column<int>(type: "integer", nullable: false),
                    Catagory = table.Column<int>(type: "integer", nullable: false),
                    DoNotPerform = table.Column<bool>(type: "boolean", nullable: false),
                    FillingPharmacyEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    FillingPharmacyOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicationEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicationOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
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
                    DoseScheduleEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    SpecialInstructions = table.Column<List<string>>(type: "text[]", nullable: true),
                    MaximumRefills = table.Column<int>(type: "integer", nullable: false),
                    RemainingRefills = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Prescriptions_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Prescriptions_DoseSchedules_DoseScheduleEntityID_DoseSchedu~",
                        columns: x => new { x.DoseScheduleEntityID, x.DoseScheduleOwnerID },
                        principalTable: "DoseSchedules",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Prescriptions_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Locations_FillingPharmacyEntityID_FillingPhar~",
                        columns: x => new { x.FillingPharmacyEntityID, x.FillingPharmacyOwnerID },
                        principalTable: "Locations",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Prescriptions_Medications_MedicationEntityID_MedicationOwne~",
                        columns: x => new { x.MedicationEntityID, x.MedicationOwnerID },
                        principalTable: "Medications",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Prescriptions_Practitioners_PractitionerEntityID_Practition~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    EntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    TreatmentId = table.Column<string>(type: "text", nullable: true),
                    TreatmentName = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StopDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DcDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DispenseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    LongDescription = table.Column<string>(type: "text", nullable: true),
                    Sig = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientEntityID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientOwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    PractitionerEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    PractitionerOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DiagnosisOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    WrittenQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    WrittenDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => new { x.EntityID, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Treatments_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                        columns: x => new { x.DiagnosisEntityID, x.DiagnosisOwnerID },
                        principalTable: "Diagnoses",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Treatments_DoseSchedules_DoseScheduleEntityID_DoseScheduleO~",
                        columns: x => new { x.DoseScheduleEntityID, x.DoseScheduleOwnerID },
                        principalTable: "DoseSchedules",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Treatments_Entity_EntityID_OwnerID",
                        columns: x => new { x.EntityID, x.OwnerID },
                        principalTable: "Entity",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Locations_LocationEntityID_LocationOwnerID",
                        columns: x => new { x.LocationEntityID, x.LocationOwnerID },
                        principalTable: "Locations",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_Treatments_Patients_PatientEntityID_PatientOwnerID",
                        columns: x => new { x.PatientEntityID, x.PatientOwnerID },
                        principalTable: "Patients",
                        principalColumns: new[] { "EntityID", "OwnerID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Practitioners_PractitionerEntityID_PractitionerO~",
                        columns: x => new { x.PractitionerEntityID, x.PractitionerOwnerID },
                        principalTable: "Practitioners",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "DoseDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ThisDoseDay = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DoseScheduleEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleOwnerID = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    TreatmentOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoseDays_DoseSchedules_DoseScheduleEntityID_DoseScheduleOwn~",
                        columns: x => new { x.DoseScheduleEntityID, x.DoseScheduleOwnerID },
                        principalTable: "DoseSchedules",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                    table.ForeignKey(
                        name: "FK_DoseDays_Treatments_TreatmentEntityID_TreatmentOwnerID",
                        columns: x => new { x.TreatmentEntityID, x.TreatmentOwnerID },
                        principalTable: "Treatments",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateTable(
                name: "DoseEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MinmumCountOrDefault = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxmumCount = table.Column<decimal>(type: "numeric", nullable: false),
                    Instruction = table.Column<string>(type: "text", nullable: true),
                    Observation = table.Column<string>(type: "text", nullable: true),
                    DoseDayId = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleEntityID = table.Column<Guid>(type: "uuid", nullable: true),
                    DoseScheduleOwnerID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoseEvents_DoseDays_DoseDayId",
                        column: x => x.DoseDayId,
                        principalTable: "DoseDays",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoseEvents_DoseSchedules_DoseScheduleEntityID_DoseScheduleO~",
                        columns: x => new { x.DoseScheduleEntityID, x.DoseScheduleOwnerID },
                        principalTable: "DoseSchedules",
                        principalColumns: new[] { "EntityID", "OwnerID" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerEntityID_CustomerOwnerID",
                table: "Addresses",
                columns: new[] { "CustomerEntityID", "CustomerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LocationEntityID_LocationOwnerID",
                table: "Addresses",
                columns: new[] { "LocationEntityID", "LocationOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PatientEntityID_PatientOwnerID",
                table: "Addresses",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PractitionerEntityID_PractitionerOwnerID",
                table: "Addresses",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_CareEvents_DiagnosisEntityID_DiagnosisOwnerID",
                table: "CareEvents",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Codes_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Codes",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_ContactEntityID_ContactOwnerID",
                table: "ContactMethods",
                columns: new[] { "ContactEntityID", "ContactOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_CustomerEntityID_CustomerOwnerID",
                table: "ContactMethods",
                columns: new[] { "CustomerEntityID", "CustomerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_EmailEntityID_EmailOwnerID",
                table: "ContactMethods",
                columns: new[] { "EmailEntityID", "EmailOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_LocationEntityID_LocationOwnerID",
                table: "ContactMethods",
                columns: new[] { "LocationEntityID", "LocationOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_PatientEntityID_PatientOwnerID",
                table: "ContactMethods",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_PhoneEntityID_PhoneOwnerID",
                table: "ContactMethods",
                columns: new[] { "PhoneEntityID", "PhoneOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMethods_PractitionerEntityID_PractitionerOwnerID",
                table: "ContactMethods",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressEntityID_AddressOwnerID",
                table: "Contacts",
                columns: new[] { "AddressEntityID", "AddressOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CustomerEntityID_CustomerOwnerID",
                table: "Contacts",
                columns: new[] { "CustomerEntityID", "CustomerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_LocationEntityID_LocationOwnerID",
                table: "Contacts",
                columns: new[] { "LocationEntityID", "LocationOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_NameEntityID_NameOwnerID",
                table: "Contacts",
                columns: new[] { "NameEntityID", "NameOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientEntityID_PatientOwnerID",
                table: "Diagnoses",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_DoseDays_DoseScheduleEntityID_DoseScheduleOwnerID",
                table: "DoseDays",
                columns: new[] { "DoseScheduleEntityID", "DoseScheduleOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_DoseDays_TreatmentEntityID_TreatmentOwnerID",
                table: "DoseDays",
                columns: new[] { "TreatmentEntityID", "TreatmentOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvents_DoseDayId",
                table: "DoseEvents",
                column: "DoseDayId");

            migrationBuilder.CreateIndex(
                name: "IX_DoseEvents_DoseScheduleEntityID_DoseScheduleOwnerID",
                table: "DoseEvents",
                columns: new[] { "DoseScheduleEntityID", "DoseScheduleOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Identifiers_PatientEntityID_PatientOwnerID",
                table: "Identifiers",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Identifiers_PractitionerEntityID_PractitionerOwnerID",
                table: "Identifiers",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Locations",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PatientEntityID_PatientOwnerID",
                table: "Locations",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PractitionerEntityID_PractitionerOwnerID",
                table: "Locations",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Note_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Note",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Observations",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_PatientEntityID_PatientOwnerID",
                table: "Observations",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationsItem_CodeEntityID_CodeOwnerID",
                table: "ObservationsItem",
                columns: new[] { "CodeEntityID", "CodeOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioners_PatientEntityID_PatientOwnerID",
                table: "PatientPractitioners",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_PatientPractitioners_PractitionerEntityID_PractitionerOwner~",
                table: "PatientPractitioners",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NameEntityID_NameOwnerID",
                table: "Patients",
                columns: new[] { "NameEntityID", "NameOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCareEntityID_PatientCareOwnerID",
                table: "Patients",
                columns: new[] { "PatientCareEntityID", "PatientCareOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonNames_PractitionerEntityID_PractitionerOwnerID",
                table: "PersonNames",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Practitioners",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Prescriptions",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoseScheduleEntityID_DoseScheduleOwnerID",
                table: "Prescriptions",
                columns: new[] { "DoseScheduleEntityID", "DoseScheduleOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_FillingPharmacyEntityID_FillingPharmacyOwnerID",
                table: "Prescriptions",
                columns: new[] { "FillingPharmacyEntityID", "FillingPharmacyOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicationEntityID_MedicationOwnerID",
                table: "Prescriptions",
                columns: new[] { "MedicationEntityID", "MedicationOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientEntityID_PatientOwnerID",
                table: "Prescriptions",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PractitionerEntityID_PractitionerOwnerID",
                table: "Prescriptions",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Treatments",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DoseScheduleEntityID_DoseScheduleOwnerID",
                table: "Treatments",
                columns: new[] { "DoseScheduleEntityID", "DoseScheduleOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_LocationEntityID_LocationOwnerID",
                table: "Treatments",
                columns: new[] { "LocationEntityID", "LocationOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientEntityID_PatientOwnerID",
                table: "Treatments",
                columns: new[] { "PatientEntityID", "PatientOwnerID" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PractitionerEntityID_PractitionerOwnerID",
                table: "Treatments",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Locations_LocationEntityID_LocationOwnerID",
                table: "Addresses",
                columns: new[] { "LocationEntityID", "LocationOwnerID" },
                principalTable: "Locations",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Patients_PatientEntityID_PatientOwnerID",
                table: "Addresses",
                columns: new[] { "PatientEntityID", "PatientOwnerID" },
                principalTable: "Patients",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Practitioners_PractitionerEntityID_PractitionerOw~",
                table: "Addresses",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" },
                principalTable: "Practitioners",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_CareEvents_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                table: "CareEvents",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" },
                principalTable: "Diagnoses",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Diagnoses_DiagnosisEntityID_DiagnosisOwnerID",
                table: "Codes",
                columns: new[] { "DiagnosisEntityID", "DiagnosisOwnerID" },
                principalTable: "Diagnoses",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethods_Contacts_ContactEntityID_ContactOwnerID",
                table: "ContactMethods",
                columns: new[] { "ContactEntityID", "ContactOwnerID" },
                principalTable: "Contacts",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethods_Locations_LocationEntityID_LocationOwnerID",
                table: "ContactMethods",
                columns: new[] { "LocationEntityID", "LocationOwnerID" },
                principalTable: "Locations",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethods_Patients_PatientEntityID_PatientOwnerID",
                table: "ContactMethods",
                columns: new[] { "PatientEntityID", "PatientOwnerID" },
                principalTable: "Patients",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMethods_Practitioners_PractitionerEntityID_Practitio~",
                table: "ContactMethods",
                columns: new[] { "PractitionerEntityID", "PractitionerOwnerID" },
                principalTable: "Practitioners",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Locations_LocationEntityID_LocationOwnerID",
                table: "Contacts",
                columns: new[] { "LocationEntityID", "LocationOwnerID" },
                principalTable: "Locations",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_PersonNames_NameEntityID_NameOwnerID",
                table: "Contacts",
                columns: new[] { "NameEntityID", "NameOwnerID" },
                principalTable: "PersonNames",
                principalColumns: new[] { "EntityID", "OwnerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Patients_PatientEntityID_PatientOwnerID",
                table: "Diagnoses",
                columns: new[] { "PatientEntityID", "PatientOwnerID" },
                principalTable: "Patients",
                principalColumns: new[] { "EntityID", "OwnerID" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Patients_PatientEntityID_PatientOwnerID",
                table: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "CareEvents");

            migrationBuilder.DropTable(
                name: "ContactMethods");

            migrationBuilder.DropTable(
                name: "DoseEvents");

            migrationBuilder.DropTable(
                name: "Identifiers");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "ObservationsItem");

            migrationBuilder.DropTable(
                name: "PatientPractitioners");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "DoseDays");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DoseSchedules");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientCares");

            migrationBuilder.DropTable(
                name: "PersonNames");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Entity");
        }
    }
}
