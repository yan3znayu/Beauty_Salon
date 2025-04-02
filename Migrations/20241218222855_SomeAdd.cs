using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Beauty_Salon.Migrations
{
    /// <inheritdoc />
    public partial class SomeAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.AddColumn<int>(
                name: "User_ID",
                table: "Favorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User_ID1",
                table: "Favorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_User_ID1",
                table: "Favorites",
                column: "User_ID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_User_ID1",
                table: "Favorites",
                column: "User_ID1",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_User_ID1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_User_ID1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "User_ID1",
                table: "Favorites");

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    History_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reservation_ID = table.Column<int>(type: "integer", nullable: false),
                    User_ID = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.History_ID);
                    table.ForeignKey(
                        name: "FK_Histories_Reservations_Reservation_ID",
                        column: x => x.Reservation_ID,
                        principalTable: "Reservations",
                        principalColumn: "Reservation_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Histories_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_Reservation_ID",
                table: "Histories",
                column: "Reservation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_User_ID",
                table: "Histories",
                column: "User_ID");
        }
    }
}
