using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<InvoiceDTO> GenerateInvoice(int suscriberId, DateTime periodStart, DateTime periodEnd);
    }
}
