using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTrade.Migrations
{
    /// <inheritdoc />
    public partial class UserRegistrationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorizedPerson",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NIP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "REGON",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductionPossibilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPossibilities", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProductionPossibilities");

            migrationBuilder.DropTable(
                name: "ProductionPossibilities");

            migrationBuilder.DropColumn(
                name: "AuthorizedPerson",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NIP",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "REGON",
                table: "AspNetUsers");
        }
    }
}
