using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyAPI.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyNameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "CurrencyRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "CurrencyRates");
        }
    }
}
