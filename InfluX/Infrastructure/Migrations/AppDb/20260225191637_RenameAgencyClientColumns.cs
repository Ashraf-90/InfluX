using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class RenameAgencyClientColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_AspNetUsers_AgencyId",
                table: "AgencyClients");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_AspNetUsers_BrandId",
                table: "AgencyClients");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "AgencyClients",
                newName: "BrandProfileId");

            migrationBuilder.RenameColumn(
                name: "AgencyId",
                table: "AgencyClients",
                newName: "AgencyProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyClients_BrandId",
                table: "AgencyClients",
                newName: "IX_AgencyClients_BrandProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyClients_AgencyId_BrandId",
                table: "AgencyClients",
                newName: "IX_AgencyClients_AgencyProfileId_BrandProfileId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "AgencyClients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId1",
                table: "AgencyClients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyClients_ApplicationUserId",
                table: "AgencyClients",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyClients_ApplicationUserId1",
                table: "AgencyClients",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_AgencyProfiles_AgencyProfileId",
                table: "AgencyClients",
                column: "AgencyProfileId",
                principalTable: "AgencyProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_AspNetUsers_ApplicationUserId",
                table: "AgencyClients",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_AspNetUsers_ApplicationUserId1",
                table: "AgencyClients",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_BrandProfiles_BrandProfileId",
                table: "AgencyClients",
                column: "BrandProfileId",
                principalTable: "BrandProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_AgencyProfiles_AgencyProfileId",
                table: "AgencyClients");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_AspNetUsers_ApplicationUserId",
                table: "AgencyClients");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_AspNetUsers_ApplicationUserId1",
                table: "AgencyClients");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyClients_BrandProfiles_BrandProfileId",
                table: "AgencyClients");

            migrationBuilder.DropIndex(
                name: "IX_AgencyClients_ApplicationUserId",
                table: "AgencyClients");

            migrationBuilder.DropIndex(
                name: "IX_AgencyClients_ApplicationUserId1",
                table: "AgencyClients");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AgencyClients");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "AgencyClients");

            migrationBuilder.RenameColumn(
                name: "BrandProfileId",
                table: "AgencyClients",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "AgencyProfileId",
                table: "AgencyClients",
                newName: "AgencyId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyClients_BrandProfileId",
                table: "AgencyClients",
                newName: "IX_AgencyClients_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyClients_AgencyProfileId_BrandProfileId",
                table: "AgencyClients",
                newName: "IX_AgencyClients_AgencyId_BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_AspNetUsers_AgencyId",
                table: "AgencyClients",
                column: "AgencyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyClients_AspNetUsers_BrandId",
                table: "AgencyClients",
                column: "BrandId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
