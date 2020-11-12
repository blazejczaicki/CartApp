using Microsoft.EntityFrameworkCore.Migrations;

namespace CartApp.Migrations
{
    public partial class AddFlagToCartProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "CartProductSet",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "CartProductSet");
        }
    }
}
