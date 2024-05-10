using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserDetailsId",
                table: "account",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_UserDetailsId",
                table: "account",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_account_userDetails_UserDetailsId",
                table: "account",
                column: "UserDetailsId",
                principalTable: "userDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_userDetails_UserDetailsId",
                table: "account");

            migrationBuilder.DropIndex(
                name: "IX_account_UserDetailsId",
                table: "account");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "account");
        }
    }
}
