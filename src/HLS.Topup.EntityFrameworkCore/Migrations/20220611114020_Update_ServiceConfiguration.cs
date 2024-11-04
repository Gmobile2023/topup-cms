using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class Update_ServiceConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnableResponseWhenJustReceived",
                table: "ServiceConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderMaxWaitingTimeout",
                table: "ServiceConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderSetTransactionTimeout",
                table: "ServiceConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusResponseWhenJustReceived",
                table: "ServiceConfigurations",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingTimeResponseWhenJustReceived",
                table: "ServiceConfigurations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnableResponseWhenJustReceived",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "ProviderMaxWaitingTimeout",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "ProviderSetTransactionTimeout",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "StatusResponseWhenJustReceived",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "WaitingTimeResponseWhenJustReceived",
                table: "ServiceConfigurations");
        }
    }
}
