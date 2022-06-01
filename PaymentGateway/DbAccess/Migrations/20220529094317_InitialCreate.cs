using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaskedCardNumber = table.Column<string>(nullable: true),
                    CardHolderNames = table.Column<string>(nullable: true),
                    CardExpirationMonth = table.Column<int>(nullable: false),
                    CardExpirationYear = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
