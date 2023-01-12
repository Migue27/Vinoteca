using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinoteca.Migrations
{
    public partial class FixedVinoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vinos_Usuarios_ApplicationUserId1",
                table: "Vinos");

            migrationBuilder.DropIndex(
                name: "IX_Vinos_ApplicationUserId1",
                table: "Vinos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Vinos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Vinos",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vinos",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Vinos",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Vinos_ApplicationUserId",
                table: "Vinos",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vinos_Usuarios_ApplicationUserId",
                table: "Vinos",
                column: "ApplicationUserId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vinos_Usuarios_ApplicationUserId",
                table: "Vinos");

            migrationBuilder.DropIndex(
                name: "IX_Vinos_ApplicationUserId",
                table: "Vinos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Vinos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vinos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Vinos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Vinos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vinos_ApplicationUserId1",
                table: "Vinos",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vinos_Usuarios_ApplicationUserId1",
                table: "Vinos",
                column: "ApplicationUserId1",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
