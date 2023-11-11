using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTrade.Migrations
{
    /// <inheritdoc />
    public partial class addedProductionPossibilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPossibilityModelUserCompany");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductionPossibilities");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ProductionPossibilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCompanyId",
                table: "ProductionPossibilities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPossibilities_UserCompanyId",
                table: "ProductionPossibilities",
                column: "UserCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPossibilities_AspNetUsers_UserCompanyId",
                table: "ProductionPossibilities",
                column: "UserCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPossibilities_AspNetUsers_UserCompanyId",
                table: "ProductionPossibilities");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPossibilities_UserCompanyId",
                table: "ProductionPossibilities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductionPossibilities");

            migrationBuilder.DropColumn(
                name: "UserCompanyId",
                table: "ProductionPossibilities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductionPossibilities",
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
    }
}
