using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class init13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTransactionId",
                table: "Fees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fees_PaymentTransactionId",
                table: "Fees",
                column: "PaymentTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_PaymentTransactions_PaymentTransactionId",
                table: "Fees",
                column: "PaymentTransactionId",
                principalTable: "PaymentTransactions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_PaymentTransactions_PaymentTransactionId",
                table: "Fees");

            migrationBuilder.DropIndex(
                name: "IX_Fees_PaymentTransactionId",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "Fees");
        }
    }
}
