using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> GenerateInvoice(int suscriberId, DateTime periodStart, DateTime periodEnd);
    }
}
