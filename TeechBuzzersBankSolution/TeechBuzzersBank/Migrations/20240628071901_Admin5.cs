using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class Admin5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ROI",
                table: "loans");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "insurance",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "insurance");

            migrationBuilder.AddColumn<float>(
                name: "ROI",
                table: "loans",
                type: "real",
                nullable: true);
        }
    }
}
