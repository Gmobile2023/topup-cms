using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class Update_Table_Product_add_IsShowOnFrontend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShowOnFrontend",
                table: "Products",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShowOnFrontend",
                table: "Products");
        }
    }
}
