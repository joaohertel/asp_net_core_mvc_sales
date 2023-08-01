using System.Data.Common;
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
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> departments = await _departmentService.FindAllAsync();
                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            // chamar metodo para inserir dados no banco
            await _sellerService.AddSellerAsync(seller);
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if( id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao fornecido" });
            }

            Seller? sr = await _sellerService.FindByIdAsync(id.Value);
            if( sr == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao encontrado" });
            }

            return View(sr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if( id == null )
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao fornecido" });
            }
            
            Seller? sr = await _sellerService.FindByIdAsync((int)id);

            if( sr == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor nao encontrado" });
            }
            
            return View(sr);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if( id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não fornecido" });
            }

            Seller? sr = await _sellerService.FindByIdAsync((int)id);

            if(sr == null){
                return RedirectToAction(nameof(Error), new { message = "ID nao encontrado" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();

            SellerFormViewModel srViewModel = new SellerFormViewModel { Seller = sr, Departments = departments };

            return View(srViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {
			if (!ModelState.IsValid)
			{
				List<Department> departments = await _departmentService.FindAllAsync();
				SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
				return View(viewModel);
			}

			if (!((int)id == seller.Id))
            {
                return RedirectToAction(nameof(Error), new { message = "IDs diferentes" });
            }

            try
            {
                await _sellerService.UpdateSellerAsync(seller);

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
