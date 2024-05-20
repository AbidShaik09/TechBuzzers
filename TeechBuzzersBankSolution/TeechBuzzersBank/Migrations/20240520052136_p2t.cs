using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class p2t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transactionId",
                table: "payables",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_payables_transactionId",
                table: "payables",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_payables_transactions_transactionId",
                table: "payables",
                column: "transactionId",
                principalTable: "transactions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payables_transactions_transactionId",
                table: "payables");

            migrationBuilder.DropIndex(
                name: "IX_payables_transactionId",
                table: "payables");

            migrationBuilder.DropColumn(
                name: "transactionId",
                table: "payables");
        }
    }
}
