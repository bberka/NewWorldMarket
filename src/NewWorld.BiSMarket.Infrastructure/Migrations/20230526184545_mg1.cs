using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorld.BiSMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    OcrTextResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OcrItemDataResult = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedAccount = table.Column<bool>(type: "bit", nullable: false),
                    DiscordId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SteamId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    InGameName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLimitedToVerifiedUsers = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedDeliveryTimeHour = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<byte>(type: "tinyint", nullable: false),
                    Attributes = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    GemId = table.Column<int>(type: "int", nullable: false),
                    IsEmptySocket = table.Column<bool>(type: "bit", nullable: false),
                    IsGemChangeable = table.Column<bool>(type: "bit", nullable: false),
                    Perks = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    IsNamed = table.Column<bool>(type: "bit", nullable: false),
                    Rarity = table.Column<int>(type: "int", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    GearScore = table.Column<int>(type: "int", nullable: false),
                    LevelRequirement = table.Column<int>(type: "int", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Orders_Image_ImageGuid",
                        column: x => x.ImageGuid,
                        principalTable: "Image",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "CompleteOrderRequests",
                columns: table => new
                {
                    OrderGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompletionVerifiedByRequester = table.Column<bool>(type: "bit", nullable: false),
                    IsCompletionVerifiedByOrderOwner = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ImageGuid",
                table: "Orders",
                column: "ImageGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserGuid",
                table: "Orders",
                column: "UserGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompleteOrderRequests");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
