using Microsoft.EntityFrameworkCore.Migrations;

namespace ToyStoreMVC.Migrations
{
    public partial class product_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceIncrement",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductSizes");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "PriceIncrement",
                table: "Sizes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
