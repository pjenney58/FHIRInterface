using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class TestResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_TestResults_TestResultEntityId",
                table: "Note");

            migrationBuilder.DropTable(
                name: "TestResultValuess");

            migrationBuilder.DropIndex(
                name: "IX_Note_TestResultEntityId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "RequestedByPractionerId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "RunDate",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestEncounterId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestLocationId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestType",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestedPatientId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestResultEntityId",
                table: "Note");

            migrationBuilder.AddColumn<string>(
                name: "BottomRangeValue",
                table: "TestResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestEntryID",
                table: "TestResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopRangeValue",
                table: "TestResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "TestResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueUnits",
                table: "TestResults",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BottomRangeValue",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestEntryID",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TopRangeValue",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "ValueUnits",
                table: "TestResults");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDateTime",
                table: "TestResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "RequestedByPractionerId",
                table: "TestResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RunDate",
                table: "TestResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDateTime",
                table: "TestResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "TestEncounterId",
                table: "TestResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TestLocationId",
                table: "TestResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TestType",
                table: "TestResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TestedPatientId",
                table: "TestResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TestResultEntityId",
                table: "Note",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestResultValuess",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EntityKey = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OriginHash = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestResultEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    TestType = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
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
        }
    }
}
