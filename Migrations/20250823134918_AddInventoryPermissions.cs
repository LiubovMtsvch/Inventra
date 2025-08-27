using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectitr.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanComment = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    InventoryId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryPermissions_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryPermissions_Inventories_InventoryId1",
                        column: x => x.InventoryId1,
                        principalTable: "Inventories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryPermissions_InventoryId",
                table: "InventoryPermissions",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryPermissions_InventoryId1",
                table: "InventoryPermissions",
                column: "InventoryId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryPermissions");
        }
    }
}
