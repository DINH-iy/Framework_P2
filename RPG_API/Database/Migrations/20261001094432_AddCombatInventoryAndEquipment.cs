using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG_API.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCombatInventoryAndEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEquipped",
                table: "PlayerItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Type", "Value" },
                values: new object[] { 2, "Iron Sword", 3, 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "IsEquipped",
                table: "PlayerItems");
        }
    }
}
