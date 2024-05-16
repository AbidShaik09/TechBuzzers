using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class p2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoansId",
                table: "payables",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_payables_LoansId",
                table: "payables",
                column: "LoansId");

            migrationBuilder.AddForeignKey(
                name: "FK_payables_loans_LoansId",
                table: "payables",
                column: "LoansId",
                principalTable: "loans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payables_loans_LoansId",
                table: "payables");

            migrationBuilder.DropIndex(
                name: "IX_payables_LoansId",
                table: "payables");

            migrationBuilder.DropColumn(
                name: "LoansId",
                table: "payables");
        }
    }
}
