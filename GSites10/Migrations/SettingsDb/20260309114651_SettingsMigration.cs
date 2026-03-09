using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSites.Migrations.SettingsDb
{
    /// <inheritdoc />
    public partial class SettingsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "core_Settings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_Settings", x => x.SettingKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "core_Settings");
        }
    }
}
