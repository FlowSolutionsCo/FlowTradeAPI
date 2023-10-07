using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTrade.Migrations
{
    /// <inheritdoc />
    public partial class addedUserRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProductionPossibilities");

            migrationBuilder.AddColumn<string>(
                name: "ProductionRequestIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductionPossibilityModelUserCompany",
                columns: table => new
                {
                    ProductionPossibilitiesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPossibilityModelUserCompany", x => new { x.ProductionPossibilitiesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProductionPossibilityModelUserCompany_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionPossibilityModelUserCompany_ProductionPossibilities_ProductionPossibilitiesId",
                        column: x => x.ProductionPossibilitiesId,
                        principalTable: "ProductionPossibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPossibilityModelUserCompany_UsersId",
                table: "ProductionPossibilityModelUserCompany",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPossibilityModelUserCompany");

            migrationBuilder.DropColumn(
                name: "ProductionRequestIds",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserProductionPossibilities",
                columns: table => new
                {
                    ProductionPossibilitiesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProductionPossibilities", x => new { x.ProductionPossibilitiesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserProductionPossibilities_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProductionPossibilities_ProductionPossibilities_ProductionPossibilitiesId",
                        column: x => x.ProductionPossibilitiesId,
                        principalTable: "ProductionPossibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProductionPossibilities_UsersId",
                table: "UserProductionPossibilities",
                column: "UsersId");
        }
    }
}
