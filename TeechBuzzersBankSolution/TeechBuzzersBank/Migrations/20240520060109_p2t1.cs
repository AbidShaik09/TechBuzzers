using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class p2t1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoansId",
                table: "transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "paidTenures",
                table: "loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_LoansId",
                table: "transactions",
                column: "LoansId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_loans_LoansId",
                table: "transactions",
                column: "LoansId",
                principalTable: "loans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_loans_LoansId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_LoansId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "LoansId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "paidTenures",
                table: "loans");
        }
    }
}
