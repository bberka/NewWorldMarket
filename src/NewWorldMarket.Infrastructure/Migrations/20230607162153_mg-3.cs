using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderReports_Users_UserGuid",
                table: "OrderReports");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserGuid",
                table: "OrderReports",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReports_Users_UserGuid",
                table: "OrderReports",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderReports_Users_UserGuid",
                table: "OrderReports");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserGuid",
                table: "OrderReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReports_Users_UserGuid",
                table: "OrderReports",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
