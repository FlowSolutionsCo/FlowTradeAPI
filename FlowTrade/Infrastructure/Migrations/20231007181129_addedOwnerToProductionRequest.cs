using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTrade.Migrations
{
    /// <inheritdoc />
    public partial class addedOwnerToProductionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUsername",
                table: "ProductionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUsername",
                table: "ProductionRequests");
        }
    }
}
