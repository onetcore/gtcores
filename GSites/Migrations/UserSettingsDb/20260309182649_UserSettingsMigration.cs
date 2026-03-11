using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSites.Migrations.UserSettingsDb
{
    /// <inheritdoc />
    public partial class UserSettingsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "core_UserSettings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_UserSettings", x => new { x.UserId, x.SettingKey });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "core_UserSettings");
        }
    }
}
