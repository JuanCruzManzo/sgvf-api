namespace sgvf_api.Entities;

using sgvf_api.Enums;

public class MovimientoStock
{
    public int Id { get; set; }

    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public TipoMovimientoStock TipoMovimiento { get; set; }

    public MotivoMovimientoStock Motivo { get; set; }

    public int CantidadCajones { get; set; }

    public int StockAnterior { get; set; }

    public int StockPosterior { get; set; }

    public int? VentaId { get; set; }
    public Venta? Venta { get; set; }
}