using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ringkey.Data.Migrations
{
    public partial class messagerating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageRating",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: true),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRating_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRating_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageRating_AccountId",
                table: "MessageRating",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRating_MessageId",
                table: "MessageRating",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageRating");
        }
    }
}
