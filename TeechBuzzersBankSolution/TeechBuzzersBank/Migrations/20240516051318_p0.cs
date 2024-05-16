using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class p0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "loans",
                newName: "LoanType");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "loans",
                newName: "TenureAmount");

            migrationBuilder.AlterColumn<string>(
                name: "LoanId",
                table: "payables",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "dueDate",
                table: "payables",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "LoanAmount",
                table: "loans",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserDetailsId",
                table: "loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_loans_UserDetailsId",
                table: "loans",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_loans_userDetails_UserDetailsId",
                table: "loans",
                column: "UserDetailsId",
                principalTable: "userDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loans_userDetails_UserDetailsId",
                table: "loans");

            migrationBuilder.DropIndex(
                name: "IX_loans_UserDetailsId",
                table: "loans");

            migrationBuilder.DropColumn(
                name: "dueDate",
                table: "payables");

            migrationBuilder.DropColumn(
                name: "LoanAmount",
                table: "loans");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "loans");

            migrationBuilder.RenameColumn(
                name: "TenureAmount",
                table: "loans",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "LoanType",
                table: "loans",
                newName: "Type");

            migrationBuilder.AlterColumn<long>(
                name: "LoanId",
                table: "payables",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
