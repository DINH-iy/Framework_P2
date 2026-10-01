using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG_API.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMapAndTreasureRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentRoomType",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionX",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionY",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TreasureRooms",
                table: "Games",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VisitedRooms",
                table: "Games",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRoomType",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PositionY",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TreasureRooms",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitedRooms",
                table: "Games");
        }
    }
}
