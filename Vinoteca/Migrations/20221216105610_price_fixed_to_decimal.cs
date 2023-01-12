using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinoteca.Migrations
{
    public partial class price_fixed_to_decimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Vinos",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Coste",
                table: "Pedidos",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Precio",
                table: "Vinos",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "Coste",
                table: "Pedidos",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
