using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTrade.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductionRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionRequests",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestAmount = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRequests", x => x.RequestId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionRequests");
        }
    }
}
