using Microsoft.EntityFrameworkCore;
using SalesMVC.Data;
using SalesMVC.Models;

namespace SalesMVC.Services
{
    public class SalesRecordsService
    {
        // find by date
        // adicionar o contexto

        private SalesMVCContext _context;

        public SalesRecordsService(SalesMVCContext context) {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (!minDate.HasValue)
            {
                // primeiro de janeiro do ano atual
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            result = result.Where(obj => obj.Date >= minDate && obj.Date <= maxDate);

            return await result
                .Include(obj => obj.Seller)
                .Include(obj => obj.Seller.Department)
                .OrderByDescending(obj => obj.Date)
                .ToListAsync();
            // join com Department, Seller e ordenado por ordem decrescente
        }

    }
}
