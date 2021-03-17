using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class NavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parent",
                table: "Message");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Message",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ParentId",
                table: "Message",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentId",
                table: "Message",
                column: "ParentId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ParentId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Parent",
                table: "Message",
                type: "longtext",
                nullable: true);
        }
    }
}
