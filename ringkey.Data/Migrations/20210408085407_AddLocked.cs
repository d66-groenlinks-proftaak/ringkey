using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class AddLocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "locked",
                table: "Message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "locked",
                table: "Message");
        }
    }
}
