using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinoteca.Migrations
{
    public partial class Field_to_PathProfileImage_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "Usuarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "Usuarios");
        }
    }
}
