using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class MessagesRequiresAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Message");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Message",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_AuthorId",
                table: "Message",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Account_AuthorId",
                table: "Message",
                column: "AuthorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Account_AuthorId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_AuthorId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Message",
                type: "longtext",
                nullable: true);
        }
    }
}
