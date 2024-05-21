using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeechBuzzersBank.Migrations
{
    public partial class bill2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    billDetailsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    billType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    transactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bill_transactions_transactionId",
                        column: x => x.transactionId,
                        principalTable: "transactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bill_userDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "userDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bill_transactionId",
                table: "bill",
                column: "transactionId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_UserDetailsId",
                table: "bill",
                column: "UserDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill");
        }
    }
}
