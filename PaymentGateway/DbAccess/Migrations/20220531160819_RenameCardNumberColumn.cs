using Microsoft.EntityFrameworkCore.Migrations;

namespace DbAccess.Migrations
{
    public partial class RenameCardNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaskedCardNumber",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "MaskedCardNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
