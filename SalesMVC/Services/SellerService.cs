using Microsoft.EntityFrameworkCore;
using SalesMVC.Data;
using SalesMVC.Models;
using SalesMVC.Services.Exceptions;

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
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(seller => seller.Id == id);
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
        public void UpdateSeller(Seller sr)
        {
            if(!_context.Seller.Any(x => x.Id == sr.Id))
            {
                throw new NotFoundException("Registro nao encontrado");
            }

            Console.WriteLine($"recebido vendedor {sr.Name}");

            try
            {
                _context.Seller.Update(sr);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException("Erro ao atualizar vendedor");
            }
        }
    }
}
