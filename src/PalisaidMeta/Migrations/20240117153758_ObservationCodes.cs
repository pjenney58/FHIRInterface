using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class ObservationCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "value",
                table: "TestResultValuess",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "ObservationItem",
                newName: "Value");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "UdiCarrier",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "UdiCarrier",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Treatments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Treatments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "TestResultValuess",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "TestResultValuess",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "TestResults",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "TestResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Tenants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "SpokenLanguage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "SpokenLanguage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Specimen",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Specimen",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Prescriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Prescriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Patient",
                table: "Prescriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Practitioners",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Practitioners",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Phone",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Phone",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "PersonName",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "PersonName",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "PaymentMethod",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "PaymentMethod",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Patients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "PatientPractitioner",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "PatientPractitioner",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "PatientCare",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "PatientCare",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Participant",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Participant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "AlternateId",
                table: "Observations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Observations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Observations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "ObservationItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "ObservationItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "ObservationItem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<Tuple<string, string>>>(
                name: "Values",
                table: "ObservationItem",
                type: "record[]",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Note",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Note",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Medications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Locations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Identifier",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Identifier",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Encounters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Encounters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Email",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Email",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Duration",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Duration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "DoseSchedule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "DoseSchedule",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "DoseEvent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "DoseEvent",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "DoseDay",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "DoseDay",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Diagnoses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Diagnoses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Device",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Device",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "ContactMethod",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "ContactMethod",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Contact",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Contact",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Collectors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Collectors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Codes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "ObservationEntityId",
                table: "Codes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Codes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "codesubname",
                table: "Codes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "units",
                table: "Codes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "value",
                table: "Codes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Address",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Address",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EntityKey",
                table: "Accreditation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OriginHash",
                table: "Accreditation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Codes_ObservationEntityId",
                table: "Codes",
                column: "ObservationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_Observations_ObservationEntityId",
                table: "Codes",
                column: "ObservationEntityId",
                principalTable: "Observations",
                principalColumn: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codes_Observations_ObservationEntityId",
                table: "Codes");

            migrationBuilder.DropIndex(
                name: "IX_Codes_ObservationEntityId",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "UdiCarrier");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "UdiCarrier");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "TestResultValuess");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "TestResultValuess");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "SpokenLanguage");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "SpokenLanguage");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Specimen");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Specimen");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Patient",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "PersonName");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "PersonName");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "PatientPractitioner");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "PatientPractitioner");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "PatientCare");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "PatientCare");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "AlternateId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "ObservationItem");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "ObservationItem");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "ObservationItem");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "ObservationItem");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Identifier");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Identifier");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Duration");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Duration");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "DoseSchedule");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "DoseSchedule");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "DoseEvent");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "DoseEvent");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "DoseDay");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "DoseDay");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "ContactMethod");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Collectors");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Collectors");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "ObservationEntityId",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "codesubname",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "units",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "value",
                table: "Codes");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "EntityKey",
                table: "Accreditation");

            migrationBuilder.DropColumn(
                name: "OriginHash",
                table: "Accreditation");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "TestResultValuess",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ObservationItem",
                newName: "value");
        }
    }
}
