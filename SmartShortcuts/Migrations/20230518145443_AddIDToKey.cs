using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class AddIDToKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Key",
                table: "Key");

            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "Key",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Key",
                table: "Key",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Key",
                table: "Key");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Key",
                table: "Key",
                column: "VKCode");
        }
    }
}