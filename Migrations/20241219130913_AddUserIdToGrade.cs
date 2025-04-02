using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beauty_Salon.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Services_Services_ID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_User_ID1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_Services_ID",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "Services_ID",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "User_ID1",
                table: "Favorites",
                newName: "Service_ID");

            migrationBuilder.RenameColumn(
                name: "Favorit_ID",
                table: "Favorites",
                newName: "Favorite_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_User_ID1",
                table: "Favorites",
                newName: "IX_Favorites_Service_ID");

            migrationBuilder.AddColumn<int>(
                name: "User_ID",
                table: "Grades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_User_ID",
                table: "Grades",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_User_ID",
                table: "Favorites",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Services_Service_ID",
                table: "Favorites",
                column: "Service_ID",
                principalTable: "Services",
                principalColumn: "Service_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_User_ID",
                table: "Favorites",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_User_ID",
                table: "Grades",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Services_Service_ID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_User_ID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_User_ID",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_User_ID",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_User_ID",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Grades");

            migrationBuilder.RenameColumn(
                name: "Service_ID",
                table: "Favorites",
                newName: "User_ID1");

            migrationBuilder.RenameColumn(
                name: "Favorite_ID",
                table: "Favorites",
                newName: "Favorit_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_Service_ID",
                table: "Favorites",
                newName: "IX_Favorites_User_ID1");

            migrationBuilder.AddColumn<int>(
                name: "Services_ID",
                table: "Favorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_Services_ID",
                table: "Favorites",
                column: "Services_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Services_Services_ID",
                table: "Favorites",
                column: "Services_ID",
                principalTable: "Services",
                principalColumn: "Service_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_User_ID1",
                table: "Favorites",
                column: "User_ID1",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
