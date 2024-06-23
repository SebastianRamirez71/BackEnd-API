using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End_TPI_PSS.Migrations
{
    /// <inheritdoc />
    public partial class addedStatusInStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Stocks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stocks");
        }
    }
}
