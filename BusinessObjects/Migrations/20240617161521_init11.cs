using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class init11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractType_TypeId",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractType",
                table: "ContractType");

            migrationBuilder.RenameTable(
                name: "ContractType",
                newName: "ContractTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractTypes",
                table: "ContractTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTypes_TypeId",
                table: "Contracts",
                column: "TypeId",
                principalTable: "ContractTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTypes_TypeId",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractTypes",
                table: "ContractTypes");

            migrationBuilder.RenameTable(
                name: "ContractTypes",
                newName: "ContractType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractType",
                table: "ContractType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractType_TypeId",
                table: "Contracts",
                column: "TypeId",
                principalTable: "ContractType",
                principalColumn: "Id");
        }
    }
}
