using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Migrations
{
    /// <inheritdoc />
    public partial class postgresMigrations7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "UserModeluserId",
                table: "PasswordTableEF",
                newName: "userModeluserId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_UserModeluserId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_userModeluserId");

            migrationBuilder.AlterColumn<string>(
                name: "userModeluserId",
                table: "PasswordTableEF",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userModeluserId",
                table: "PasswordTableEF",
                column: "userModeluserId",
                principalTable: "UserTableEF",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_userModeluserId",
                table: "PasswordTableEF");

            migrationBuilder.RenameColumn(
                name: "userModeluserId",
                table: "PasswordTableEF",
                newName: "UserModeluserId");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordTableEF_userModeluserId",
                table: "PasswordTableEF",
                newName: "IX_PasswordTableEF_UserModeluserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserModeluserId",
                table: "PasswordTableEF",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "PasswordTableEF",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordTableEF_UserTableEF_UserModeluserId",
                table: "PasswordTableEF",
                column: "UserModeluserId",
                principalTable: "UserTableEF",
                principalColumn: "userId");
        }
    }
}
