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

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task AddSellerAsync(Seller seller)
        {
            _context.Add(seller);
			await _context.SaveChangesAsync();
        }

        public async Task<Seller?> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(seller => seller.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            Seller? sr = await FindByIdAsync(id);
            if (sr != null)
            {
                try {
                    _context.Seller.Remove(sr);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException e)
                {
                    throw new IntegrityException(e.Message);
                }
            }
        }
        public async Task UpdateSellerAsync(Seller sr)
        {
            bool hasAny = _context.Seller.Any(x => x.Id == sr.Id);

			if (!hasAny)
            {
                throw new NotFoundException("Registro nao encontrado");
            }

            Console.WriteLine($"recebido vendedor {sr.Name}");

            try
            {
                _context.Seller.Update(sr);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException("Erro ao atualizar vendedor");
            }
        }
    }
}
