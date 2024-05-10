using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Roi = table.Column<int>(type: "int", nullable: false),
                    Tenure = table.Column<int>(type: "int", nullable: false),
                    Due = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoanId = table.Column<long>(type: "bigint", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    DebitId = table.Column<long>(type: "bigint", nullable: false),
                    CreditId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdhaarNumber = table.Column<long>(type: "bigint", nullable: false),
                    PANNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryAccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    accountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<float>(type: "real", nullable: false),
                    UserDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_userDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "userDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_UserDetailsId",
                table: "account",
                column: "UserDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "payables");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "userDetails");
        }
    }
}
