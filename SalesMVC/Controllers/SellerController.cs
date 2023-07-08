using Microsoft.AspNetCore.Mvc;

namespace SalesMVC.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
