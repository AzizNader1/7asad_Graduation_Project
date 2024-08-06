using Microsoft.AspNetCore.Mvc;
using Graduation_Web_App.Models;
using Newtonsoft.Json;
using System.Text;
using System.Security.AccessControl;
using System.Dynamic;
using Graduation_Web_App.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net.Http;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using GraduationApi.Models;

namespace Graduation_Web_App.Controllers
{
    public class CompaniesServicesController : Controller
    {

        Uri baseAddress = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _client;
        dynamic myModel = new ExpandoObject();
        private readonly IFileService _fileService;
        public CompaniesServicesController(IFileService fileService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult CompanyHomePage()
        {
            /*
                 this id is get form the login function when user enter his credentials and start his session
                 according this id which we get from the session of the user we will delete any id exists in
                 any function as a parameter of the company id
            */
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLands(string? Location)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            /*
                this function is related to get the data of all the avaliabe land inside our system and 
                display them to the company to make it able to see all the information which it need aobut
                any land to make it able to rent the specific land which it need
            */
            try
            {
                HttpResponseMessage respone = _client.GetAsync(baseAddress + "Lands/GetAllLands").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var landsWithImages = new List<LandImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var lands = JsonConvert.DeserializeObject<List<Land>>(data);
                    if(Location == null)
                    {
                        foreach (var land in lands)
                        {
                            if (land.LandSize <= 0)
                            {
                                continue;
                            }
                            var image = await _fileService.GetUserProfileLatestFileNames("land", land.LandId);
                            var landWithImage = new LandImageDto()
                            {
                                Land = land,
                                LandImageUrl = image.latestImageFileName
                            };
                            landsWithImages.Add(landWithImage);
                        }
                        return View(landsWithImages);
                    }
                    var FilteredLands = lands.Where(a => a.LandLocation.Contains(Location)).ToList();
                    foreach (var land in FilteredLands)
                    {
                        if (land.LandSize <= 0)
                        {
                            continue;
                        }
                        var image = await _fileService.GetUserProfileLatestFileNames("land", land.LandId);
                        var landWithImage = new LandImageDto()
                        {
                            Land = land,
                            LandImageUrl = image.latestImageFileName
                        };
                        landsWithImages.Add(landWithImage);
                    }
                    return View(landsWithImages);

                }
                else
                {
                    TempData["GelAllLandsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllLandsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("CompanyHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LandDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");

            /*
                this function is related to get a data of a selected land from the page where we display all
                the lands data 
                according to the land id where the user do his action we will receive the id and get the data
                of that land which matched with the id
             */

            if (id == 0)
            {
                TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands", "CompaniesServices");
            }
            try
            {
                HttpResponseMessage respone = _client.GetAsync(baseAddress + "Lands/GetLandById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var landWithImage = new LandImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    Land land = JsonConvert.DeserializeObject<Land>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("land", land.LandId);
                    landWithImage.Land = land;
                    landWithImage.LandImageUrl = image.latestImageFileName;
                    var LandViewModel = new RentLandViewModel()
                    {
                        LandImageDto = landWithImage,
                        CompanyId = (int)HttpContext.Session.GetInt32("UserId"),
                        LandId = landWithImage.Land.LandId,
                        FarmerId = land.FarmerId,
                        LandSize = land.LandSize,
                    };
                    return View(LandViewModel);

                }
                else
                {
                    TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllLands","CompaniesServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands","CompaniesServices");
            }
        }

        [HttpPost]
        public IActionResult RentLand(RentLandViewModel rentLandViewModel)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            /*
                this function recive the actual order and actual data that the user enter in the form to make
                a request about renting that land and this request will still pending untill the owner of the land
                change it to any other status either accept or reject
             */
            var landOrderDto = new LandOrderDto()
            {
                CompanyId = rentLandViewModel.CompanyId,
                LandSize = rentLandViewModel.LandSize,
                FarmerId = rentLandViewModel.FarmerId,
                LandRentStatus = LandRentStatus.Pending,
                OrderEndDate = rentLandViewModel.ServiceEndDate,
                OrderPrice = rentLandViewModel.ServicePrice,
                OrderStartDate = rentLandViewModel.ServiceStartDate,
                LandId = rentLandViewModel.LandId,
            };
            string data = JsonConvert.SerializeObject(landOrderDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(baseAddress + "LandOrders/AddLandOrder", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["RentLandMessageSuccess"] = "تم تقديم طلبك بنجاح";
                return RedirectToAction("GetAllLands");
            }
            else
            {
                TempData["RentLandMessageError"] = "حدث خطا اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands");

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string? ProductName)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            /*
                this function is related to get the data of all the avaliabe products inside our system and 
                display them to the company to make it able to see all the information which it need about
                any product to make it able to but the specific product which it need
            */
            try
            {
                HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "Products/GetAllProducts").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var ProductsWithImages = new List<ProductImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var Products = JsonConvert.DeserializeObject<List<Product>>(data);
                    if(ProductName == null)
                    {
                        foreach (var Product in Products)
                        {
                            if (Product.ProductWeight <= 0)
                            {
                                continue;
                            }
                            var image = await _fileService.GetUserProfileLatestFileNames("Product", Product.ProductId);
                            var ProductWithImage = new ProductImageDto()
                            {
                                Product = Product,
                                ProductImageUrl = image.latestImageFileName
                            };
                            ProductsWithImages.Add(ProductWithImage);
                        }
                        ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
                        return View(ProductsWithImages);
                    }
                    var FilteredProducts = Products.Where(a => a.ProductName.Contains(ProductName)).ToList();
                    foreach (var Product in FilteredProducts)
                    {
                        if (Product.ProductWeight <= 0)
                        {
                            continue;
                        }
                        var image = await _fileService.GetUserProfileLatestFileNames("Product", Product.ProductId);
                        var ProductWithImage = new ProductImageDto()
                        {
                            Product = Product,
                            ProductImageUrl = image.latestImageFileName
                        };
                        ProductsWithImages.Add(ProductWithImage);
                    }
                    ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
                    return View(ProductsWithImages);
                }
                else
                {
                    TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("CompanyHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            /*
                this function will get the id of the product where the user show it's data in more details from the 
                last function 
                and according to that id we will display to him a new page include form which make him able
                to fill the data out in that for making a request to buy that product
             */

            if (id == 0)
            {
                TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts", "CompaniesServices");
            }
            try
            {
                HttpResponseMessage respone = _client.GetAsync(baseAddress + "Products/GetProductById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var productWithImage = new ProductImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var product = JsonConvert.DeserializeObject<Product>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("product", product.ProductId);
                    productWithImage.Product = product;
                    productWithImage.ProductImageUrl = image.latestImageFileName;
                    var ProductViewModel = new BuyProductViewModel()
                    {
                        FarmerId = product.FarmerId,
                        ProductImageDto = productWithImage,
                        ProductName = product.ProductName,
                        CompanyId = (int) HttpContext.Session.GetInt32("UserId"),
                        AvalibleWeight = product.ProductWeight,
                        CurrentPrice = product.ProductPrice
                    };
                    return View(ProductViewModel);

                }
                else
                {
                    TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllProducts", "CompaniesServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts", "CompaniesServices");
            }

        }

        [HttpPost]
        public IActionResult BuyProduct(BuyProductViewModel buyProductViewModel)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            /*
                this function recive the actual order and actual data that the user enter in the form to make
                a request about renting that land and this request will still pending untill the owner of the land
                change it to any other status either accept or reject
             */

            var productOrderDto = new ProductOrderDto()
            {
                CompanyId = buyProductViewModel.CompanyId,
                FarmerId = buyProductViewModel.FarmerId,
                OrderPrice = buyProductViewModel.OrderPrice,
                OrderWeight = buyProductViewModel.OrderWeight,
                ProductName = buyProductViewModel.ProductName,
                ProductOffersStatus = ProductOffersStatus.Pending
                
            };
            string data = JsonConvert.SerializeObject(productOrderDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(baseAddress + "ProductOrders/AddProductOrder", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["BuyProductMessageSuccess"] = "تم ارسال طلبك بنجاح";
                    return RedirectToAction("GetAllProducts");
            }
               
            else
            {
                TempData["BuyProductMessageError"] = "حدث خطا اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts");

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllEngineers()
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "Engineers/GetAllEngineers").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var engineersWithImages = new List<EngineerImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var engineers = JsonConvert.DeserializeObject<List<Engineer>>(data);
                    foreach (var enginner in engineers)
                    {
                        var image = await _fileService.GetUserProfileLatestFileNames("engineer", enginner.EngineerId);
                        var engineerWithImage = new EngineerImageDto()
                        {
                            Engineer = enginner,
                            EngineerImageUrl = image.latestImageFileName
                        };
                        engineersWithImages.Add(engineerWithImage);
                    }
                    return View(engineersWithImages);

                } 
                else
                {
                    TempData["GetAllEngineerError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GetAllEngineerError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("CompanyHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EngineerDetails(int id)
        {
            ViewBag.EngineerId = id;
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "Engineers/GetEngineerById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var EngineersWithImages = new EngineerImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var Engineer = JsonConvert.DeserializeObject<Engineer>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("Engineer", Engineer.EngineerId);
                    EngineersWithImages.Engineer = Engineer;
                    EngineersWithImages.EngineerImageUrl = image.latestImageFileName;
                    var EngineerViewModel = new EngineerRequestViewModel()
                    {
                        CompanyId = (int) HttpContext.Session.GetInt32("UserId"),
                        EngineerId = Engineer.EngineerId,
                        EngineerImageDto = EngineersWithImages
                    };
                    return View(EngineerViewModel);

                }
                else
                {
                    TempData["GetEngineerDetailsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllEngineers","CompaniesServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetEngineerDetailsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEngineers", "CompaniesServices");
            }
        }

        [HttpPost]
        public IActionResult RequestEngineer(EngineerRequestViewModel engineerRequestViewModel)
        {
            ViewBag.farmerid = HttpContext.Session.GetInt32("UserId");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            var engineerCompanyDto = new EngineerCompanyDto()
            {
                CompanyId = engineerRequestViewModel.CompanyId,
                EngineerId = engineerRequestViewModel.EngineerId,
                ServicePrice = engineerRequestViewModel.ServicePrice,
                ServiveDate = engineerRequestViewModel.ServiceDate,
                Status = ServiceStatusEC.Pending
            };
            string data = JsonConvert.SerializeObject(engineerCompanyDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "EngineerCompanies/AddEngineerCompany", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["RequestEngineerMessageSuccess"] = "تم ارسال طلبك بنجاح";
                return RedirectToAction("GetAllEngineers");
            }
            else
            {
                TempData["RequestEngineerMessageError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("GetAllEngineers");

            }

        }

        [HttpGet]
        public async Task<IActionResult> CompanyProfile(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "Companies/GetCompanyById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var companyImageDto = new CompanyImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var company = JsonConvert.DeserializeObject<Company>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("company", company.CompanyId);
                    companyImageDto.Company = company;
                    companyImageDto.CompanyImageUrl = image.latestImageFileName;
                    return View(companyImageDto);

                }
                else
                {
                    TempData["GetCompanyProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                    return RedirectToAction("CompanyHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GetCompanyProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("CompanyHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            HttpResponseMessage message = _client.GetAsync(_client.BaseAddress + "Companies/GetCompanyById/" + HttpContext.Session.GetInt32("UserId")).Result;

            if (message.IsSuccessStatusCode)
            {
                string Getdata = message.Content.ReadAsStringAsync().Result;
                var company = JsonConvert.DeserializeObject<Company>(Getdata);
                var companyDto = new CompanyDto()
                {
                    CompanyAddress = company.CompanyAddress,
                    CompanyEmail = company.CompanyEmail,
                    CompanyName = company.CompanyName,
                    CompanyPassword = company.CompanyPassword,
                    CompanyType = company.CompanyType
                };
                return View(companyDto);
            }
            else
            {
                TempData["GetEditCompanyProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("CompanyProfile", new {id = ViewBag.CompanyId});
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(CompanyDto companyDto, IFormCollection form)
        {
            var logingUser = new LogingUser();
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            
            try
            {

                IFormFile CompanyImageFromView = form.Files["CompanyImage"];
                IFormFile CompanyImgae = null;

                if (CompanyImageFromView != null)
                {
                    string CompanyData = JsonConvert.SerializeObject(companyDto);
                    StringContent content = new StringContent(CompanyData, Encoding.UTF8, "application/json");
                    HttpResponseMessage CompanyResponse = _client.PutAsync(_client.BaseAddress + "Companies/UpdateCompany/" + HttpContext.Session.GetInt32("UserId"), content).Result;
                    if (CompanyResponse.IsSuccessStatusCode)
                    {
                        var originalFileName = CompanyImageFromView.FileName;
                        var newFileName = companyDto.CompanyName + Path.GetExtension(originalFileName);
                        CompanyImgae = new FormFile(CompanyImageFromView.OpenReadStream(), 0, CompanyImageFromView.Length, CompanyImageFromView.FileName, originalFileName);

                        await _fileService.UploadFile(CompanyImgae, "company", (int)HttpContext.Session.GetInt32("UserId"));

                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = companyDto.CompanyEmail,
                            UserPassword = companyDto.CompanyPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData,Encoding.UTF8,"application/json");
                        HttpResponseMessage logignUserResponse = _client.PutAsync(_client.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("CompanyHomePage");
                        }
                        else
                        {
                            TempData["EditCompanyError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(companyDto);
                        }
                    }
                }
                else
                {
                    string data2 = JsonConvert.SerializeObject(companyDto);
                    StringContent content = new StringContent(data2, Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = _client.PutAsync(_client.BaseAddress + "Companies/UpdateCompany/" + HttpContext.Session.GetInt32("UserId"), content).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = companyDto.CompanyEmail,
                            UserPassword = companyDto.CompanyPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _client.PutAsync(_client.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("CompanyHomePage");
                        }
                        else
                        {
                            TempData["EditCompanyError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(companyDto);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["EditCompanyError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return View(companyDto);
            }
            return View(companyDto);
        }

        [HttpGet]
        public IActionResult CompanyHistory()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage landResponse = _client.GetAsync(_client.BaseAddress + "LandOrders/" +
                "GetOrdersByCompanyId/" + ViewBag.CompanyId).Result;
            landResponse.EnsureSuccessStatusCode();
            string landData = landResponse.Content.ReadAsStringAsync().Result;
            var landOrders = JsonConvert.DeserializeObject<List<LandOrderDetailsDto>>(landData);

            HttpResponseMessage productResponse = _client.GetAsync(_client.BaseAddress + "ProductOrders/" +
                "GetProductsByCompanyId/" + ViewBag.CompanyId).Result;
            productResponse.EnsureSuccessStatusCode();
            string productData = productResponse.Content.ReadAsStringAsync().Result;
            var productOrders = JsonConvert.DeserializeObject<List<ProductOrderDetailsDto>>(productData);

            HttpResponseMessage serviceResponse = _client.GetAsync(_client.BaseAddress + "EngineerCompanies/" +
                "GetEngineerCompanysByCompanyId/" + ViewBag.CompanyId).Result;
            serviceResponse.EnsureSuccessStatusCode();
            string serviceData = serviceResponse.Content.ReadAsStringAsync().Result;
            var serviceOrders = JsonConvert.DeserializeObject<List<EngineerCompanyDetailsDto>>(serviceData);

            var CompanyAppliedOffers = new CompanyAppliedOffersDto()
            {
                EngineerCompany = serviceOrders,
                ProductOrder = productOrders,
                LandOrder = landOrders
            };
            return View(CompanyAppliedOffers);
        }

        [HttpGet]
        public IActionResult CancleRequest(int OrderId, string OrderType)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "company")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.CompanyId = HttpContext.Session.GetInt32("UserId");
            if (OrderType == "EngineerCompany")
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "EngineerCompanies/DeleteEngineerCompany/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("CompanyHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHistory");
                }
            }
            else if (OrderType == "LandOrder")
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "LandOrders/DeleteLandOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("CompanyHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHistory");
                }
            }
            else
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "ProductOrders/DeleteProductOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("CompanyHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("CompanyHistory");
                }
            }

        }

        [HttpGet]
        public IActionResult CompanyBankAccount()
        {
            ViewBag.companyid = HttpContext.Session.GetInt32("UserId");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage CheckResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            CheckResponse.EnsureSuccessStatusCode();
            string CheckData = CheckResponse.Content.ReadAsStringAsync().Result;
            var CheckUser = JsonConvert.DeserializeObject<LogingUser>(CheckData);


            if (CheckUser != null && CheckUser.UserRole.ToLower() != "company")
            {
                return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
            }

            HttpResponseMessage BanksResponse = _client.GetAsync(_client.BaseAddress + "Banks/GetAllBanks").Result;
            BanksResponse.EnsureSuccessStatusCode();
            string BanksDate = BanksResponse.Content.ReadAsStringAsync().Result;
            var banks = JsonConvert.DeserializeObject<List<Bank>>(BanksDate);
            if (banks != null)
            {
                var banksNames = banks.Select(b => new SelectListItem
                {
                    Text = b.BankName,
                    Value = b.BankId.ToString()
                }).ToList();

                banksNames.Insert(0, new SelectListItem
                {
                    Text = "-- Select your bank --",
                    Value = ""
                });

                ViewBag.BanksNames = banksNames;
            }
            else
            {
                ViewBag.Message = "There is no banks avalible";
            }
            return View();
        }

        [HttpPost]
        public IActionResult CompanyBankAccount(CompanyAccountDto companyAccountDto)
        {
            ViewBag.companyid = HttpContext.Session.GetInt32("UserId");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage CheckResponse = _client.GetAsync(_client.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            CheckResponse.EnsureSuccessStatusCode();
            string CheckData = CheckResponse.Content.ReadAsStringAsync().Result;
            var CheckUser = JsonConvert.DeserializeObject<LogingUser>(CheckData);


            if (CheckUser != null && CheckUser.UserRole.ToLower() != "company")
            {
                return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
            }

            var fees = 200;
            var newBalance = companyAccountDto.AccountBalance - fees;
            companyAccountDto.AccountBalance = newBalance;
            var userid = HttpContext.Session.GetInt32("UserId");
            var CompanyAccount = new CompanyAccount()
            {
                AccountBalance = companyAccountDto.AccountBalance,
                CvvNumber = companyAccountDto.CvvNumber,
                ExpireDate = companyAccountDto.ExpireDate,
                AccountNumber = companyAccountDto.AccountNumber,
                BankId = companyAccountDto.BankId,
                CompanyId = (int)HttpContext.Session.GetInt32("UserId")
            };

            string companydata = JsonConvert.SerializeObject(CompanyAccount);
            StringContent companycontent = new StringContent(companydata, Encoding.UTF8, "application/json");
            HttpResponseMessage companyresponse = _client.PutAsync(baseAddress + "CompanyAccounts/AddCompanyAccount", companycontent).Result;
            if (companyresponse.IsSuccessStatusCode)
            {
                return RedirectToAction("CompanyHomePage");
            }
            return View(companyAccountDto);
        }
    }

}