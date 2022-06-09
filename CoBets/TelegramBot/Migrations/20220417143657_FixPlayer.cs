using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramBot.Migrations
{
    public partial class FixPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "players",
                newName: "Number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "players",
                newName: "Age");
        }
    }
}
