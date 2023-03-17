using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinoteca.Migrations
{
    public partial class UsuarioId_Pedido_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId1",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "UsuarioId1",
                table: "Pedidos",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_UsuarioId1",
                table: "Pedidos",
                newName: "IX_Pedidos_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_ApplicationUserId",
                table: "Pedidos",
                column: "ApplicationUserId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_ApplicationUserId",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Pedidos",
                newName: "UsuarioId1");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_ApplicationUserId",
                table: "Pedidos",
                newName: "IX_Pedidos_UsuarioId1");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId1",
                table: "Pedidos",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
