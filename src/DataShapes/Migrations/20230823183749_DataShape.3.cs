using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataShapes.Migrations
{
    /// <inheritdoc />
    public partial class DataShape3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimerDurstionHours",
                table: "Collectors",
                newName: "TimerDurationHours");

            migrationBuilder.RenameColumn(
                name: "TimerDurstionDays",
                table: "Collectors",
                newName: "TimerDurationDays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimerDurationHours",
                table: "Collectors",
                newName: "TimerDurstionHours");

            migrationBuilder.RenameColumn(
                name: "TimerDurationDays",
                table: "Collectors",
                newName: "TimerDurstionDays");
        }
    }
}
