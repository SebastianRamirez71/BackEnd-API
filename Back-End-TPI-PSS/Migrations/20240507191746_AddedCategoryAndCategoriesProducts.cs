using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryAndCategoriesProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesProducts",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesProducts", x => new { x.CategoriesId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CategoriesProducts_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesProducts_ProductId",
                table: "CategoriesProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesProducts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
