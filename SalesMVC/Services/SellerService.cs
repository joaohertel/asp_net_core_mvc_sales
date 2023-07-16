using SalesMVC.Data;
using SalesMVC.Models;

namespace SalesMVC.Services
{
    public class SellerService
    {
        public readonly SalesMVCContext _context;

        public SellerService(SalesMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void AddSeller(Seller seller)
        {
            _context.Add(seller);
            _context.SaveChanges();
        }

        public Seller? FindById(int id)
        {
            return _context.Seller.FirstOrDefault(seller => seller.Id == id);
        }

        public void Delete(int id)
        {
            Seller? sr = FindById(id);
            if (sr != null)
            {
                _context.Seller.Remove(sr);
                _context.SaveChanges();
            }
        }
    }
}
