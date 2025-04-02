using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Salon.Migrations
{
    /// <inheritdoc />
    public partial class DelRowAdditionFromReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Addition",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Addition",
                table: "Reservations",
                type: "text",
                nullable: true);
        }
    }
}
