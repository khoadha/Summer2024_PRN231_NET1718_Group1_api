using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTypes_TypeId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_TypeId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "Contracts",
                newName: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTypes_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId",
                principalTable: "ContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTypes_ContractTypeId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "ContractTypeId",
                table: "Contracts",
                newName: "ContractId");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TypeId",
                table: "Contracts",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTypes_TypeId",
                table: "Contracts",
                column: "TypeId",
                principalTable: "ContractTypes",
                principalColumn: "Id");
        }
    }
}
