using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgvf_api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTipoYVentaAPagoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "PagosClientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VentaId",
                table: "PagosClientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagosClientes_VentaId",
                table: "PagosClientes",
                column: "VentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PagosClientes_Ventas_VentaId",
                table: "PagosClientes",
                column: "VentaId",
                principalTable: "Ventas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PagosClientes_Ventas_VentaId",
                table: "PagosClientes");

            migrationBuilder.DropIndex(
                name: "IX_PagosClientes_VentaId",
                table: "PagosClientes");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "PagosClientes");

            migrationBuilder.DropColumn(
                name: "VentaId",
                table: "PagosClientes");
        }
    }
}
