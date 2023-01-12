using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinoteca.Migrations
{
    public partial class VinoImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImagenVino",
                table: "Vinos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenVino",
                table: "Vinos");
        }
    }
}
