using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgvf_api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCanceladaVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelada",
                table: "Ventas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelada",
                table: "Ventas");
        }
    }
}
