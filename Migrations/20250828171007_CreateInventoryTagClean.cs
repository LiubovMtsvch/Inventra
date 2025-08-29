using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectitr.Migrations
{
    /// <inheritdoc />
    public partial class CreateInventoryTagClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryPermissions_Inventories_InventoryId1",
                table: "InventoryPermissions");

            migrationBuilder.DropIndex(
                name: "IX_InventoryPermissions_InventoryId1",
                table: "InventoryPermissions");

            migrationBuilder.DropColumn(
                name: "InventoryId1",
                table: "InventoryPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryId1",
                table: "InventoryPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryPermissions_InventoryId1",
                table: "InventoryPermissions",
                column: "InventoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryPermissions_Inventories_InventoryId1",
                table: "InventoryPermissions",
                column: "InventoryId1",
                principalTable: "Inventories",
                principalColumn: "Id");
        }
    }
}
