using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class ip4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstallmentMonth",
                table: "insurancePayables",
                newName: "InstallmentYear");

            migrationBuilder.AddColumn<string>(
                name: "UserDetailsId",
                table: "insurance",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "amountCovered",
                table: "insurance",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "claimed",
                table: "insurance",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_insurance_UserDetailsId",
                table: "insurance",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_insurance_userDetails_UserDetailsId",
                table: "insurance",
                column: "UserDetailsId",
                principalTable: "userDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_insurance_userDetails_UserDetailsId",
                table: "insurance");

            migrationBuilder.DropIndex(
                name: "IX_insurance_UserDetailsId",
                table: "insurance");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "insurance");

            migrationBuilder.DropColumn(
                name: "amountCovered",
                table: "insurance");

            migrationBuilder.DropColumn(
                name: "claimed",
                table: "insurance");

            migrationBuilder.RenameColumn(
                name: "InstallmentYear",
                table: "insurancePayables",
                newName: "InstallmentMonth");
        }
    }
}
