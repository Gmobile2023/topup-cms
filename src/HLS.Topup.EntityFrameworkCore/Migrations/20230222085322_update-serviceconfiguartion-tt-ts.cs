using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class updateserviceconfiguartionttts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowTopupReceiverType",
                table: "ServiceConfigurations",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowTopupReceiverType",
                table: "ServiceConfigurations");
        }
    }
}
