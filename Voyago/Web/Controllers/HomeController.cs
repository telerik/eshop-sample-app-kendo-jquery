using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.Interfaces;
using Web.Extensions;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var products = productService.GetAllProducts();
            return View();
        }

        [HttpGet]
        public IActionResult Other()
        {
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult Clothing()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Bikes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Accessories()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Components()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }       

        public IActionResult ProductCatalog()
        {
            var reportSource = new ReportSourceModel()
            {
                ReportId = "ProductCatalog.trdp"
            };            

            return View("~/Views/Shared/ReportViewer.cshtml", reportSource);
        }


        public JsonResult GetSearchSuggestions(string text)
        {
            var products = productService.GetSearchSuggestions(text).ToList();

            return Json(products);
        }

        [HttpGet]
        public JsonResult GetTopSellingSubCategories(int count)
        {
            var topSellingCategories = productService.GetTopSellingSubCategories(count);

            return Json(topSellingCategories);
        }

        [HttpGet]
        public JsonResult GetRecentlyViewedProducts()
        {
            var recentlyViewedProductIds = HttpContext.Session.Get<Queue<int>>("_RecentlyViewed");
            if (recentlyViewedProductIds == default)
            {
                return Json(new List<string>());
            }

            var recentlyViewedProducts = productService.GetListOfProducts(recentlyViewedProductIds).ToList();

            return Json(recentlyViewedProducts);
        }

    }
}