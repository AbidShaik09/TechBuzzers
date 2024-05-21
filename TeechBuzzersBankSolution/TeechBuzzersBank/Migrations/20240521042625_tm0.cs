using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class tm0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "receiverClosingBalance",
                table: "transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "receiverOpeningBalance",
                table: "transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiverClosingBalance",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "receiverOpeningBalance",
                table: "transactions");
        }
    }
}
