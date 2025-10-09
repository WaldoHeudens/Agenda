using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda_Cons.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "AppointmentTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#FF000000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "AppointmentTypes");
        }
    }
}
