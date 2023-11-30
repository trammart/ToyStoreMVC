using Microsoft.EntityFrameworkCore.Migrations;

namespace ToyStoreMVC.Migrations
{
    public partial class Size_Edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PriceIncrement",
                table: "Sizes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceIncrement",
                table: "Sizes");
        }
    }
}
