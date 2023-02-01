using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class postgresMigrations8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "userModeluserId",
                table: "PasswordTableEF",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_userModeluserId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userId",
                table: "PasswordTableEF",
                column: "userId",
                principalTable: "UserTableEF",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
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
                newName: "userModeluserId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_userId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_userModeluserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userModeluserId",
                table: "PasswordTableEF",
                column: "userModeluserId",
                principalTable: "UserTableEF",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
