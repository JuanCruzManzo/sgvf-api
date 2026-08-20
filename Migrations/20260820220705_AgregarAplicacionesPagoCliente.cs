using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgvf_api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAplicacionesPagoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AplicacionesPagosClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagoClienteId = table.Column<int>(type: "int", nullable: false),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    MontoAplicado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AplicacionesPagosClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AplicacionesPagosClientes_PagosClientes_PagoClienteId",
                        column: x => x.PagoClienteId,
                        principalTable: "PagosClientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AplicacionesPagosClientes_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionesPagosClientes_PagoClienteId",
                table: "AplicacionesPagosClientes",
                column: "PagoClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AplicacionesPagosClientes_VentaId",
                table: "AplicacionesPagosClientes",
                column: "VentaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AplicacionesPagosClientes");
        }
    }
}
