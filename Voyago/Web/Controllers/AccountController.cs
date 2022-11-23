using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Services.Interfaces;
using Models.InputModels;
using Models.ViewModels;
using Web.Extensions;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        private readonly IProductService productService;

        public static UserProfileViewModel userProfile;

        public static List<StateViewModel> states = new List<StateViewModel> {
          new StateViewModel("AL", "Alabama"),
          new StateViewModel("AK", "Alaska"),
          new StateViewModel("AZ", "Arizona"),
          new StateViewModel("AR", "Arkansas"),
          new StateViewModel("CA", "California"),
          new StateViewModel("CO", "Colorado"),
          new StateViewModel("CT", "Connecticut"),
          new StateViewModel("DE", "Delaware"),
          new StateViewModel("DC", "District Of Columbia"),
          new StateViewModel("FL", "Florida"),
          new StateViewModel("GA", "Georgia"),
          new StateViewModel("HI", "Hawaii"),
          new StateViewModel("ID", "Idaho"),
          new StateViewModel("IL", "Illinois"),
          new StateViewModel("IN", "Indiana"),
          new StateViewModel("IA", "Iowa"),
          new StateViewModel("KS", "Kansas"),
          new StateViewModel("KY", "Kentucky"),
          new StateViewModel("LA", "Louisiana"),
          new StateViewModel("ME", "Maine"),
          new StateViewModel("MD", "Maryland"),
          new StateViewModel("MA", "Massachusetts"),
          new StateViewModel("MI", "Michigan"),
          new StateViewModel("MN", "Minnesota"),
          new StateViewModel("MS", "Mississippi"),
          new StateViewModel("MO", "Missouri"),
          new StateViewModel("MT", "Montana"),
          new StateViewModel("NE", "Nebraska"),
          new StateViewModel("NV", "Nevada"),
          new StateViewModel("NH", "New Hampshire"),
          new StateViewModel("NJ", "New Jersey"),
          new StateViewModel("NM", "New Mexico"),
          new StateViewModel("NY", "New York"),
          new StateViewModel("NC", "North Carolina"),
          new StateViewModel("ND", "North Dakota"),
          new StateViewModel("OH", "Ohio"),
          new StateViewModel("OK", "Oklahoma"),
          new StateViewModel("OR", "Oregon"),
          new StateViewModel("PA", "Pennsylvania"),
          new StateViewModel("RI", "Rhode Island"),
          new StateViewModel("SC", "South Carolina"),
          new StateViewModel("SD", "South Dakota"),
          new StateViewModel("TN", "Tennessee"),
          new StateViewModel("TX", "Texas"),
          new StateViewModel("UT", "Utah"),
          new StateViewModel("VT", "Vermont"),
          new StateViewModel("VA", "Virginia"),
          new StateViewModel("WA", "Washington"),
          new StateViewModel("WV", "West Virginia"),
          new StateViewModel("WI", "Wisconsin"),
          new StateViewModel("WY", "Wyoming")
        };

        public AccountController(IUserService userService, IOrderService orderService, IProductService productService)
        {
            
            this.userService = userService;
            this.productService = productService;

            this.orderService = orderService;

            if (userProfile == null)
            {
                userProfile = new UserProfileViewModel();
            }
         }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext == null || HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new ArgumentNullException("HttpContext", "HttpContext is NULL");
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        //[HttpGet]
        //public ActionResult Invoice(int orderNumber)
        //{
        //    var test = new OrderDetailsViewModel()
        //    {
        //        OrderNumber = orderNumber
        //    };
        //
        //    ViewBag.OrderNumber = orderNumber;
        //    return View("Invoice2",test);
        //}

        [HttpGet]
        public IActionResult Invoice(int orderNumber)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("OrN", orderNumber);

            //string parameterValue = Newtonsoft.Json.JsonConvert.SerializeObject(parameters).Replace("\"", "'");
            //reportSource.Parameters.Add("JSONData", parameterValue);
            // {}

            var reportSource = new ReportSourceModel()
            {
                ReportId = "OrderInvoice.trdp",
                Parameters = parameters
                                                                    
            };

            //report.Parameters.Add("OrN", orderNumber);
            //return View("~/Views/Shared/ReportViewer.cshtml", report);

            return View("~/Views/Shared/ReportViewer.cshtml", reportSource);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginUserInpuModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userService.GetUserByLoginCredentials(input);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Name, user.FirstName),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext == null || HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new ArgumentNullException("HttpContext", "HttpContext is NULL");
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInpuModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await userService.UserExists(input.Email))
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
                return View();
            }

            if (!await userService.CreateNewUser(input))
            {
                ModelState.AddModelError(string.Empty, "An error occured. Please try again");
                return View();
            }

            return Redirect("/Account/Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ShoppingCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.ItemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userDetails = await userService.GetUserPersonalDetails(userEmail);
                if (userDetails != null)
                {
                    userProfile = userDetails;
                }
                return View(userProfile);
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        [HttpGet]
        [Authorize]
        public JsonResult States()
        {
            return Json(states.ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserPersonalDetails(ProfileUserInputModel personalDetails)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (personalDetails != null && ModelState.IsValid)
            {
                if (!await userService.EditUserDetails(personalDetails, userEmail))
                {
                    return Redirect("/Home/Error");
                }
                userProfile.FirstName = personalDetails.FirstName;
                userProfile.LastName = personalDetails.LastName;
                //userProfile.EmailAddress = personalDetails.EmailAddress;
                userProfile.Phone = personalDetails.Phone;
            }
            return View("Profile", userProfile);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserPassword(PasswordUserInputModel input)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (input != null)
            {
                if (!await userService.EditUserPassword(input, userEmail))
                {
                    return Redirect("/Home/Error");
                }
                userProfile.Password = input.Password;
                userProfile.ConfimPassword = input.ConfimPassword;
            }
            return View("Profile", userProfile);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserShippingDetails(AddressUserInputModel input)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (input != null)
            {
                if (!await userService.EditUserAddress(input, userEmail))
                {
                    return Redirect("/Home/Error");
                }
                userProfile.Street = input.Street;
                userProfile.State = input.State;
                userProfile.Country = input.Country;
                userProfile.City = input.City;
                userProfile.Zipcode = input.Zipcode;
            }
            return View("Profile", userProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProductToShoppingCart(int productId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (!await userService.AddShoppingCartItem(productId, userEmail))
            {
                return Redirect("/Home/Error");
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserShoppingCartItems()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var items = userService.GetUserShoppingCartItems(userEmail);

            return Json(items);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserShoppingCartItem(ShoppingCartItemViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                await userService.ChangeShoppingCartItemQuantity(item.ProductId, item.ShoppingCartId, item.Quantity);
            }

            return Json(item);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoverUserShoppingCartItem(ShoppingCartItemViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                await userService.RemoveShoppingCartItem(item.ProductId, item.ShoppingCartId);
            }

            return Json(ModelState);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCartItemsCount()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var itemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            return Json(itemsCount);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckoutShoppingCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var shoppingCartItems = userService.GetUserShoppingCartItems(userEmail).ToList();

            if (!await orderService.AddSalesOrder(shoppingCartItems, userEmail))
            {
                return Redirect("/Home/Error");
            }

            if (!await userService.ClearUserShoppingCart(userEmail))
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Account/ShoppingCart");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Favorites()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProductToFavorites(int productId)
        {
            var key = "_Favorites";
            var value = HttpContext.Session.Get<List<int>>(key);

            if (value == default)
            {
                value = new List<int>();
                value.Add(productId);
            }
            else if (!value.Contains(productId))
            {
                value.Add(productId);
            }

            HttpContext.Session.Set<List<int>>(key, value);

            var test = HttpContext.Session.Get<List<int>>(key);

            return Json(productId);
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveProductFromFavorites(int productId)
        {
            var key = "_Favorites";
            var value = HttpContext.Session.Get<List<int>>(key);

            if (value != default)
            {
                value.Remove(productId);
                HttpContext.Session.Set<List<int>>(key, value);
            }

            return Json(value);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFavoriteProducts()
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
                return Json(new { });
            }

            var favoriteProducts = userService.GetFavoriteProductsById(favoriteProductIds);

            return Json(favoriteProducts);
        }


        [HttpGet]
        public async Task<FileContentResult> FavouritesReport()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userDetails = await userService.GetUserPersonalDetails(userEmail);
            var userName = userDetails.FirstName + " " + userDetails.LastName;

            var reportData = new FavoriteUserProductViewModel()
            {
                UserName = userName,
                FavoriteReportProducts = new List<FavoriteReportProductViewModel>()
            };

            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
               // return Json(new { });
            }

            var favoriteProducts = userService.GetFavoriteProductsById(favoriteProductIds);

            foreach(var favoriteProduct in favoriteProducts)
            {
                //var photo = productService.GetProductLargePhotoById((int)favoriteProduct.PhotoId);
                byte[]? largePhotoData = await productService.GetProductLargePhotoById((int)favoriteProduct.PhotoId);

                reportData.FavoriteReportProducts.Add(new FavoriteReportProductViewModel
                {
                    Id = favoriteProduct.Id,
                    ProductName = favoriteProduct.Name,
                    Price = (decimal)favoriteProduct.FinalPrice,
                    LargePhoto = largePhotoData
                });
            }

            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            var reportSource = new Telerik.Reporting.UriReportSource();

            reportSource.Uri = "./Reports/Favourites.trdp";

            string parameterValue = Newtonsoft.Json.JsonConvert.SerializeObject(reportData);
            reportSource.Parameters.Add("JSONData", parameterValue);

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, null);

            if (!result.HasErrors)
            {
                return File(result.DocumentBytes, "application/pdf", "favourites.pdf");

                //string fileName = result.DocumentName + "." + result.Extension;
                //string path = System.IO.Path.GetTempPath();
                //string filePath = System.IO.Path.Combine(path, fileName);
                //
                //using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                //{
                //    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                //}
            }

            //var fileContents = Convert.FromBase64String(base64); 


            throw new Exception("Incorrect data");
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetFavoritesCount()
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");

            if (favoriteProductIds == default)
            {
                return Json(0);
            }

            return Json(favoriteProductIds.Count);
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> AddSalesOrder()
        //{
        //    var userEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var items = userService.GetUserShoppingCartItems(userEmail);
        //
        //    if (!await orderService.AddSalesOrder(items, userEmail))
        //    {
        //        return Redirect("/Home/Error");
        //    }
        //
        //    return Ok();
        //}

        [HttpGet]
        [Authorize]
        public IActionResult ProductIsInFavorites(int productId)
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
                return Json(false);
            }

            return Json(favoriteProductIds.Contains(productId));
        }

        [HttpGet]
        public IActionResult ProfileDetails()
        {
            return View();
        }


        [HttpGet]
        [Authorize]

        public IActionResult OrdersPage()
        {           
            return View();
        }

        [HttpGet]
        [Authorize]

        public IActionResult GetAllOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = orderService.GetAllOrders(userEmail);
            return Json(orders);
        }

        [HttpGet]
        [Authorize]
        public IActionResult OrderDetails(int id)
        {
            ViewData["OrderNumber"] = id;
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetOrderById(int id)
        {
            ViewData["OrderNumber"] = id;
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = orderService.GetOrderDetailsById(id, userEmail);
            return Json(orders);
        }
    }
}