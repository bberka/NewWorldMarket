using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CfConnectingIpAddress",
                table: "OrderReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "OrderReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoteIpAddress",
                table: "OrderReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "OrderReports",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XForwardedForIpAddress",
                table: "OrderReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XRealIpAddress",
                table: "OrderReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CfConnectingIpAddress",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "RemoteIpAddress",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "XForwardedForIpAddress",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "XRealIpAddress",
                table: "OrderReports");
        }
    }
}
