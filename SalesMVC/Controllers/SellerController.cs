using Microsoft.AspNetCore.Mvc;
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
    }
}
