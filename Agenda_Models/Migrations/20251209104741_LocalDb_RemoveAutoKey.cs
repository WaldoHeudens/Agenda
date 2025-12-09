using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda_Models.Migrations
{
    /// <inheritdoc />
    public partial class LocalDb_RemoveAutoKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentTypes_AppointmentTypeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentTypes_Users_UserId",
                table: "AppointmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppointmentTypes",
                table: "AppointmentTypes");

            migrationBuilder.RenameTable(
                name: "AppointmentTypes",
                newName: "LocalAppointmentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentTypes_UserId",
                table: "LocalAppointmentTypes",
                newName: "IX_LocalAppointmentTypes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalAppointmentTypes",
                table: "LocalAppointmentTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_LocalAppointmentTypes_AppointmentTypeId",
                table: "Appointments",
                column: "AppointmentTypeId",
                principalTable: "LocalAppointmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalAppointmentTypes_Users_UserId",
                table: "LocalAppointmentTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_LocalAppointmentTypes_AppointmentTypeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_LocalAppointmentTypes_Users_UserId",
                table: "LocalAppointmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalAppointmentTypes",
                table: "LocalAppointmentTypes");

            migrationBuilder.RenameTable(
                name: "LocalAppointmentTypes",
                newName: "AppointmentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_LocalAppointmentTypes_UserId",
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
                name: "FK_AppointmentTypes_Users_UserId",
                table: "AppointmentTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
