using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class Admin4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ROI",
                table: "loans",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ROI",
                table: "loans");
        }
    }
}
