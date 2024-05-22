using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class Ip0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "insurance",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    insurancePolicyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueIdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    yearOfPurchase = table.Column<int>(type: "int", nullable: false),
                    purchaseAmount = table.Column<double>(type: "float", nullable: false),
                    valididTill = table.Column<DateTime>(type: "datetime2", nullable: true),
                    installmentAmount = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "insurancePolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsuranceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceAccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Insurancevalidity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurancePolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "insurancePayables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    transactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsuranceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstallmentMonth = table.Column<int>(type: "int", nullable: false),
                    InstallmentAmount = table.Column<float>(type: "real", nullable: false),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurancePayables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_insurancePayables_insurance_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "insurance",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_insurancePayables_transactions_transactionId",
                        column: x => x.transactionId,
                        principalTable: "transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_insurancePayables_InsuranceId",
                table: "insurancePayables",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_insurancePayables_transactionId",
                table: "insurancePayables",
                column: "transactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insurancePayables");

            migrationBuilder.DropTable(
                name: "insurancePolicies");

            migrationBuilder.DropTable(
                name: "insurance");
        }
    }
}
