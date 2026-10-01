using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RPG_API.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEnemyInheritanceAndSpeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Enemies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Enemies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enemies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Enemies",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Enemies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Enemies",
                columns: new[] { "Id", "Damage", "Discriminator", "Health", "MaxHealth", "Name", "RewardExperience", "RewardGold", "Speed" },
                values: new object[,]
                {
                    { 1, 8, "Skeleton", 40, 40, "Skeleton", 20, 10, 10 },
                    { 2, 6, "Goblin", 30, 30, "Goblin", 15, 15, 14 },
                    { 3, 14, "Orc", 70, 70, "Orc", 40, 30, 6 }
                });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1,
                column: "Speed",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2,
                column: "Speed",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Enemies");
        }
    }
}
