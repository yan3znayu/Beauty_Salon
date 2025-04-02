using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Salon.Migrations
{
    /// <inheritdoc />
    public partial class something : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specialists_Specialist_ID",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_Specialist_ID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Specialist_ID",
                table: "Services");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Specialist_ID",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Specialist_ID",
                table: "Services",
                column: "Specialist_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specialists_Specialist_ID",
                table: "Services",
                column: "Specialist_ID",
                principalTable: "Specialists",
                principalColumn: "Specialists_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
