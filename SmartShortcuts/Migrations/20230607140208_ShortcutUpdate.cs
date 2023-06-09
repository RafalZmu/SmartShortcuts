using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class ShortcutUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Key");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Shortcuts");

            migrationBuilder.RenameColumn(
                name: "ShortcutToDisplay",
                table: "Shortcuts",
                newName: "ShortcutKeys");

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ActionShortcut",
                columns: table => new
                {
                    ActionsID = table.Column<string>(type: "TEXT", nullable: false),
                    ShortcutsID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionShortcut", x => new { x.ActionsID, x.ShortcutsID });
                    table.ForeignKey(
                        name: "FK_ActionShortcut_Actions_ActionsID",
                        column: x => x.ActionsID,
                        principalTable: "Actions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionShortcut_Shortcuts_ShortcutsID",
                        column: x => x.ShortcutsID,
                        principalTable: "Shortcuts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionShortcut_ShortcutsID",
                table: "ActionShortcut",
                column: "ShortcutsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionShortcut");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.RenameColumn(
                name: "ShortcutKeys",
                table: "Shortcuts",
                newName: "ShortcutToDisplay");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Shortcuts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Key",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    KeyName = table.Column<string>(type: "TEXT", nullable: false),
                    ShortcutID = table.Column<string>(type: "TEXT", nullable: true),
                    VKCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Key", x => x.ID);
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
    }
}
