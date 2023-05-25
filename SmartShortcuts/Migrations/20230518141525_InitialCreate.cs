using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shortcuts",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    ShortcutToDisplay = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shortcuts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Key",
                columns: table => new
                {
                    VKCode = table.Column<string>(type: "TEXT", nullable: false),
                    KeyName = table.Column<string>(type: "TEXT", nullable: false),
                    ShortcutID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Key", x => x.VKCode);
                    table.ForeignKey(
                        name: "FK_Key_Shortcuts_ShortcutID",
                        column: x => x.ShortcutID,
                        principalTable: "Shortcuts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Key_ShortcutID",
                table: "Key",
                column: "ShortcutID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Key");

            migrationBuilder.DropTable(
                name: "Shortcuts");
        }
    }
}