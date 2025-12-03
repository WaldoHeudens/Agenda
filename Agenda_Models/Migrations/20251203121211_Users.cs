using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda_Models.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaUser_Language_LanguageCode",
                table: "AgendaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AgendaUser_UserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentTypes_AgendaUser_UserId",
                table: "AppointmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgendaUser",
                table: "AgendaUser");

            migrationBuilder.RenameTable(
                name: "AgendaUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_AgendaUser_LanguageCode",
                table: "Users",
                newName: "IX_Users_LanguageCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentTypes_Users_UserId",
                table: "AppointmentTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Language_LanguageCode",
                table: "Users",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentTypes_Users_UserId",
                table: "AppointmentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Language_LanguageCode",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AgendaUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LanguageCode",
                table: "AgendaUser",
                newName: "IX_AgendaUser_LanguageCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgendaUser",
                table: "AgendaUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaUser_Language_LanguageCode",
                table: "AgendaUser",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AgendaUser_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "AgendaUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentTypes_AgendaUser_UserId",
                table: "AppointmentTypes",
                column: "UserId",
                principalTable: "AgendaUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
