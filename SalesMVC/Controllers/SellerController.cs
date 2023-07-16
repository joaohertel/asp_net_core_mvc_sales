﻿using Microsoft.AspNetCore.Mvc;
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
    }
}
