using Microsoft.EntityFrameworkCore;
using SalesMVC.Data;
using SalesMVC.Models;

namespace SalesMVC.Services
{
    public class DepartmentService
    {
        public SalesMVCContext _context { get; set; }
        
        public DepartmentService(SalesMVCContext context)
        {
            _context = context;
        }
    
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(dep => dep.Name).ToListAsync();
        }
    }
}
