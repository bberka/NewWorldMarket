using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogType",
                table: "Logs",
                newName: "Severity");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SuccessStatus",
                table: "Logs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "SuccessStatus",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "Severity",
                table: "Logs",
                newName: "LogType");
        }
    }
}
