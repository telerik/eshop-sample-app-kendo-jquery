using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Web.Extensions;
using Data;

namespace Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Category(string? searchParam, string? subCategory)
        {
            if (subCategory == null || subCategory == "null")
            {
                return Redirect("/Home/Index");
            }            

            var parentCategory = await productService.GetParentCategory(subCategory);

            ViewBag.Category = parentCategory;
            ViewBag.SubCategory = subCategory;
            ViewBag.SearchParam = searchParam ?? String.Empty;

            return View();
        }

        [HttpGet]
        public JsonResult ReadAllProducts()
        {
            var result = productService.GetAllProducts().Where(p => p.SubCategory != "Other" && p.SubCategory != null).ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult Summary(string? searchParam)
        {
            ViewBag.SearchParam = searchParam ?? String.Empty;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(int productId)
        {
            var model = await productService.GetProductDetailsById(productId);

            if (model == null)
            {
                return Redirect("/Home/Index");
            }

            addToRecentlyViewed(productId);

            if (!string.IsNullOrEmpty(model.SubCategory))
            {
                ViewBag.SubCategory = model.SubCategory;
                ViewBag.Price = model.Price;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSizes(int productId)
        {
            return Json(await productService.GetAvailableSizes(productId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableColors(int productId)
        {
            return Json(await productService.GetAvailableColors(productId));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductIdByModelAndSize(int modelId, string size)
        {
            return Json(await productService.GetProductIdByModelAndSize(modelId, size));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductIdByModelAndColor(int modelId, string color)
        {
            return Json(await productService.GetProductIdByModelAndColor(modelId, color));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductIdByModelSizeAndColor(int modelId, string size, string color)
        {
            return Json(await productService.GetProductIdByModelSizeAndColor(modelId, size, color));
        }

        [HttpGet]
        public async Task<IActionResult> ReadSimilarProducts(int productId, string subCategory, int count)
        {
            var result = productService.GetSimilarProducts(productId, count).ToList();

            return Json(result);
        }


        [HttpGet]
        public IActionResult GetAllModelsInSubCategory(string subCategory)
        {
            var result = productService.GetAllModelsInSubCategory(subCategory);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetAllSizes()
        {
            return Json(productService.GetAllSizes());
        }

        [HttpGet]
        public IActionResult GetAllSizesInSubCategory(string subCategory)
        {
            return Json(productService.GetAllSizesInSubCategory(subCategory));
        }

        [HttpGet]
        public IActionResult GetAllColors()
        {
            return Json(productService.GetAllColors());
        }

        [HttpGet]
        public IActionResult GetAllColorsInSubCategory(string subCategory)
        {
            return Json(productService.GetAllColorsInSubCategory(subCategory));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductThumbnailPhotoById(int photoId)
        {
            byte[]? thumbnailData = await productService.GetProductThumbnailById(photoId);

            if (thumbnailData == null)
            {
                return File("./images/generic_bike1.png", "image/png");
            }

            return File(thumbnailData, "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductLargePhotoById(int photoId)
        {
            byte[]? largePhotoData = await productService.GetProductLargePhotoById(photoId);

            if (largePhotoData == null)
            {
                return File("./images/generic_bike1.png", "image/png");
            }

            return File(largePhotoData, "image/png");
        }

        private void addToRecentlyViewed(int productId)
        {
            var key = "_RecentlyViewed";
            var value = HttpContext.Session.Get<Queue<int>>(key);

            if (value == default)
            {
                value = new Queue<int>(4);
                value.Enqueue(productId);
            }
            else if (!value.Contains(productId))
            {
                if (value.Count() == 4)
                {
                    value.Dequeue();
                }
                value.Enqueue(productId);
            }

            HttpContext.Session.Set<Queue<int>>(key, value);
        }
    }
}
