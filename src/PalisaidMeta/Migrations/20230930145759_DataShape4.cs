using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class DataShape4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "HipaaChangeDate",
                table: "PatientCare",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "TestResultEntityId",
                table: "Note",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestedPatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByPractionerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestEncounterId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestType = table.Column<int>(type: "integer", nullable: false),
                    RunDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_TestResults", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "TestResultValuess",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    TestType = table.Column<int>(type: "integer", nullable: false),
                    TestResultEntityId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_TestResultValuess", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_TestResultValuess_TestResults_TestResultEntityId",
                        column: x => x.TestResultEntityId,
                        principalTable: "TestResults",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_TestResultEntityId",
                table: "Note",
                column: "TestResultEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResultValuess_TestResultEntityId",
                table: "TestResultValuess",
                column: "TestResultEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_TestResults_TestResultEntityId",
                table: "Note",
                column: "TestResultEntityId",
                principalTable: "TestResults",
                principalColumn: "EntityId");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_TestResults_TestResultEntityId",
                table: "Note");

            migrationBuilder.DropTable(
                name: "TestResultValuess");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_Note_TestResultEntityId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "TestResultEntityId",
                table: "Note");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HipaaChangeDate",
                table: "PatientCare",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
