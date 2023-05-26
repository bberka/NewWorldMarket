using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorld.BiSMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompleteOrderRequests");

            migrationBuilder.CreateTable(
                name: "OrderRequests",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompletionVerifiedByRequester = table.Column<bool>(type: "bit", nullable: false),
                    IsCompletionVerifiedByOrderOwner = table.Column<bool>(type: "bit", nullable: false),
                    OrderGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRequests", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_OrderRequests_Orders_OrderGuid",
                        column: x => x.OrderGuid,
                        principalTable: "Orders",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRequests_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRequests_OrderGuid",
                table: "OrderRequests",
                column: "OrderGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRequests_UserGuid",
                table: "OrderRequests",
                column: "UserGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderRequests");

            migrationBuilder.CreateTable(
                name: "CompleteOrderRequests",
                columns: table => new
                {
                    OrderGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompletionVerifiedByOrderOwner = table.Column<bool>(type: "bit", nullable: false),
                    IsCompletionVerifiedByRequester = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteOrderRequests", x => new { x.OrderGuid, x.UserGuid });
                    table.ForeignKey(
                        name: "FK_CompleteOrderRequests_Orders_OrderGuid",
                        column: x => x.OrderGuid,
                        principalTable: "Orders",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompleteOrderRequests_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompleteOrderRequests_UserGuid",
                table: "CompleteOrderRequests",
                column: "UserGuid");
        }
    }
}
