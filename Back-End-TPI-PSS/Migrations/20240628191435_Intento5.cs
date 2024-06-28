using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class Intento5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Colors_ColorId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Colors_ColourId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Colours");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colours",
                table: "Colours",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Colours_ColorId",
                table: "OrderLines",
                column: "ColorId",
                principalTable: "Colours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Colours_ColourId",
                table: "Stocks",
                column: "ColourId",
                principalTable: "Colours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Colours_ColorId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Colours_ColourId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colours",
                table: "Colours");

            migrationBuilder.RenameTable(
                name: "Colours",
                newName: "Colors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Colors_ColorId",
                table: "OrderLines",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Colors_ColourId",
                table: "Stocks",
                column: "ColourId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
