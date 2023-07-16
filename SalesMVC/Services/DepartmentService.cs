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
    
        public List<Department> FindAll()
        {
            return _context.Department.OrderBy(dep => dep.Name).ToList();
        }
    }
}
