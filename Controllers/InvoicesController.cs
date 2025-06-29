using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_MEGA_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService) { 
            _invoiceService = invoiceService;   
        }

        [HttpPost("calculate-debt")]
        public async Task<IActionResult> CalculateDebt([FromBody] InvoiceCalculationRequest request) {

            try
            {
                var invoice = await _invoiceService.GenerateInvoice(request.SubscriberId, request.PeriodStart, request.PeriodEnd);
               
                return Ok(invoice);
            }
            catch (Exception ex) { 
                                return BadRequest(new { message = ex.Message });

            }
        }
    }
}
