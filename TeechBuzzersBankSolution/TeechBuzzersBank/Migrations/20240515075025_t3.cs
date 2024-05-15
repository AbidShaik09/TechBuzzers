using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class t3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreditUserName",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DebitUserName",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "closingBalance",
                table: "transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "openingBalance",
                table: "transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "transactionType",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditUserName",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "DebitUserName",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "closingBalance",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "openingBalance",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "transactionType",
                table: "transactions");
        }
    }
}
