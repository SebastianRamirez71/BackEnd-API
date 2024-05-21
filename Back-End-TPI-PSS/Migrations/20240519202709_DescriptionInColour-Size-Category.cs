using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionInColourSizeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeName",
                table: "Sizes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ColourName",
                table: "Colours",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sizes",
                newName: "SizeName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Colours",
                newName: "ColourName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "CategoryName");
        }
    }
}
