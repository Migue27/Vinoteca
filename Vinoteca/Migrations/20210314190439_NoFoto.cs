using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinoteca.Migrations
{
    public partial class NoFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenVino",
                table: "Vinos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImagenVino",
                table: "Vinos",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
