using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class DropDefaultTenantId1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "UdiCarrier");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "SpokenLanguage");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Specimen");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "PersonName");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "PatientPractitioner");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "PatientCare");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "ObservationItem");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Identifier");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Duration");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "DoseSchedule");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "DoseEvent");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "DoseDay");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Collectors");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "DefaultTenantId",
                table: "Accreditation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "UdiCarrier",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Treatments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "TestResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Tenants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "SpokenLanguage",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Specimen",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Prescriptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Practitioners",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Phone",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "PersonName",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "PaymentMethod",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Patients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "PatientPractitioner",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "PatientCare",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Participant",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Observations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "ObservationItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Note",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Medications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Identifier",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Encounters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Email",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Duration",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "DoseSchedule",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "DoseEvent",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "DoseDay",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Diagnoses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Device",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "ContactMethod",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Contact",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Collectors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Codes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Address",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultTenantId",
                table: "Accreditation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
