using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class Update_Table_Product_Add_MinAmount_MaxAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxAmount",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinAmount",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinAmount",
                table: "Products");
        }
    }
}
