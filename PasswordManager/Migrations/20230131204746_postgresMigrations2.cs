using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class postgresMigrations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "PasswordTableEF",
                newName: "accountId");

            migrationBuilder.AddColumn<string>(
                name: "UserModeluserId",
                table: "PasswordTableEF",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "PasswordTableEF",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTableEF",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    aesKey = table.Column<string>(type: "text", nullable: true),
                    aesIV = table.Column<string>(type: "text", nullable: true),
                    refreshToken = table.Column<string>(type: "text", nullable: true),
                    tokenCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTableEF", x => x.userId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordTableEF_UserModeluserId",
                table: "PasswordTableEF",
                column: "UserModeluserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF",
                column: "UserModeluserId",
                principalTable: "UserTableEF",
                principalColumn: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.DropTable(
                name: "UserTableEF");

            migrationBuilder.DropIndex(
                name: "IX_PasswordTableEF_UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.DropColumn(
                name: "UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "PasswordTableEF",
                newName: "id");
        }
    }
}
