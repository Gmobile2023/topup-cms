using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HLS.Topup.Migrations
{
    public partial class updateprovider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Providers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoDeposit",
                table: "Providers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRoundRobinAccount",
                table: "Providers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MinBalance",
                table: "Providers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinBalanceToDeposit",
                table: "Providers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ParentProvider",
                table: "Providers",
                maxLength: 50,
                nullable: true);

            // migrationBuilder.CreateTable(
            //     name: "PartnerServiceConfigurations",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CreationTime = table.Column<DateTime>(nullable: false),
            //         CreatorUserId = table.Column<long>(nullable: true),
            //         LastModificationTime = table.Column<DateTime>(nullable: true),
            //         LastModifierUserId = table.Column<long>(nullable: true),
            //         TenantId = table.Column<int>(nullable: true),
            //         Name = table.Column<string>(maxLength: 255, nullable: false),
            //         Status = table.Column<byte>(nullable: false),
            //         Description = table.Column<string>(maxLength: 255, nullable: true),
            //         ServiceId = table.Column<int>(nullable: true),
            //         ProviderId = table.Column<int>(nullable: true),
            //         CategoryId = table.Column<int>(nullable: true),
            //         UserId = table.Column<long>(nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PartnerServiceConfigurations", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_PartnerServiceConfigurations_Categories_CategoryId",
            //             column: x => x.CategoryId,
            //             principalTable: "Categories",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_PartnerServiceConfigurations_Providers_ProviderId",
            //             column: x => x.ProviderId,
            //             principalTable: "Providers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_PartnerServiceConfigurations_Services_ServiceId",
            //             column: x => x.ServiceId,
            //             principalTable: "Services",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_PartnerServiceConfigurations_AbpUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AbpUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_PartnerServiceConfigurations_CategoryId",
            //     table: "PartnerServiceConfigurations",
            //     column: "CategoryId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PartnerServiceConfigurations_ProviderId",
            //     table: "PartnerServiceConfigurations",
            //     column: "ProviderId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PartnerServiceConfigurations_ServiceId",
            //     table: "PartnerServiceConfigurations",
            //     column: "ServiceId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PartnerServiceConfigurations_TenantId",
            //     table: "PartnerServiceConfigurations",
            //     column: "TenantId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PartnerServiceConfigurations_UserId",
            //     table: "PartnerServiceConfigurations",
            //     column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "PartnerServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "IsAutoDeposit",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "IsRoundRobinAccount",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "MinBalance",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "MinBalanceToDeposit",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ParentProvider",
                table: "Providers");
        }
    }
}
