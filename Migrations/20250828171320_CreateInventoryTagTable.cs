using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectitr.Migrations
{
    /// <inheritdoc />
    public partial class CreateInventoryTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryTag",
                columns: table => new
                {
                    InventoriesId = table.Column<int>(nullable: false),
                    TagsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTag", x => new { x.InventoriesId, x.TagsId });

                    table.ForeignKey(
                        name: "FK_InventoryTag_Inventories_InventoriesId",
                        column: x => x.InventoriesId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_InventoryTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTag_TagsId",
                table: "InventoryTag",
                column: "TagsId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "InventoryTag");
        }

    }
}
