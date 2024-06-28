using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
