using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using sgvf_api.Entities;
using QuestPDF.Fluent;
namespace sgvf_api.Services.Pdf
{
    public class TicketDocument : IDocument
    {
        private readonly Venta _venta;

        public TicketDocument(Venta venta)
        {
            _venta = venta;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // Tamaño aproximado ticket térmico 80 mm
                page.Size(new PageSize(80, 200, Unit.Millimetre));

                page.Margin(8);

                page.DefaultTextStyle(x =>
                    x.FontFamily(Fonts.Arial)
                     .FontSize(10));

                page.Content().Column(column =>
                {
                    Encabezado(column);

                    column.Item().PaddingVertical(8);

                    column.Item().LineHorizontal(1);

                    Productos(column);

                    column.Item().PaddingVertical(8);

                    column.Item().LineHorizontal(1);

                    Total(column);

                    column.Item().PaddingVertical(8);

                    column.Item().LineHorizontal(1);

                    Pie(column);
                });
            });
        }

        private void Encabezado(ColumnDescriptor column)
        {
            column.Item()
                .AlignCenter()
                .Text("FRUTIHORTÍCOLA")
                .FontSize(18)
                .Bold();

            column.Item()
                .AlignCenter()
                .Text("Sistema de Gestión de Ventas");

            column.Item().PaddingTop(10);

            column.Item().Text($"Venta N°: {_venta.Id}");

            column.Item().Text($"Fecha: {_venta.Fecha:dd/MM/yyyy}");

            column.Item().Text($"Hora: {_venta.Fecha:HH:mm}");

            column.Item().Text(
                $"Cliente: {(_venta.Cliente != null ? _venta.Cliente.Nombre : "Público General")}");
        }

        private void Productos(ColumnDescriptor column)
        {
            foreach (var detalle in _venta.DetallesVenta)
            {
                column.Item().PaddingTop(8);

                column.Item()
                    .Text(detalle.Producto.Nombre)
                    .Bold();

                column.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text($"{detalle.CantidadCajones} x ${detalle.PrecioUnitario:N2}");

                    row.ConstantItem(70)
                        .AlignRight()
                        .Text($"${detalle.Subtotal:N2}");
                });
            }
        }
        private void Total(ColumnDescriptor column)
        {
            column.Item().PaddingTop(10);

            column.Item().Row(row =>
            {
                row.RelativeItem()
                    .Text("TOTAL")
                    .FontSize(16)
                    .Bold();

                row.ConstantItem(90)
                    .AlignRight()
                    .Text($"${_venta.Total:N2}")
                    .FontSize(16)
                    .Bold();
            });
        }

        private void Pie(ColumnDescriptor column)
        {
            column.Item().PaddingTop(15);

            column.Item()
                .AlignCenter()
                .Text("¡Gracias por su compra!")
                .Bold();

            column.Item()
                .AlignCenter()
                .Text("Sistema SGVF");

            column.Item().PaddingTop(5);

            column.Item()
                .AlignCenter()
                .Text("Ticket no fiscal")
                .FontSize(8)
                .Italic();
        }
    }
}