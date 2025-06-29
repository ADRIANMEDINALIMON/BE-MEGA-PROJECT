using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Interfaces;

namespace BE_MEGA_PROJECT.Services.Implementations
{
    public class InvoiceService: IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository) { 
            _invoiceRepository = invoiceRepository;
        }
        public async Task<InvoiceDTO> GenerateInvoice(int suscriberId, DateTime periodStart, DateTime periodEnd)
        {
            return await _invoiceRepository.GenerateInvoice(suscriberId, periodStart, periodEnd);
            
        }
    }
}
