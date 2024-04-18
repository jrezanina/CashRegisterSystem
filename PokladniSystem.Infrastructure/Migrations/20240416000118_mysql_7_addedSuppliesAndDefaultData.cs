using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokladniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mysql_7_addedSuppliesAndDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supplies_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Potraviny" },
                    { 2, "Drogerie" },
                    { 3, "Pečivo" },
                    { 4, "Mléčné výrobky" },
                    { 5, "Nápoje" }
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DIC", "ICO", "Name" },
                values: new object[] { "CZ70883521", "70883521", "Univerzita Tomáše Bati ve Zlíně" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BuildingNumber", "City", "Email", "Phone", "PostalCode", "Street", "Web" },
                values: new object[] { "5555", "Zlín", "podatelna@utb.cz", "+420 576 038 120", "760 01", "nám. T. G. Masaryka", "www.utb.cz" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "BuildingNumber", "City", "Email", "Phone", "PostalCode", "Street", "Web" },
                values: new object[] { 2, "4511", "Zlín", "dekanat@fai.utb.cz", "+420 576 035 221", "760 05", "Nad Stráněmi", "www.fai.utb.cz" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "EanCode", "Name", "PriceSale", "PriceVAT", "PriceVATFree", "SellerCode", "ShortName", "VATRateId" },
                values: new object[,]
                {
                    { 1, null, null, "Rohlík tukový 43g", 2.5, 1.6799999999999999, 1.5, "2207", "Rohlík tukový 43g", 2 },
                    { 2, null, null, "Chléb konzumní 1000g", 39.899999999999999, 30.800000000000001, 27.5, "2701", "Chléb konzumní 1000g", 2 },
                    { 3, null, null, "Kobliha s náplní meruňka 70g", 11.5, 7.2800000000000002, 6.5, "2003", "Kobliha s n.mer. 70g", 2 },
                    { 4, null, null, "Francouzská bageta 150g", 15.9, 11.199999999999999, 10.0, "2024", "Franc. bageta 150g", 2 },
                    { 5, null, "9788071963455", "Trvanlivé mléko plnotučné 3,5% 1l", 24.899999999999999, 17.920000000000002, 16.0, null, "T.mléko pol. 3,5% 1l", 2 },
                    { 6, null, "4014400901191", "Smetanový jogurt borůvka 150g", 15.9, 10.42, 9.3000000000000007, null, "Smet.jog.bor. 150g", 2 },
                    { 7, null, "4014400400007", "Smetanový jogurt jahoda 150g", 15.9, 10.42, 9.3000000000000007, null, "Smet.jog.jah. 150g", 2 },
                    { 8, null, "7622210606754", "Zakysaná smetana 15% 200g", 14.9, 12.880000000000001, 11.5, null, "Zak.smetana 15% 200g", 2 },
                    { 9, null, "5900259128515", "Smetana ke šlehání 31% 200g", 37.899999999999999, 28.559999999999999, 25.5, null, "Smet.ke šl. 31% 200g", 2 },
                    { 10, null, "4000512363835", "Tvaroh polotučný 250g", 32.899999999999999, 25.420000000000002, 22.699999999999999, null, "Tvaroh pol. 250g", 2 },
                    { 11, null, "8000500179864", "Eidam 30% plátky 100g", 32.899999999999999, 22.399999999999999, 20.0, null, "Eidam 30% plát. 100g", 2 },
                    { 12, null, "8594050910072", "Zubní pasta 75ml", 99.900000000000006, 67.760000000000005, 56.0, null, "Zubní pasta 75ml", 3 },
                    { 13, null, "8594003849626", "Tekuté mýdlo dezinfekční 250ml", 54.899999999999999, 39.200000000000003, 35.0, null, "Tek.mýdlo dez. 250ml", 2 },
                    { 14, null, "5053990161669", "Šampon 400ml", 109.90000000000001, 84.700000000000003, 70.0, null, "Šampon 400ml", 3 },
                    { 15, null, "54491472", "Cola 500ml", 24.899999999999999, 18.149999999999999, 15.0, null, "Cola 500ml", 3 },
                    { 16, null, "20504755", "Limonáda 500ml", 21.899999999999999, 15.73, 13.0, null, "Limonáda 500ml", 3 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 3, 1 },
                    { 3, 1, 2 },
                    { 4, 3, 2 },
                    { 5, 1, 3 },
                    { 6, 3, 3 },
                    { 7, 1, 4 },
                    { 8, 3, 4 },
                    { 9, 1, 5 },
                    { 10, 4, 5 },
                    { 11, 1, 6 },
                    { 12, 4, 6 },
                    { 13, 1, 7 },
                    { 14, 4, 7 },
                    { 15, 1, 8 },
                    { 16, 4, 8 },
                    { 17, 1, 9 },
                    { 18, 4, 9 },
                    { 19, 1, 10 },
                    { 20, 4, 10 },
                    { 21, 1, 11 },
                    { 22, 4, 11 },
                    { 23, 2, 12 },
                    { 24, 2, 13 },
                    { 25, 2, 14 },
                    { 26, 1, 15 },
                    { 27, 5, 15 },
                    { 28, 1, 16 },
                    { 29, 5, 16 }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ContactId", "Name" },
                values: new object[] { 2, 2, "Fakulta aplikované informatiky" });

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_ProductId",
                table: "Supplies",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_StoreId",
                table: "Supplies",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DIC", "ICO", "Name" },
                values: new object[] { "CZ12345678", "12345678", "Společnost" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BuildingNumber", "City", "Email", "Phone", "PostalCode", "Street", "Web" },
                values: new object[] { "1234", "Město", "email@spolecnost.cz", "+420 123 456 789", "123 45", "Ulice", "www.spolecnost.cz" });
        }
    }
}
