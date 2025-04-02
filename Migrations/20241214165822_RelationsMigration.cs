using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Salon.Migrations
{
    /// <inheritdoc />
    public partial class RelationsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_ID",
                table: "Users",
                column: "Role_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Status_ID",
                table: "Users",
                column: "Status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_Specialist_ID",
                table: "Services",
                column: "Specialist_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Service_ID",
                table: "Reservations",
                column: "Service_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Specialist_ID",
                table: "Reservations",
                column: "Specialist_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_User_ID",
                table: "Reservations",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_Reservation_ID",
                table: "Histories",
                column: "Reservation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_User_ID",
                table: "Histories",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Specialists_ID",
                table: "Grades",
                column: "Specialists_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Specialists_Specialists_ID",
                table: "Grades",
                column: "Specialists_ID",
                principalTable: "Specialists",
                principalColumn: "Specialists_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Reservations_Reservation_ID",
                table: "Histories",
                column: "Reservation_ID",
                principalTable: "Reservations",
                principalColumn: "Reservation_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_User_ID",
                table: "Histories",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Services_Service_ID",
                table: "Reservations",
                column: "Service_ID",
                principalTable: "Services",
                principalColumn: "Service_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Specialists_Specialist_ID",
                table: "Reservations",
                column: "Specialist_ID",
                principalTable: "Specialists",
                principalColumn: "Specialists_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_User_ID",
                table: "Reservations",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Restrict);
            

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specialists_Specialist_ID",
                table: "Services",
                column: "Specialist_ID",
                principalTable: "Specialists",
                principalColumn: "Specialists_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_ID",
                table: "Users",
                column: "Role_ID",
                principalTable: "Roles",
                principalColumn: "Role_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Statuses_Status_ID",
                table: "Users",
                column: "Status_ID",
                principalTable: "Statuses",
                principalColumn: "Status_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Specialists_Specialists_ID",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Reservations_Reservation_ID",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_User_ID",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Services_Service_ID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Specialists_Specialist_ID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_User_ID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specialists_Specialist_ID",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_ID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Statuses_Status_ID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role_ID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Status_ID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Services_Specialist_ID",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Service_ID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Specialist_ID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_User_ID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Histories_Reservation_ID",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_User_ID",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Grades_Specialists_ID",
                table: "Grades");
        }
    }
}
