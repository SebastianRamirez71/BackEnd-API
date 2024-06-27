using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class MercadoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Colours_ColourId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Sizes_SizeId",
                table: "OrderLines");

            migrationBuilder.DropIndex(
                name: "IX_OrderLines_ColourId",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "ColourId",
                table: "OrderLines");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "PreferenceId");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "OrderLines",
                newName: "OrderId1");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLines_SizeId",
                table: "OrderLines",
                newName: "IX_OrderLines_OrderId1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderLines",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderLines",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Orders_OrderId1",
                table: "OrderLines",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Orders_OrderId1",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderLines");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "PreferenceId",
                table: "Orders",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "OrderId1",
                table: "OrderLines",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLines_OrderId1",
                table: "OrderLines",
                newName: "IX_OrderLines_SizeId");

            migrationBuilder.AddColumn<int>(
                name: "ColourId",
                table: "OrderLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ColourId",
                table: "OrderLines",
                column: "ColourId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Colours_ColourId",
                table: "OrderLines",
                column: "ColourId",
                principalTable: "Colours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Sizes_SizeId",
                table: "OrderLines",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
