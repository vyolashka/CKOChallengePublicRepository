using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbAccess.Migrations
{
    public partial class AddBankSimulatorReferenceNumberToPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankSimlatorPaymentReferenceNumber",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankSimlatorPaymentReferenceNumber",
                table: "Payments");
        }
    }
}
