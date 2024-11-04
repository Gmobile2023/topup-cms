using Microsoft.EntityFrameworkCore.Migrations;

namespace HLS.Topup.Migrations
{
    public partial class addftpprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FolderFtp",
                table: "AbpUserProfile",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderFtp",
                table: "AbpUserProfile");
        }
    }
}
