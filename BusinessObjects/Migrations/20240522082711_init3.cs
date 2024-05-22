using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Contracts_ContractId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomServices_Rooms_RoomId",
                table: "RoomServices");

            migrationBuilder.DropIndex(
                name: "IX_Order_ContractId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "RoomServices",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomServices_RoomId",
                table: "RoomServices",
                newName: "IX_RoomServices_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePrices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_OrderId",
                table: "Contracts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrices_ServiceId",
                table: "ServicePrices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Order_OrderId",
                table: "Contracts",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomServices_Order_OrderId",
                table: "RoomServices",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Order_OrderId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomServices_Order_OrderId",
                table: "RoomServices");

            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_OrderId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "RoomServices",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomServices_OrderId",
                table: "RoomServices",
                newName: "IX_RoomServices_RoomId");

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ContractId",
                table: "Order",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Contracts_ContractId",
                table: "Order",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomServices_Rooms_RoomId",
                table: "RoomServices",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
