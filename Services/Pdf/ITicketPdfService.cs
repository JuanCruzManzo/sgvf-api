namespace sgvf_api.Services.Pdf

{
    public interface ITicketPdfService
    {
        Task<byte[]> GenerarTicketAsync(int ventaId);
    }
}