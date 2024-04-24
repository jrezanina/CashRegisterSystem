using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokladniSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mysql_8_renamedSuppliesQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Supplies",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Supplies",
                newName: "Amount");
        }
    }
}
