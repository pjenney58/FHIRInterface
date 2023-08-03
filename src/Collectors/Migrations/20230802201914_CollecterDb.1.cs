using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collectors.Migrations
{
    /// <inheritdoc />
    public partial class CollecterDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetConfiguration",
                columns: table => new
                {
                    TargetId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TransportType = table.Column<int>(type: "integer", nullable: false),
                    TransportProtocol = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    apikey = table.Column<string>(type: "text", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetConfiguration", x => x.TargetId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetConfiguration");
        }
    }
}
