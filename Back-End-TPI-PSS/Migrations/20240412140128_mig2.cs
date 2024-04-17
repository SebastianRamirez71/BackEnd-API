using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Colours",
                columns: new[] { "Id", "ColourName", "ProductId" },
                values: new object[] { 1, "Azul", null });

            migrationBuilder.InsertData(
                table: "Colours",
                columns: new[] { "Id", "ColourName", "ProductId" },
                values: new object[] { 2, "Rojo", null });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "ProductId", "SizeName" },
                values: new object[] { 1, null, "L" });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "ProductId", "SizeName" },
                values: new object[] { 2, null, "XL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colours",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Colours",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
