using Microsoft.AspNetCore.Mvc;
using SalesMVC.Models;
using SalesMVC.Services;

namespace SalesMVC.Controllers
{
    public class SellerController : Controller
    {
        public readonly SellerService _sellerService;

        public SellerController(SellerService sellerService) {
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            // chamar metodo para inserir dados no banco
            _sellerService.AddSeller(seller);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
