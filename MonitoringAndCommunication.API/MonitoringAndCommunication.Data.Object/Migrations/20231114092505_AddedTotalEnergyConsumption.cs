using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoringAndCommunication.Data.Object.Migrations
{
    /// <inheritdoc />
    public partial class AddedTotalEnergyConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalEnergyConsumption",
                table: "Devices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEnergyConsumption",
                table: "Devices");
        }
    }
}
