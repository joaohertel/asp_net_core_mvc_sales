using Microsoft.AspNetCore.Mvc;
using SalesMVC.Services;

namespace SalesMVC.Controllers
{
	public class SalesRecordsController : Controller
	{
		private SalesRecordsService _salesRecordService;

		public SalesRecordsController(SalesRecordsService salesRecordService) {
			_salesRecordService = salesRecordService;
        }

		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
		{
            if (!minDate.HasValue)
            {
                // primeiro de janeiro do ano atual
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

			ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
			ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var salesRecords = await _salesRecordService.FindByDateAsync(minDate, maxDate);
			return View(salesRecords);
		}
		public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
		{
            if (!minDate.HasValue)
            {
                // primeiro de janeiro do ano atual
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var salesRecords = await _salesRecordService.FindByGroupDateAsync(minDate, maxDate);
            return View(salesRecords);
        }
	}
}
