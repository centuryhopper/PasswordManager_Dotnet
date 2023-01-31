using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class postgresMigrations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "UserModeluserId",
                table: "PasswordTableEF",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_UserModeluserId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userId",
                table: "PasswordTableEF",
                column: "userId",
                principalTable: "UserTableEF",
                principalColumn: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "PasswordTableEF",
                newName: "UserModeluserId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_userId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_UserModeluserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF",
                column: "UserModeluserId",
                principalTable: "UserTableEF",
                principalColumn: "userId");
        }
    }
}
