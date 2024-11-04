using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class AddIsLastConfigurationServiceConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLastConfiguration",
                table: "ServiceConfigurations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLastConfiguration",
                table: "ServiceConfigurations");
        }
    }
}
