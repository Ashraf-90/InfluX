using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCampaignToUseProfilesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_AgencyId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_BrandId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_AspNetUsers_OpenedBy",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderApprovals_AspNetUsers_ApprovedBy",
                table: "OrderApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ServiceListings_ServiceListingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderApprovals_ApprovedBy",
                table: "OrderApprovals");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_OpenedBy",
                table: "Disputes");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Campaigns",
                newName: "BrandProfileId");

            migrationBuilder.RenameColumn(
                name: "AgencyId",
                table: "Campaigns",
                newName: "ApplicationUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_BrandId",
                table: "Campaigns",
                newName: "IX_Campaigns_BrandProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_AgencyId",
                table: "Campaigns",
                newName: "IX_Campaigns_ApplicationUserId1");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "OrderDeliverables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedByUserId",
                table: "OrderApprovals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "OrderApprovals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OpenedByUserId",
                table: "Disputes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "Disputes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgencyProfileId",
                table: "Campaigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Campaigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliverables_OrderId1",
                table: "OrderDeliverables",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderApprovals_ApprovedByUserId",
                table: "OrderApprovals",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderApprovals_OrderId1",
                table: "OrderApprovals",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_OpenedByUserId",
                table: "Disputes",
                column: "OpenedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_OrderId1",
                table: "Disputes",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_AgencyProfileId",
                table: "Campaigns",
                column: "AgencyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_ApplicationUserId",
                table: "Campaigns",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AgencyProfiles_AgencyProfileId",
                table: "Campaigns",
                column: "AgencyProfileId",
                principalTable: "AgencyProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_ApplicationUserId",
                table: "Campaigns",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_ApplicationUserId1",
                table: "Campaigns",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_BrandProfiles_BrandProfileId",
                table: "Campaigns",
                column: "BrandProfileId",
                principalTable: "BrandProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_AspNetUsers_OpenedByUserId",
                table: "Disputes",
                column: "OpenedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_Orders_OrderId1",
                table: "Disputes",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderApprovals_AspNetUsers_ApprovedByUserId",
                table: "OrderApprovals",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderApprovals_Orders_OrderId1",
                table: "OrderApprovals",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliverables_Orders_OrderId1",
                table: "OrderDeliverables",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ServiceListings_ServiceListingId",
                table: "Orders",
                column: "ServiceListingId",
                principalTable: "ServiceListings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AgencyProfiles_AgencyProfileId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_ApplicationUserId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_ApplicationUserId1",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_BrandProfiles_BrandProfileId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_AspNetUsers_OpenedByUserId",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_Orders_OrderId1",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderApprovals_AspNetUsers_ApprovedByUserId",
                table: "OrderApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderApprovals_Orders_OrderId1",
                table: "OrderApprovals");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliverables_Orders_OrderId1",
                table: "OrderDeliverables");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ServiceListings_ServiceListingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliverables_OrderId1",
                table: "OrderDeliverables");

            migrationBuilder.DropIndex(
                name: "IX_OrderApprovals_ApprovedByUserId",
                table: "OrderApprovals");

            migrationBuilder.DropIndex(
                name: "IX_OrderApprovals_OrderId1",
                table: "OrderApprovals");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_OpenedByUserId",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_OrderId1",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_AgencyProfileId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_ApplicationUserId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderDeliverables");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "OrderApprovals");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderApprovals");

            migrationBuilder.DropColumn(
                name: "OpenedByUserId",
                table: "Disputes");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Disputes");

            migrationBuilder.DropColumn(
                name: "AgencyProfileId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "BrandProfileId",
                table: "Campaigns",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId1",
                table: "Campaigns",
                newName: "AgencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_BrandProfileId",
                table: "Campaigns",
                newName: "IX_Campaigns_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_ApplicationUserId1",
                table: "Campaigns",
                newName: "IX_Campaigns_AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderApprovals_ApprovedBy",
                table: "OrderApprovals",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_OpenedBy",
                table: "Disputes",
                column: "OpenedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_AgencyId",
                table: "Campaigns",
                column: "AgencyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_BrandId",
                table: "Campaigns",
                column: "BrandId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_AspNetUsers_OpenedBy",
                table: "Disputes",
                column: "OpenedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderApprovals_AspNetUsers_ApprovedBy",
                table: "OrderApprovals",
                column: "ApprovedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ServiceListings_ServiceListingId",
                table: "Orders",
                column: "ServiceListingId",
                principalTable: "ServiceListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
