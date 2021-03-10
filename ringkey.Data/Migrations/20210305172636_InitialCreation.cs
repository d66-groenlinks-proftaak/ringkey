using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    Author = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Parent = table.Column<string>(type: "longtext", nullable: true),
                    Created = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
