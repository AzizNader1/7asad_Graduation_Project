using Graduation_Web_App.Models;
using Graduation_Web_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;

namespace Graduation_Web_App.Controllers
{
    public class BuyerFarmersController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _client;
        dynamic myModel = new ExpandoObject();
        private readonly IFileService _fileService;
        public BuyerFarmersController(IFileService fileService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult BuyerFarmerHomePage()
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");

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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
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
                    return RedirectToAction("BuyerFarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllLandsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("BuyerFarmerHomePage");
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");

            /*
                this function is related to get a data of a selected land from the page where we display all
                the lands data 
                according to the land id where the user do his action we will receive the id and get the data
                of that land which matched with the id
             */

            if (id == 0)
            {
                TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands", "BuyerFarmers");
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
                    var LandViewModel = new FarmerRentLandViewModel()
                    {
                        LandImageDto = landWithImage,
                        BuyerFarmerId = (int)HttpContext.Session.GetInt32("UserId"),
                        LandId = landWithImage.Land.LandId,
                        FarmerId = land.FarmerId,
                        LandSize = land.LandSize,
                    };
                    return View(LandViewModel);

                }
                else
                {
                    TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllLands", "BuyerFarmers");
                }
            }
            catch (Exception ex)
            {
                TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands", "BuyerFarmers");
            }
        }

        [HttpPost]
        public IActionResult RentLand(FarmerRentLandViewModel rentLandViewModel)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
            /*
                this function recive the actual order and actual data that the user enter in the form to make
                a request about renting that land and this request will still pending untill the owner of the land
                change it to any other status either accept or reject
             */
            var landOrderDto = new FarmerLandOrderDto()
            {
                BuyerFarmerId = rentLandViewModel.BuyerFarmerId,
                LandSize = rentLandViewModel.LandSize,
                FarmerId = rentLandViewModel.FarmerId,
                LandRentStatus = FarmerLandRentStatus.Pending,
                OrderEndDate = rentLandViewModel.ServiceEndDate,
                OrderPrice = rentLandViewModel.ServicePrice,
                OrderStartDate = rentLandViewModel.ServiceStartDate,
                LandId = rentLandViewModel.LandId,
            };
            string data = JsonConvert.SerializeObject(landOrderDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(baseAddress + "FarmerLandOrders/AddFarmerLandOrder", content).Result;
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
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
                        ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
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
                    ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
                    return View(ProductsWithImages);

                }
                else
                {
                    TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("BuyerFarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("BuyerFarmerHomePage");
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
            /*
                this function will get the id of the product where the user show it's data in more details from the 
                last function 
                and according to that id we will display to him a new page include form which make him able
                to fill the data out in that for making a request to buy that product
             */

            if (id == 0)
            {
                TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts", "BuyerFarmers");
            }
            try
            {
                HttpResponseMessage respone = _client.GetAsync(baseAddress + "Products/GetProductById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var productWithImage = new ProductImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    Product product = JsonConvert.DeserializeObject<Product>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("product", product.ProductId);
                    productWithImage.Product = product;
                    productWithImage.ProductImageUrl = image.latestImageFileName;
                    var ProductViewModel = new FarmerBuyProductViewModel()
                    {
                        FarmerId = product.FarmerId,
                        ProductImageDto = productWithImage,
                        ProductName = product.ProductName,
                        BuyerFarmerId = (int)HttpContext.Session.GetInt32("UserId"),
                        CurrentWeight = product.ProductWeight,
                        CurrentPrice = product.ProductPrice,
                    };
                    return View(ProductViewModel);

                }
                else
                {
                    TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllProducts", "BuyerFarmers");
                }
            }
            catch (Exception ex)
            {
                TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts", "BuyerFarmers");
            }

        }

        [HttpPost]
        public IActionResult BuyProduct(FarmerBuyProductViewModel buyProductViewModel)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
            /*
                this function recive the actual order and actual data that the user enter in the form to make
                a request about renting that land and this request will still pending untill the owner of the land
                change it to any other status either accept or reject
             */

            var productOrderDto = new FarmerProductOrderDto()
            {
                BuyerFarmerId = buyProductViewModel.BuyerFarmerId,
                FarmerId = buyProductViewModel.FarmerId,
                OrderPrice = buyProductViewModel.OrderPrice,
                OrderWeight = buyProductViewModel.OrderWeight,
                ProductName = buyProductViewModel.ProductName,
                ProductOffersStatus = FarmerProductOffersStatus.Pending

            };
            string data = JsonConvert.SerializeObject(productOrderDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(baseAddress + "FarmerProductOrders/AddFarmerProductOrder", content).Result;
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
        public async Task<IActionResult> BuyerFarmerProfile(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var buyerFarmerImageDto = new BuyerFarmerImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var buyer = JsonConvert.DeserializeObject<BuyerFarmer>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", buyer.BuyerFarmerId);
                    buyerFarmerImageDto.BuyerFarmer = buyer;
                    buyerFarmerImageDto.BuyerFarmerImageUrl = image.latestImageFileName;
                    return View(buyerFarmerImageDto);

                }
                else
                {
                    TempData["GetBuyerFarmerProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                    return RedirectToAction("BuyerFarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GetBuyerFarmerProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("BuyerFarmerHomePage");
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");
            HttpResponseMessage message = _client.GetAsync(_client.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + HttpContext.Session.GetInt32("UserId")).Result;

            if (message.IsSuccessStatusCode)
            {
                string Getdata = message.Content.ReadAsStringAsync().Result;
                var buyer = JsonConvert.DeserializeObject<BuyerFarmer>(Getdata);
                var buyerDto = new BuyerFarmerDto()
                {
                    FarmerAddress = buyer.FarmerAddress,
                    FarmerEmail = buyer.FarmerEmail,
                    FarmerName = buyer.FarmerName,
                    FarmerPassword = buyer.FarmerPassword,
                    FarmerPhone = buyer.FarmerPhone
                };
                return View(buyerDto);
            }
            else
            {
                TempData["GetEditBuyerFarmerProfileError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("BuyerFarmerProfile", new { id = ViewBag.BuyerFarmerId });
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(BuyerFarmerDto buyerFarmerDto, IFormCollection form)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {

                IFormFile buyerFarmerImageFromView = form.Files["BuyerFarmerImage"];
                IFormFile buyerFarmerImgae = null;

                if (buyerFarmerImageFromView != null)
                {
                    string buyerFarmerData = JsonConvert.SerializeObject(buyerFarmerDto);
                    StringContent content = new StringContent(buyerFarmerData, Encoding.UTF8, "application/json");
                    HttpResponseMessage buyerFarmerResponse = _client.PutAsync(_client.BaseAddress + "BuyerFarmers/UpdateBuyerFarmer/" + HttpContext.Session.GetInt32("UserId"), content).Result;
                    if (buyerFarmerResponse.IsSuccessStatusCode)
                    {
                        var originalFileName = buyerFarmerImageFromView.FileName;
                        var newFileName = buyerFarmerDto.FarmerName + Path.GetExtension(originalFileName);
                        buyerFarmerImgae = new FormFile(buyerFarmerImageFromView.OpenReadStream(), 0, buyerFarmerImageFromView.Length, buyerFarmerImageFromView.FileName, originalFileName);

                        await _fileService.UploadFile(buyerFarmerImgae, "buyerfarmer", (int)HttpContext.Session.GetInt32("UserId"));

                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = buyerFarmerDto.FarmerEmail,
                            UserPassword = buyerFarmerDto.FarmerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _client.PutAsync(_client.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("BuyerFarmerHomePage");
                        }
                        else
                        {
                            TempData["EditBuyerFarmerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(buyerFarmerDto);
                        }
                    }
                }
                else
                {
                    string data2 = JsonConvert.SerializeObject(buyerFarmerDto);
                    StringContent content = new StringContent(data2, Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = _client.PutAsync(_client.BaseAddress + "BuyerFarmers/UpdateBuyerFarmer/" + HttpContext.Session.GetInt32("UserId"), content).Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = buyerFarmerDto.FarmerEmail,
                            UserPassword = buyerFarmerDto.FarmerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _client.PutAsync(_client.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("BuyerFarmerHomePage");
                        }
                        else
                        {
                            TempData["EditBuyerFarmerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(buyerFarmerDto);
                        }
                    }
                    else
                    {
                        TempData["EditBuyerFarmerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                        return View(buyerFarmerDto);
                    }
                }
            }
            catch (Exception e)
            {
                TempData["EditBuyerFarmerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return View(buyerFarmerDto);
            }
            return View(buyerFarmerDto);
        }

        [HttpGet]
        public IActionResult BuyerFarmerHistory()
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage landResponse = _client.GetAsync(_client.BaseAddress + "FarmerLandOrders/" +
                "GetFarmerOrdersByBuyerId/" + ViewBag.BuyerFarmerId).Result;
            landResponse.EnsureSuccessStatusCode();
            string landData = landResponse.Content.ReadAsStringAsync().Result;
            var landOrders = JsonConvert.DeserializeObject<List<FarmerLandOrderDetailsDto>>(landData);


            HttpResponseMessage productResponse = _client.GetAsync(_client.BaseAddress + "FarmerProductOrders/" +
                "GetFarmerProductOrdersByBuyerId/" + ViewBag.BuyerFarmerId).Result;
            productResponse.EnsureSuccessStatusCode();
            string productData = productResponse.Content.ReadAsStringAsync().Result;
            var productOrders = JsonConvert.DeserializeObject<List<FarmerProductOrderDetailsDto>>(productData);

            var buyerAppliedOffers = new BuyerFarmerAppliedOffersDto()
            {
                ProductOrder = productOrders,
                LandOrder = landOrders
            };
            return View(buyerAppliedOffers);
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "buyerfarmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserId");

            if (OrderType == "LandOrder")
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "FarmerLandOrders/DeleteFarmerLandOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("BuyerFarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("BuyerFarmerHistory");
                }
            }
            else
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "FarmerProductOrders/DeleteFarmerProductOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("BuyerFarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("BuyerFarmerHistory");
                }
            }
        }
    }
}

