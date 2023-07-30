using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using SalesMVC.Models;
using SalesMVC.Models.ViewModels;
using SalesMVC.Services;

namespace SalesMVC.Controllers
{
    public class SellerController : Controller
    {
        public readonly SellerService _sellerService;
        public readonly DepartmentService _departmentService;

        public SellerController(SellerService sellerService, DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            // chamar metodo para inserir dados no banco
            _sellerService.AddSeller(seller);
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if( id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao fornecido" });
            }

            Seller sr = _sellerService.FindById(id.Value);
            if( sr == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao encontrado" });
            }

            return View(sr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if( id == null )
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao fornecido" });
            }
            
            Seller sr = _sellerService.FindById((int)id);

            if( sr == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao encontrado" });
            }
            
            return View(sr);
        }

        public IActionResult Edit(int? id)
        {
            if( id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não fornecido" });
            }

            Seller sr = _sellerService.FindById((int)id);

            if(sr == null){
                return RedirectToAction(nameof(Error), new { message = "ID nao encontrado" });
            }

            List<Department> departments = _departmentService.FindAll();

            SellerFormViewModel srViewModel = new SellerFormViewModel { Seller = sr, Departments = departments };

            return View(srViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, Seller seller)
        {
            
            if(!((int)id == seller.Id))
            {
                return RedirectToAction(nameof(Error), new { message = "IDs diferentes" });
            }

            try
            {
                _sellerService.UpdateSeller(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        public IActionResult Error(string message="")
        {
            string requestId = Activity.Current.Id ?? HttpContext.TraceIdentifier;

            ErrorViewModel viewModel = new ErrorViewModel { RequestId = requestId, Message = message };
            return View(viewModel);
        }
    }
}
