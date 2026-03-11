using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectorSupport.Migrations
{
    /// <inheritdoc />
    public partial class TargetConfig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetName = table.Column<string>(type: "text", nullable: true),
                    TargetUri = table.Column<string>(type: "text", nullable: true),
                    TargetIp = table.Column<string>(type: "text", nullable: true),
                    TargetPort = table.Column<string>(type: "text", nullable: true),
                    DataProtocolOut = table.Column<int>(type: "integer", nullable: false),
                    NetworkProtocolOut = table.Column<int>(type: "integer", nullable: false),
                    TransportPrototcolOut = table.Column<int>(type: "integer", nullable: false),
                    DataProtocolIn = table.Column<int>(type: "integer", nullable: false),
                    NetworkProtocolIn = table.Column<int>(type: "integer", nullable: false),
                    TransportPrototcolIn = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Configs", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    TargetName = table.Column<string>(type: "text", nullable: true),
                    CollectorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.EntityId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}