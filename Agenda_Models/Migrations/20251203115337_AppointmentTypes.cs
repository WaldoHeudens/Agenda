using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda_Models.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApointmentTypes_AgendaUser_UserId",
                table: "ApointmentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ApointmentTypes_AppointmentTypeId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApointmentTypes",
                table: "ApointmentTypes");

            migrationBuilder.RenameTable(
                name: "ApointmentTypes",
                newName: "AppointmentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_ApointmentTypes_UserId",
                table: "AppointmentTypes",
                newName: "IX_AppointmentTypes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppointmentTypes",
                table: "AppointmentTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentTypes_AppointmentTypeId",
                table: "Appointments",
                column: "AppointmentTypeId",
                principalTable: "AppointmentTypes",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentTypes_AppointmentTypeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentTypes_AgendaUser_UserId",
                table: "AppointmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppointmentTypes",
                table: "AppointmentTypes");

            migrationBuilder.RenameTable(
                name: "AppointmentTypes",
                newName: "ApointmentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentTypes_UserId",
                table: "ApointmentTypes",
                newName: "IX_ApointmentTypes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApointmentTypes",
                table: "ApointmentTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApointmentTypes_AgendaUser_UserId",
                table: "ApointmentTypes",
                column: "UserId",
                principalTable: "AgendaUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ApointmentTypes_AppointmentTypeId",
                table: "Appointments",
                column: "AppointmentTypeId",
                principalTable: "ApointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
