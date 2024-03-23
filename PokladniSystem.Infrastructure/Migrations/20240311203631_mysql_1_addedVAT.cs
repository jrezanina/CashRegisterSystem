using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokladniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mysql_1_addedVAT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "PriceVATFree");

            migrationBuilder.AddColumn<double>(
                name: "PriceSale",
                table: "Products",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceVAT",
                table: "Products",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VATRateId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VATRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VATRates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "VATRates",
                columns: new[] { "Id", "Rate" },
                values: new object[,]
                {
                    { 1, 0 },
                    { 2, 12 },
                    { 3, 21 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_VATRateId",
                table: "Products",
                column: "VATRateId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StoreId",
                table: "AspNetUsers",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Stores_StoreId",
                table: "AspNetUsers",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VATRates_VATRateId",
                table: "Products",
                column: "VATRateId",
                principalTable: "VATRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Stores_StoreId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_VATRates_VATRateId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "VATRates");

            migrationBuilder.DropIndex(
                name: "IX_Products_VATRateId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StoreId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PriceSale",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PriceVAT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VATRateId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "PriceVATFree",
                table: "Products",
                newName: "Price");
        }
    }
}
