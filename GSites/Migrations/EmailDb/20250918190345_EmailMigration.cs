using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSites.Migrations.EmailDb
{
    /// <inheritdoc />
    public partial class EmailMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "core_Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    HtmlContent = table.Column<string>(type: "TEXT", nullable: true),
                    To = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TryTimes = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ConfirmDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Result = table.Column<int>(type: "INTEGER", nullable: false),
                    HashKey = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    TextContent = table.Column<string>(type: "TEXT", nullable: true),
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "core_EmailSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SmtpServer = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    SmtpUserName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    SmtpDisplayName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    SmtpPort = table.Column<int>(type: "INTEGER", nullable: false),
                    UseSsl = table.Column<bool>(type: "INTEGER", nullable: false),
                    SmtpPassword = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_EmailSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "core_Emails");

            migrationBuilder.DropTable(
                name: "core_EmailSettings");
        }
    }
}
