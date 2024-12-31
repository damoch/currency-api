using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyAPI.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "CurrencyRates",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "CurrencyRates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCode_Date",
                table: "CurrencyRates",
                columns: new[] { "CurrencyCode", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyCode_Date",
                table: "CurrencyRates");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CurrencyRates",
                newName: "LastUpdated");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "CurrencyRates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
