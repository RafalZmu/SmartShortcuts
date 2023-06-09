using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShortcuts.Migrations
{
    /// <inheritdoc />
    public partial class updateActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionShortcut");

            migrationBuilder.AddColumn<string>(
                name: "ShortcutID",
                table: "Actions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ShortcutID",
                table: "Actions",
                column: "ShortcutID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Shortcuts_ShortcutID",
                table: "Actions",
                column: "ShortcutID",
                principalTable: "Shortcuts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Shortcuts_ShortcutID",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Actions_ShortcutID",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "ShortcutID",
                table: "Actions");

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
    }
}
