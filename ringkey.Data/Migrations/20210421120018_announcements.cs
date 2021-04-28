using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class announcements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Announcement",
                table: "Message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Announcement",
                table: "Message");
        }
    }
}
