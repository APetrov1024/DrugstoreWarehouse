using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugstoreWarehouse.Migrations
{
    /// <inheritdoc />
    public partial class DisableCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Products_ProductId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Warehouses_WarehouseId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Drugstores_DrugstoreId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Products_ProductId",
                table: "Batches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Warehouses_WarehouseId",
                table: "Batches",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Drugstores_DrugstoreId",
                table: "Warehouses",
                column: "DrugstoreId",
                principalTable: "Drugstores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Products_ProductId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Warehouses_WarehouseId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Drugstores_DrugstoreId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Products_ProductId",
                table: "Batches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Warehouses_WarehouseId",
                table: "Batches",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Drugstores_DrugstoreId",
                table: "Warehouses",
                column: "DrugstoreId",
                principalTable: "Drugstores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
