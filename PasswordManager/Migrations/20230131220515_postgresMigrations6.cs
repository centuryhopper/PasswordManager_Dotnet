using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class postgresMigrations6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userId",
                table: "PasswordTableEF");

            migrationBuilder.DropIndex(
                name: "IX_PasswordTableEF_userId",
                table: "PasswordTableEF");

            migrationBuilder.AddColumn<string>(
                name: "UserModeluserId",
                table: "PasswordTableEF",
                type: "text",
                nullable: true);

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

            migrationBuilder.DropIndex(
                name: "IX_PasswordTableEF_UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.DropColumn(
                name: "UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordTableEF_userId",
                table: "PasswordTableEF",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userId",
                table: "PasswordTableEF",
                column: "userId",
                principalTable: "UserTableEF",
                principalColumn: "userId");
        }
    }
}
