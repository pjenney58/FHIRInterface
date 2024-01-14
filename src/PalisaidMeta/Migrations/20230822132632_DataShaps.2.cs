using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalisaidMeta.Migrations
{
    /// <inheritdoc />
    public partial class DataShaps2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    TimerDurstionHours = table.Column<decimal>(type: "numeric", nullable: false),
                    TimerDurstionDays = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_Collectors", x => x.EntityId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collectors");
        }
    }
}
