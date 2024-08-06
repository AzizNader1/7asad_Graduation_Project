using Graduation_Web_App.Models;
using Graduation_Web_App.Services;
using GraduationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Graduation_Web_App.Controllers
{
    public class FarmerServicesController : Controller
    {
        readonly Uri ApiAddress = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _httpClient;
        dynamic mymodel = new ExpandoObject();
        private readonly IFileService _fileService;
        private Company CurrentCompnay;
        public FarmerServicesController(HttpClient httpClient, IFileService fileService, Company currentCompnay)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = ApiAddress;
            _fileService = fileService;
            CurrentCompnay = currentCompnay;
        }

        [HttpGet]
        public IActionResult FarmerHomePage()
        {
            /*
             this method is related to display the navigation page of the farmer which include
            some of card (or whatever the way) that enable the farmer to go to other pages to make
            him able to use the services of the website
             */
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            var FarmerHomeViewModel = new FarmerHomeViewModel()
            {
                FarmerId = (int) HttpContext.Session.GetInt32("UserId")
            };
            return View(FarmerHomeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> FarmerDetails()
        {
            /*
             this method is related to display the profile page of the farmer which include
            farmer data and include button called edit which will give the farmer the ability to edit 
            his profile and the post action will related to the next method which will recive the 
            request of edit profile
             */
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + HttpContext.Session.GetInt32("UserId")).Result;
            if (getfarmer.IsSuccessStatusCode)
            {
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerModel = JsonConvert.DeserializeObject<Farmer>(farmerdata);
                var farmerimage = await _fileService.GetUserProfileLatestFileNames("farmer", farmerModel.FarmerId);
                var farmerImageDto = new FarmerImageDto()
                {
                    farmer = farmerModel,
                    FarmerImageUrl = farmerimage.latestImageFileName
                };
                return View(farmerImageDto);
            }
            else
            {
                TempData["GetFarmerProfileError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage");
            }

        }

        [HttpGet]
        public IActionResult EditFarmerProfile()
        {
            
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + HttpContext.Session.GetInt32("UserId")).Result;
            if(FarmerResponse.IsSuccessStatusCode)
            {

                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var Farmer = JsonConvert.DeserializeObject<Farmer>(FarmerData);
                var FarmerDto = new FarmerDto()
                {
                    FarmerAddress = Farmer.FarmerAddress,
                    FarmerEmail = Farmer.FarmerEmail,
                    FarmerName = Farmer.FarmerName,
                    FarmerPassword = Farmer.FarmerPassword,
                    FarmerPhone = Farmer.FarmerPhone
                };
                return View(FarmerDto);
            }
            else
            {
                TempData["GetEditFarmerProfileError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerDetails");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFarmerProfile(FarmerDto farmerDto, IFormCollection form)
        {
            var logingUser = new LogingUser();
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            /*
             this method is related to the request which came from the farmer to edit the profile data
            which include farmername, email, address, and all other data
             */

            try
            {

                IFormFile farmerImageFromView = form.Files["FarmerImage"];
                IFormFile farmerImgae = null;

                if (farmerImageFromView != null)
                {
                    string data = JsonConvert.SerializeObject(farmerDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Farmers/UpdateFarmer/" + HttpContext.Session.GetInt32("UserId"), content).Result;

                    var buyerDto = new BuyerFarmerDto()
                    {
                        FarmerAddress = farmerDto.FarmerAddress,
                        FarmerEmail = farmerDto.FarmerEmail,
                        FarmerName = farmerDto.FarmerName,
                        FarmerPassword = farmerDto.FarmerPassword,
                        FarmerPhone = farmerDto.FarmerPhone
                    };

                    string data2 = JsonConvert.SerializeObject(buyerDto);
                    StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "BuyerFarmers/UpdateBuyerFarmer/" + HttpContext.Session.GetInt32("UserBuyerId"), content2).Result;

                    if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                    {
                        var originalFileName = farmerImageFromView.FileName;
                        var newFileName = farmerDto.FarmerName + Path.GetExtension(originalFileName);
                        farmerImgae = new FormFile(farmerImageFromView.OpenReadStream(), 0, farmerImageFromView.Length, farmerImageFromView.FileName, originalFileName);

                        await _fileService.UploadFile(farmerImgae, "farmer", (int)HttpContext.Session.GetInt32("UserId"));
                        await _fileService.UploadFile(farmerImgae, "buyerfarmer", (int)HttpContext.Session.GetInt32("UserBuyerId"));
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = farmerDto.FarmerEmail,
                            UserPassword = farmerDto.FarmerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {
                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("FarmerHomePage", "FarmerServices");
                        }
                        else
                        {
                            TempData["EditResultError"] = "حدث خطا ما اثناء تحديث البيانات يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("FarmerHomePage", "FarmerServices");
                        }

                    }
                    else
                    {
                        TempData["EditResultError"] = "حدث خطا ما اثناء تحديث البيانات يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("FarmerHomePage", "FarmerServices");
                    }
                }
                else
                {
                    string data = JsonConvert.SerializeObject(farmerDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Farmers/UpdateFarmer/" + HttpContext.Session.GetInt32("UserId"), content).Result;

                    var buyerDto = new BuyerFarmerDto()
                    {
                        FarmerAddress = farmerDto.FarmerAddress,
                        FarmerEmail = farmerDto.FarmerEmail,
                        FarmerName = farmerDto.FarmerName,
                        FarmerPassword = farmerDto.FarmerPassword,
                        FarmerPhone = farmerDto.FarmerPhone
                    };

                    string data2 = JsonConvert.SerializeObject(buyerDto);
                    StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                    HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "BuyerFarmers/UpdateBuyerFarmer/" + HttpContext.Session.GetInt32("UserBuyerId"), content2).Result;

                    if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                    {
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = farmerDto.FarmerEmail,
                            UserPassword = farmerDto.FarmerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("FarmerHomePage", "FarmerServices");
                        }
                        else
                        {
                            TempData["EditResultError"] = "حدث خطا ما اثناء تحديث البيانات يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("FarmerHomePage", "FarmerServices");
                        }

                    }
                    else
                    {
                        TempData["EditResultError"] = "حدث خطا ما اثناء تحديث البيانات يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("FarmerHomePage", "FarmerServices");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["EditResultError"] = "حدث خطا ما اثناء تحديث البيانات يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");

            }
        }

        [HttpGet]
        public IActionResult FarmerHistory()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");

            /*
             this method related to display the page which include some of tables of all the 
            transactions that farmer do in his profile such as rent land or equipments from other farmers
            and also table include all the transactions between tha company and him about buying his crops
            and also the transaction with the engineer 
             */

            //1. get all land rent orders for this farmer by using the data of the farmer which we get from above method

            var serviceOrders = new List<EngineerFarmerDetailsDto>();
            var productOrders = new List<FarmerProductOrderDetailsDto>();
            var equipmentOrders = new List<FarmerEquipmentDetailsDto>();
            var landOrders = new List<FarmerLandOrderDetailsDto>();

            HttpResponseMessage landResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerLandOrders/" +
                "GetFarmerOrdersByBuyerId/" + ViewBag.BuyerFarmerId).Result;
            if (landResponse.IsSuccessStatusCode)
            {
                string landData = landResponse.Content.ReadAsStringAsync().Result;
                 landOrders = JsonConvert.DeserializeObject<List<FarmerLandOrderDetailsDto>>(landData);

            }

            //2. get all equipments rent orders for this farmer by using the data of the farmer which we get from above method

            HttpResponseMessage equipmentResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/" +
                "GetAllFarmerEquipments").Result;
            if (equipmentResponse.IsSuccessStatusCode)
            {
                string equipmentData = equipmentResponse.Content.ReadAsStringAsync().Result;
                 var FarmerEquipments = JsonConvert.DeserializeObject<List<FarmerEquipment>>(equipmentData);

                foreach(var order in FarmerEquipments)
                {
                    if(order.BuyerFarmerId != HttpContext.Session.GetInt32("UserBuyerId"))
                    {
                        continue;
                    }

                    HttpResponseMessage BuyerFarmerResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + order.BuyerFarmerId).Result;
                    BuyerFarmerResponseMessage.EnsureSuccessStatusCode();
                    string BuyerFarmerData = BuyerFarmerResponseMessage.Content.ReadAsStringAsync().Result;
                    var BuyerFarmer = JsonConvert.DeserializeObject<BuyerFarmer>(BuyerFarmerData);

                    HttpResponseMessage EquipmentResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetEquipmentById/" + order.EquipmentId).Result;
                    EquipmentResponseMessage.EnsureSuccessStatusCode();
                    string EquipmentData = EquipmentResponseMessage.Content.ReadAsStringAsync().Result;
                    var Equipment = JsonConvert.DeserializeObject<Equipment>(EquipmentData);

                    HttpResponseMessage FarmerResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + order.FarmerId).Result;
                    FarmerResponseMessage.EnsureSuccessStatusCode();
                    string FarmerData = FarmerResponseMessage.Content.ReadAsStringAsync().Result;
                    var Farmer = JsonConvert.DeserializeObject<Farmer>(FarmerData);

                    var FarmerEquipmentDetailsDto = new FarmerEquipmentDetailsDto()
                    {
                       BuyerFarmerId = order.BuyerFarmerId,
                       BuyerFarmerName = BuyerFarmer.FarmerName,
                       FarmerName = Farmer.FarmerName,
                       EquipmentId = order.EquipmentId,
                       EquipmentName = Equipment.EquipmentName,
                       EquipmentRentStatus = order.EquipmentRentStatus,
                       OwnerPhone = Farmer.FarmerPhone,
                       FarmerId = order.FarmerId,
                       RentEndDate = order.RentEndDate,
                       RentStartDate = order.RentStartDate,
                       RentPrice = order.RentPrice,
                       Id = order.FarmerEquipmentId
                    };
                    equipmentOrders.Add(FarmerEquipmentDetailsDto);
                }
            }
            

            //3. get all selling orders for this farmer by using the data of the farmer which we get from above method

            HttpResponseMessage productResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerProductOrders/" +
                "GetFarmerProductOrdersByBuyerId/" + ViewBag.BuyerFarmerId).Result;
            if (productResponse.IsSuccessStatusCode)
            {
                string productData = productResponse.Content.ReadAsStringAsync().Result;
                 productOrders = JsonConvert.DeserializeObject<List<FarmerProductOrderDetailsDto>>(productData);
            }
            

            //4. get all services orders for this farmer about test his land by using the data of the farmer which we get from above method

            HttpResponseMessage serviceResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerFarmers/" +
                "GetEngineerFarmersByFarmerId/" + ViewBag.FarmerId).Result;
            if (serviceResponse.IsSuccessStatusCode)
            {
                string serviceData = serviceResponse.Content.ReadAsStringAsync().Result;
                serviceOrders = JsonConvert.DeserializeObject<List<EngineerFarmerDetailsDto>>(serviceData);
            }

            var FarmerAppliedOffers = new FarmerAppliedOffersDto()
            {
                EngineerFarmer = serviceOrders,
                ProductOrder = productOrders,
                FarmerEquipment = equipmentOrders,
                LandOrder = landOrders
            };
            return View(FarmerAppliedOffers);
        }

        [HttpGet]
        public IActionResult CancleRequest(int OrderId, string OrderType)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            if (OrderType == "EngineerFarmer")
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "EngineerFarmers/DeleteEngineerFarmer/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("FarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHistory");
                }
            }
            else if (OrderType == "LandOrder")
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "FarmerLandOrders/DeleteFarmerLandOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("FarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHistory");
                }
            }
            else if(OrderType == "ProductOrder")
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "FarmerProductOrders/DeleteFarmerProductOrder/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("FarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHistory");
                }
            }
            else
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "FarmerEquipments/DeleteFarmerEquipment/" + OrderId).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["CancleRequestResultSuccess"] = "تم الغاء الطلب الذي قمت بتقديمه بنجاح";
                    return RedirectToAction("FarmerHistory");
                }
                else
                {
                    TempData["CancleRequestResultError"] = "حدث خطا ما اثناء الغاء الطلب يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHistory");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadLand(LandDto landDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will recive the model that farmer need to upload land by it
            and the code of this method will call an api's endpoint which responsible for 
            adding a new land to the land model in the database
             */


            IFormFile landImageFromView = form.Files["LandImage"];
            IFormFile landImgae = null;

            if (landImageFromView != null)
            {
                var originalFileName = landImageFromView.FileName;
                var newFileName = landDto.LandSize + Path.GetExtension(originalFileName);
                landImgae = new FormFile(landImageFromView.OpenReadStream(), 0, landImageFromView.Length, landImageFromView.FileName, originalFileName);

                string landData = JsonConvert.SerializeObject(landDto);
                StringContent landContect = new StringContent(landData, Encoding.UTF8, "application/json");
                HttpResponseMessage landResponse = _httpClient.PostAsync(_httpClient.BaseAddress + "Lands/AddLand", landContect).Result;

                if (landResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage Response = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetAllLands").Result;
                    Response.EnsureSuccessStatusCode();
                    string landdata = Response.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<List<Land>>(landdata);
                    var wantedland = res.Where(wf => wf.LandSize == landDto.LandSize && wf.LandLocation == landDto.LandLocation).First();

                    await _fileService.UploadFile(landImgae, "land", wantedland.LandId);
                    TempData["UploadSuccessResult"] = "تم رفع البيانات الخاصه بالارض بنجاح";
                    return RedirectToAction("FarmerHomePage");
                }
                else
                {
                    TempData["UploadErrorResult"] = "حدثت مشكله اثناء رفع البيانات يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHomePage");
                }
            }
            else
            {
                TempData["UploadErrorResult"] = "يرجي ارفاق صوره للارض التي ترغب في رفع بيانات لها";
                return RedirectToAction("FarmerHomePage");
            }


        }

        [HttpPost]
        public async Task<IActionResult> UploadEquipment(EquipmentDto equipmentDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will recive the model that farmer need to upload equipment by it
            and the code of this method will call an api's endpoint which responsible for 
            adding a new equipment to the equipment model in the database
             */


            IFormFile equipmentImageFromView = form.Files["EquipmentImage"];
            IFormFile equipmentImgae = null;

            if (equipmentImageFromView != null)
            {
                var originalFileName = equipmentImageFromView.FileName;
                var newFileName = equipmentDto.EquipmentName + Path.GetExtension(originalFileName);
                equipmentImgae = new FormFile(equipmentImageFromView.OpenReadStream(), 0, equipmentImageFromView.Length, equipmentImageFromView.FileName, originalFileName);

                string equipmentData = JsonConvert.SerializeObject(equipmentDto);
                StringContent equipmentContect = new StringContent(equipmentData, Encoding.UTF8, "application/json");
                HttpResponseMessage equipmentResponse = _httpClient.PostAsync(_httpClient.BaseAddress + "Equipments/AddEquipment", equipmentContect).Result;

                if (equipmentResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage Response = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetAllEquipments").Result;
                    Response.EnsureSuccessStatusCode();
                    string equipmentdata = Response.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<List<Equipment>>(equipmentdata);
                    var wantedequipment = res.Where(wf => wf.EquipmentName == equipmentDto.EquipmentName && wf.EquipmentPrice == equipmentDto.EquipmentPrice && wf.FarmerId == ViewBag.FarmerId).First();

                    await _fileService.UploadFile(equipmentImgae, "equipment", wantedequipment.EquipmentId);
                    TempData["UploadSuccessResult"] = "تم رفع البيانات الخاصه بالاله الزراعيه بنجاح";
                    return RedirectToAction("FarmerHomePage");
                }
                else
                {
                    TempData["UploadErrorResult"] = "حدثت مشكله اثناء رفع البيانات يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHomePage");
                }
            }
            else
            {
                TempData["UploadErrorResult"] = "يرجي ارفاق صوره للاله الزراعيه التي ترغب في رفع بيانات لها";
                return RedirectToAction("FarmerHomePage");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadCrops(ProductDto productDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will recive the model that farmer need to upload crops by it
            and the code of this method will call an api's endpoint which responsible for 
            adding a new crops to the crops model in the database
             */



            IFormFile productImageFromView = form.Files["ProductImage"];
            IFormFile productImgae = null;

            if (productImageFromView != null)
            {
                var originalFileName = productImageFromView.FileName;
                var newFileName = productDto.ProductName + Path.GetExtension(originalFileName);
                productImgae = new FormFile(productImageFromView.OpenReadStream(), 0, productImageFromView.Length, productImageFromView.FileName, originalFileName);

                string productData = JsonConvert.SerializeObject(productDto);
                StringContent productContect = new StringContent(productData, Encoding.UTF8, "application/json");
                HttpResponseMessage productResponse = _httpClient.PostAsync(_httpClient.BaseAddress + "Products/AddProduct", productContect).Result;

                if (productResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage Response = _httpClient.GetAsync(_httpClient.BaseAddress + "products/GetAllproducts").Result;
                    Response.EnsureSuccessStatusCode();
                    string productdata = Response.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<List<Product>>(productdata);
                    var wantedproduct = res.Where(wf => wf.ProductName == productDto.ProductName && wf.ProductQuality == productDto.ProductQuality).First();

                    await _fileService.UploadFile(productImgae, "product", wantedproduct.ProductId);
                    TempData["UploadSuccessResult"] = "تم رفع البيانات الخاصه بالمحصول الزراعي بنجاح";
                    return RedirectToAction("FarmerHomePage");
                }
                else
                {
                    TempData["UploadErrorResult"] = "حدثت مشكله اثناء رفع البيانات يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHomePage");
                }
            }
            else
            {
                TempData["UploadErrorResult"] = "يرجي ارفاق صوره للمحصول الذي ترغب في رفع بيانات له";
                return RedirectToAction("FarmerHomePage");
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Engineers/GetAllEngineers").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var engineersWithImages = new List<EngineerImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    List<Engineer> engineers = JsonConvert.DeserializeObject<List<Engineer>>(data);
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
                    TempData["GetAllEngineerError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHomePage", "FarmerServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetAllEngineerError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EngineerDetails(int id)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            ViewBag.EngineerId = id;

            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Engineers/GetEngineerById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var EngineersWithImages = new EngineerImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    Engineer Engineer = JsonConvert.DeserializeObject<Engineer>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("Engineer", Engineer.EngineerId);
                    EngineersWithImages.Engineer = Engineer;
                    EngineersWithImages.EngineerImageUrl = image.latestImageFileName;
                    var EngineerViewModel = new EngineerRequestViewModel()
                    {
                        EngineerImageDto = EngineersWithImages,
                        EngineerId = Engineer.EngineerId,
                        FarmerId = (int)HttpContext.Session.GetInt32("UserId")
                    };
                    return View(EngineerViewModel);

                }
                else
                {
                    TempData["GetEngineerDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllEngineers", "FarmerServices");
                }

            }
            catch (Exception ex)
            {
                TempData["GetEngineerDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEngineers", "FarmerServices");
            }
        }

        [HttpPost]
        public IActionResult RequestEngineer(EngineerRequestViewModel engineerRequestViewModel)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            var engineerFarmerDto = new EngineerFarmerDto()
            {
                EngnieerId = engineerRequestViewModel.EngineerId,
                FarmerId = engineerRequestViewModel.FarmerId,
                ServicePrice = engineerRequestViewModel.ServicePrice,
                ServiveDate = engineerRequestViewModel.ServiceDate,
                Status = ServiceStatusEF.Pending
            };
            string data = JsonConvert.SerializeObject(engineerFarmerDto);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "EngineerFarmers/AddEngineerFarmer", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["AddEngineerRequestSuccess"] = "تمت اضافه طلبك بنجاح ";
                return RedirectToAction("GetAllEngineers");
            }
            else
            {
                TempData["AddEngineerRequestError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return RedirectToAction("GetAllEngineers");

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipmentOffers()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will display a page which will include all the offers which came from other farmers
            to rent the land which the farmer upload for rent
            and the farmer will have the full authority to accepted the offer that he wanted
             */

            try
            {
                HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                getfarmer.EnsureSuccessStatusCode();
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerRes = JsonConvert.DeserializeObject<Farmer>(farmerdata);

                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/" +
                    "GetFarmerEquipmentsByFarmerId/" + ViewBag.FarmerId).Result;
                response.EnsureSuccessStatusCode();
                string data = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<FarmerEquipmentDetailsDto>>(data);
                var PendingEquipmentsOffers = res.Where(a => a.EquipmentRentStatus == EquipmentRentStatus.Pending).ToList();

                var EquipmentOrdersWithImagesDto = new List<FarmerEquipmentImageDto>();

                foreach (var item in PendingEquipmentsOffers)
                {


                    HttpResponseMessage EquipmentResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetAllEquipments").Result;
                    HttpResponseMessage BuyerFarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + item.BuyerFarmerId).Result;

                    if (EquipmentResponse.IsSuccessStatusCode && BuyerFarmerResponse.IsSuccessStatusCode)
                    {
                        string EquipmentData = EquipmentResponse.Content.ReadAsStringAsync().Result;
                        var Equipments = JsonConvert.DeserializeObject<List<Equipment>>(EquipmentData);
                        var WantedEquipment = Equipments.FirstOrDefault(a => a.EquipmentId == item.EquipmentId && a.FarmerId == ViewBag.FarmerId);

                        string BuyerDate = BuyerFarmerResponse.Content.ReadAsStringAsync().Result;
                        var BuyerFarmer = JsonConvert.DeserializeObject<BuyerFarmer>(BuyerDate);
                        var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", BuyerFarmer.BuyerFarmerId);

                        var EquipmentOrderWithImageDto = new FarmerEquipmentImageDto()
                        {
                            Equipment = WantedEquipment,
                            Farmer = farmerRes,
                            FarmerEquipmentImageUrl = image.latestImageFileName,
                            Id = item.Id,
                            EquipmentRentStatus = item.EquipmentRentStatus,
                            RentEndDate = item.RentEndDate,
                            RentStartDate = item.RentStartDate,
                            RentPrice = item.RentPrice,
                            BuyerFarmer = BuyerFarmer

                        };

                        EquipmentOrdersWithImagesDto.Add(EquipmentOrderWithImageDto); 
                    }
                    else
                    {
                        TempData["GetEquipmentsOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("FarmerHomePage", "FarmerServices");
                    }
                }

                return View(EquipmentOrdersWithImagesDto);
            }
            catch (Exception)
            {
                TempData["GetEquipmentsOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EquipmentOfferDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage FarmerEquipments = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/GetFarmerEquipmentById/" + id).Result;
                FarmerEquipments.EnsureSuccessStatusCode();
                string FarmerEquipmentsData = FarmerEquipments.Content.ReadAsStringAsync().Result;
                var FarmerEquipmentResult = JsonConvert.DeserializeObject<FarmerEquipment>(FarmerEquipmentsData);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var FarmerResult = JsonConvert.DeserializeObject<Farmer>(FarmerData);

                HttpResponseMessage BuyerFarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + FarmerEquipmentResult.BuyerFarmerId).Result;
                BuyerFarmerResponse.EnsureSuccessStatusCode();
                string BuyerDate = BuyerFarmerResponse.Content.ReadAsStringAsync().Result;
                var BuyerFarmer = JsonConvert.DeserializeObject<BuyerFarmer>(BuyerDate);

                HttpResponseMessage EquipmentsResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetAllEquipments").Result;
                EquipmentsResponse.EnsureSuccessStatusCode();
                string EquipmentsData = EquipmentsResponse.Content.ReadAsStringAsync().Result;
                var ListOfEquipments = JsonConvert.DeserializeObject<List<Equipment>>(EquipmentsData);
                var WantedEquipment = ListOfEquipments.FirstOrDefault(a => a.EquipmentId == FarmerEquipmentResult.EquipmentId && a.FarmerId == FarmerEquipmentResult.FarmerId);
                var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", BuyerFarmer.BuyerFarmerId);

                var FarmerEquipmentImageDto = new FarmerEquipmentImageDto
                {

                    Farmer = FarmerResult,
                    RentPrice = FarmerEquipmentResult.RentPrice,
                    RentStartDate = FarmerEquipmentResult.RentStartDate,
                    Id = FarmerEquipmentResult.FarmerEquipmentId,
                    RentEndDate = FarmerEquipmentResult.RentEndDate,
                    FarmerEquipmentImageUrl = image.latestImageFileName,
                    Equipment = WantedEquipment,
                    BuyerFarmer = BuyerFarmer
                };

                return View(FarmerEquipmentImageDto);
            }
            catch (Exception)
            {

                TempData["EquipmentOfferDetails"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEquipmentOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeEquipmentOfferAcceptStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage GetAllResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/GetAllFarmerEquipments").Result;

            HttpResponseMessage OneOrderResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/GetFarmerEquipmentById/" + OfferId).Result;

            if (GetAllResponseMessage.IsSuccessStatusCode && OneOrderResponse.IsSuccessStatusCode)
            {

                string GetAllData = GetAllResponseMessage.Content.ReadAsStringAsync().Result;
                var FarmerEquipmentsOrders = JsonConvert.DeserializeObject<List<FarmerEquipment>>(GetAllData);

                string responseData = OneOrderResponse.Content.ReadAsStringAsync().Result;
                var EquipmentOrder = JsonConvert.DeserializeObject<FarmerEquipment>(responseData);

              

                foreach(var order in FarmerEquipmentsOrders)
                {

                    if (order.RentStartDate == EquipmentOrder.RentStartDate && order.RentEndDate == EquipmentOrder.RentEndDate)
                    {
                        TempData["RentEquipmentDateError"] = "لا يمكنك قبول هذا العرض لان المعده الزراعيه تحت التاجير في هذا الوقت المطلوب";
                        return RedirectToAction("EquipmentOfferDetails", new { id = OfferId });
                    }
                   

                }
                var FarmerEquipmentDto = new FarmerEquipmentDto()
                {
                    EquipmentId = EquipmentOrder.EquipmentId,
                    EquipmentRentStatus = EquipmentRentStatus.Accepted,
                    FarmerId = EquipmentOrder.FarmerId,
                    RentEndDate = EquipmentOrder.RentEndDate,
                    RentPrice = EquipmentOrder.RentPrice,
                    RentStartDate = EquipmentOrder.RentStartDate,
                    BuyerFarmerId = EquipmentOrder.BuyerFarmerId
                };
                string data = JsonConvert.SerializeObject(FarmerEquipmentDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerEquipments/UpdateFarmerEquipment/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllEquipmentOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllEquipmentOffers");
                }

            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEquipmentOffers");
            }
        }
        
        [HttpPost]
        public IActionResult ChangeEquipmentOfferRejectStatus(int OfferId) 
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerEquipments/GetFarmerEquipmentById/" + OfferId).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var EquipmentOrder = JsonConvert.DeserializeObject<FarmerEquipment>(responseData);

                var FarmerEquipmentDto = new FarmerEquipmentDto()
                {
                    EquipmentId = EquipmentOrder.EquipmentId,
                    EquipmentRentStatus = EquipmentRentStatus.Rejected,
                    FarmerId = EquipmentOrder.FarmerId,
                    RentEndDate = EquipmentOrder.RentEndDate,
                    RentPrice = EquipmentOrder.RentPrice,
                    RentStartDate = EquipmentOrder.RentStartDate,
                    BuyerFarmerId = EquipmentOrder.BuyerFarmerId
                };
                string data = JsonConvert.SerializeObject(FarmerEquipmentDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerEquipments/UpdateFarmerEquipment/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllEquipmentOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllEquipmentOffers");
                } 
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEquipmentOffers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLandOffers()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will display a page which will include all the offers which came from other farmers
            to rent the land which the farmer upload for rent
            and the farmer will have the full authority to accepted the offer that he wanted
             */

            try
            {
                HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                getfarmer.EnsureSuccessStatusCode();
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerRes = JsonConvert.DeserializeObject<Farmer>(farmerdata);


                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "LandOrders/" +
                    "GetOrdersByFarmerId/" + ViewBag.FarmerId).Result;
                response.EnsureSuccessStatusCode();
                string data = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<LandOrderDetailsDto>>(data);
                var PendingLandsOffers = res.Where(a => a.LandRentStatus == LandRentStatus.Pending).ToList();

                var LandOrdersImagesDto = new List<LandOrderImageDto>();

                foreach (var item in PendingLandsOffers)
                {


                    HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetLandsByFarmerId/" + ViewBag.FarmerId).Result;

                    if (LandResponse.IsSuccessStatusCode)
                    {
                        string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                        var Lands = JsonConvert.DeserializeObject<List<Land>>(LandData);
                        var WantedLand = Lands.FirstOrDefault(a => a.LandId == item.LandId && a.FarmerId == ViewBag.FarmerId);

                        HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + item.CompanyId).Result;

                        if (CompanyResponse.IsSuccessStatusCode)
                        {
                            string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                            var CompanyResult = JsonConvert.DeserializeObject<Company>(CompanyData);
                            var image = await _fileService.GetUserProfileLatestFileNames("company", CompanyResult.CompanyId);

                            var LandOrderImageDto = new LandOrderImageDto()
                            {
                                Land = WantedLand,
                                Company = CompanyResult,
                                Id = item.Id,
                                LandOrderImageUrl = image.latestImageFileName,
                                LandRentStatus = item.LandRentStatus,
                                LandSize = item.LandSize,
                                OrderStartDate = item.OrderStartDate,
                                OrderEndDate = item.OrderEndDate,
                                OrderPrice = item.OrderPrice
                            };

                            LandOrdersImagesDto.Add(LandOrderImageDto);  
                        }
                        else
                        {
                            TempData["GetAllLandOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("FarmerHomePage", "FarmerServices");
                        }
                    }
                    else 
                    {
                        TempData["GetAllLandOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("FarmerHomePage", "FarmerServices");
                    }
                }

                return View(LandOrdersImagesDto);
            }
            catch (Exception)
            {

                TempData["GetAllLandOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LandOfferDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "LandOrders/GetLandOrderById/" + id).Result;
                LandOfferResponse.EnsureSuccessStatusCode();
                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<LandOrder>(LandOfferData);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + LandOfferResult.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var FarmerResult = JsonConvert.DeserializeObject<Farmer>(FarmerData);
                ViewBag.FarmerId = FarmerResult.FarmerId;

                HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetAllLands").Result;
                LandResponse.EnsureSuccessStatusCode();
                string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                var ListOfLands = JsonConvert.DeserializeObject<List<Land>>(LandData);
                var WantedLand = ListOfLands.FirstOrDefault(a => a.LandId == LandOfferResult.LandId && a.FarmerId == LandOfferResult.FarmerId);

                HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + LandOfferResult.CompanyId).Result;
                CompanyResponse.EnsureSuccessStatusCode();
                string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                var CompanyResult = JsonConvert.DeserializeObject<Company>(CompanyData);
                var image = await _fileService.GetUserProfileLatestFileNames("company", CompanyResult.CompanyId);
                var LandOrderImageDto = new LandOrderImageDto
                {
                    Land = WantedLand,
                    Company = CompanyResult,
                    Id = LandOfferResult.LandOrderId,
                    LandOrderImageUrl = image.latestImageFileName,
                    LandRentStatus = LandOfferResult.LandRentStatus,
                    LandSize = LandOfferResult.LandSize,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate,
                    OrderPrice = LandOfferResult.OrderPrice
                };

                return View(LandOrderImageDto);
            }
            catch (Exception)
            {

                TempData["GetLandOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLandOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeLandOfferAcceptStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "LandOrders/GetLandOrderById/" + OfferId).Result;

            if (LandOfferResponse.IsSuccessStatusCode)
            {
                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<LandOrder>(LandOfferData);

               
                var LandOrderDto = new LandOrderDto()
                {
                    CompanyId = LandOfferResult.CompanyId,
                    FarmerId = LandOfferResult.FarmerId,
                    LandId = LandOfferResult.LandId,
                    LandRentStatus = LandRentStatus.Accepted,
                    LandSize = LandOfferResult.LandSize,
                    OrderPrice = LandOfferResult.OrderPrice,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate
                };

                HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetLandById/" + LandOfferResult.LandId).Result;
                LandResponse.EnsureSuccessStatusCode();
                string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                var Land = JsonConvert.DeserializeObject<Land>(LandData);
                if(LandOfferResult.LandSize <= Land.LandSize)
                {
                    var UpdatedSize = Land.LandSize - LandOfferResult.LandSize;
                    var landDto = new LandDto()
                    {
                        FarmerId = Land.FarmerId,
                        LandSize = UpdatedSize,
                        LandDescribtion = Land.LandDescribtion,
                        LandLocation = Land.LandLocation,
                        LandType = Land.LandType
                    };


                    string data = JsonConvert.SerializeObject(LandOrderDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "LandOrders/UpdateLandOrder/" + OfferId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data2 = JsonConvert.SerializeObject(landDto);
                        StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                        HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "Lands/UpdateLand/" + Land.LandId, content2).Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                            return RedirectToAction("GetAllLandOffers");
                        }
                        else
                        {
                            TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("GetAllLandOffers");
                        }
                    }
                    else
                    {
                        TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("GetAllLandOffers");
                    }
                }
                else
                {
                    TempData["SizeError"] = "لا يمكنك قبول هذا الطلب لعدم توفر المساحه المطلوبه";
                    return RedirectToAction("LandOfferDetails", new { id = OfferId });
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLandOffers");
            }

        }

        [HttpPost]
        public IActionResult ChangeLandOfferRejectStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "LandOrders/GetLandOrderById/" + OfferId).Result;

            if (LandOfferResponse.IsSuccessStatusCode)
            {
                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<LandOrder>(LandOfferData);

                var LandOrderDto = new LandOrderDto()
                {
                    CompanyId = LandOfferResult.CompanyId,
                    FarmerId = LandOfferResult.FarmerId,
                    LandId = LandOfferResult.LandId,
                    LandRentStatus = LandRentStatus.Rejected,
                    LandSize = LandOfferResult.LandSize,
                    OrderPrice = LandOfferResult.OrderPrice,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate
                };
                string data = JsonConvert.SerializeObject(LandOrderDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "LandOrders/UpdateLandOrder/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllLandOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllLandOffers");
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLandOffers");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductOffers()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will display a page which will include all the offers which came from other farmers
            to rent the land which the farmer upload for rent
            and the farmer will have the full authority to accepted the offer that he wanted
             */

            try
            {
                HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                getfarmer.EnsureSuccessStatusCode();
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerRes = JsonConvert.DeserializeObject<Farmer>(farmerdata);


                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "ProductOrders/" +
                    "GetProductsByFarmerId/" + ViewBag.FarmerId).Result;
                response.EnsureSuccessStatusCode();
                string data = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<ProductOrderDetailsDto>>(data);
                var PendingProductsOffers = res.Where(a => a.ProductOffersStatus == ProductOffersStatus.Pending).ToList();

                var ProductOrdersImagesDto = new List<ProductOrderImageDto>();

                foreach (var item in PendingProductsOffers)
                {


                    HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetProductsByFarmerId/" + ViewBag.FarmerId).Result;
                    ProductResponse.EnsureSuccessStatusCode();
                    string ProductData = ProductResponse.Content.ReadAsStringAsync().Result;
                    var Products = JsonConvert.DeserializeObject<List<Product>>(ProductData);
                    var WantedProduct = Products.FirstOrDefault(a => a.ProductName == item.ProductName && a.FarmerId == ViewBag.FarmerId);

                    HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + item.CompanyId).Result;
                    CompanyResponse.EnsureSuccessStatusCode();
                    string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                    var CompanyResult = JsonConvert.DeserializeObject<Company>(CompanyData);
                    var image = await _fileService.GetUserProfileLatestFileNames("company", CompanyResult.CompanyId);

                    var ProductOrderImageDto = new ProductOrderImageDto()
                    {
                        Company = CompanyResult,
                        Product = WantedProduct,
                        ProductName = item.ProductName,
                        ProductOffersStatus = item.ProductOffersStatus,
                        Id = item.Id,
                        OrderPrice = item.OrderPrice,
                        OrderWeight = item.OrderWeight,
                        CompanyImage = image.latestImageFileName
                    };

                    ProductOrdersImagesDto.Add(ProductOrderImageDto);
                }

                return View(ProductOrdersImagesDto);
            }
            catch (Exception)
            {

                TempData["GetAllProductOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ProductOfferDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "ProductOrders/GetProductOrderById/" + id).Result;
                ProductOfferResponse.EnsureSuccessStatusCode();
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<ProductOrder>(ProductOfferData);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ProductOfferResult.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var FarmerResult = JsonConvert.DeserializeObject<Farmer>(FarmerData);
                ViewBag.FarmerId = FarmerResult.FarmerId;

                HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetAllProducts").Result;
                ProductResponse.EnsureSuccessStatusCode();
                string ProductData = ProductResponse.Content.ReadAsStringAsync().Result;
                var ListOfProducts = JsonConvert.DeserializeObject<List<Product>>(ProductData);
                var WantedProduct = ListOfProducts.FirstOrDefault(a => a.ProductName == ProductOfferResult.ProductName && a.FarmerId == ProductOfferResult.FarmerId);

                HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + ProductOfferResult.CompanyId).Result;
                CompanyResponse.EnsureSuccessStatusCode();
                string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                var CompanyResult = JsonConvert.DeserializeObject<Company>(CompanyData);
                var image = await _fileService.GetUserProfileLatestFileNames("company", CompanyResult.CompanyId);
                var ProductOrderImageDto = new ProductOrderImageDto
                {
                    Product = WantedProduct,
                    Company = CompanyResult,
                    Id = ProductOfferResult.ProductOrderId,
                    CompanyImage = image.latestImageFileName,
                    ProductOffersStatus = ProductOfferResult.ProductOffersStatus,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductName = ProductOfferResult.ProductName,
                    OrderPrice = ProductOfferResult.OrderPrice
                };

                return View(ProductOrderImageDto);
            }
            catch (Exception)
            {

                TempData["GetProductOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProductOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeProductOfferAcceptStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "ProductOrders/GetProductOrderById/" + OfferId).Result;

            if (ProductOfferResponse.IsSuccessStatusCode)
            {
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<ProductOrder>(ProductOfferData);

                var ProductOrderDto = new ProductOrderDto()
                {
                    CompanyId = ProductOfferResult.CompanyId,
                    FarmerId = ProductOfferResult.FarmerId,
                    OrderPrice = ProductOfferResult.OrderPrice,
                    ProductName = ProductOfferResult.ProductName,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductOffersStatus = ProductOffersStatus.Accepted
                };

                HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetAllProducts").Result;
                ProductResponse.EnsureSuccessStatusCode();
                string ProductsData = ProductResponse.Content.ReadAsStringAsync().Result;
                var Products = JsonConvert.DeserializeObject<List<Product>>(ProductsData);
                var UnderProcessingProduct = Products.FirstOrDefault(a => a.ProductName == ProductOfferResult.ProductName && a.FarmerId == ViewBag.FarmerId);

                if (ProductOfferResult.OrderWeight <= UnderProcessingProduct.ProductWeight)
                {
                    var UpdatedWeight = UnderProcessingProduct.ProductWeight - ProductOfferResult.OrderWeight;

                    var ProductDto = new ProductDto()
                    {
                        ProductWeight = UpdatedWeight,
                        FarmerId = UnderProcessingProduct.FarmerId,
                        ProductDescribtion = UnderProcessingProduct.ProductDescribtion,
                        ProductName = UnderProcessingProduct.ProductName,
                        ProductPrice = UnderProcessingProduct.ProductPrice,
                        ProductQuality = UnderProcessingProduct.ProductQuality
                    };

                    string data = JsonConvert.SerializeObject(ProductOrderDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "ProductOrders/UpdateProductOrder/" + OfferId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data2 = JsonConvert.SerializeObject(ProductDto);
                        StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                        HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "Products/UpdateProduct/" + UnderProcessingProduct.ProductId, content2).Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                            return RedirectToAction("GetAllProductOffers");
                        }
                        else
                        {
                            TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("GetAllProductOffers");
                        }
                    }
                    else
                    {
                        TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("GetAllProductOffers");
                    } 
                }
                else
                {
                    TempData["WeightError"] = "لا يمكنك قبول هذا الطلب لعدم توفر الكميه في محصولك";
                    return RedirectToAction("ProductOfferDetails", new { id = OfferId });
                }
               
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProductOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeProductOfferRejectStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "ProductOrders/GetProductOrderById/" + OfferId).Result;

            if (ProductOfferResponse.IsSuccessStatusCode)
            {
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<ProductOrder>(ProductOfferData);

                var ProductOrderDto = new ProductOrderDto()
                {
                    CompanyId = ProductOfferResult.CompanyId,
                    FarmerId = ProductOfferResult.FarmerId,
                    OrderPrice = ProductOfferResult.OrderPrice,
                    ProductName = ProductOfferResult.ProductName,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductOffersStatus = ProductOffersStatus.Rejected
                };
                string data = JsonConvert.SerializeObject(ProductOrderDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "ProductOrders/UpdateProductOrder/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllProductOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllProductOffers");
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProductOffers");
            }
        }

        // the transaction between two farmers (Buyer && Seller)

        [HttpGet]
        public async Task<IActionResult> GetAllFarmerLandOffers()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will display a page which will include all the offers which came from other farmers
            to rent the land which the farmer upload for rent
            and the farmer will have the full authority to accepted the offer that he wanted
             */

            try
            {
                HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                getfarmer.EnsureSuccessStatusCode();
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerRes = JsonConvert.DeserializeObject<Farmer>(farmerdata);


                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerLandOrders/" +
                    "GetFarmerOrdersByFarmerId/" + ViewBag.FarmerId).Result;
                response.EnsureSuccessStatusCode();
                string data = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<FarmerLandOrderDetailsDto>>(data);
                var PendingLandsOffers = res.Where(a => a.LandRentStatus == FarmerLandRentStatus.Pending).ToList();

                var LandOrdersImagesDto = new List<FarmerLandOrderImageDto>();

                foreach (var item in PendingLandsOffers)
                {


                    HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetLandsByFarmerId/" + ViewBag.FarmerId).Result;
                    LandResponse.EnsureSuccessStatusCode();
                    string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                    var Lands = JsonConvert.DeserializeObject<List<Land>>(LandData);
                    var WantedLand = Lands.FirstOrDefault(a => a.LandId == item.LandId && a.FarmerId == ViewBag.FarmerId);

                    HttpResponseMessage buyerFarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + item.BuyerFarmerId).Result;
                    buyerFarmerResponse.EnsureSuccessStatusCode();
                    string buyerFarmerData = buyerFarmerResponse.Content.ReadAsStringAsync().Result;
                    var buyerFarmerResult = JsonConvert.DeserializeObject<BuyerFarmer>(buyerFarmerData);
                    var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", buyerFarmerResult.BuyerFarmerId);

                    var LandOrderImageDto = new FarmerLandOrderImageDto()
                    {
                        Land = WantedLand,
                        BuyerFarmer = buyerFarmerResult,
                        Id = item.Id,
                        LandOrderImageUrl = image.latestImageFileName,
                        LandRentStatus = item.LandRentStatus,
                        LandSize = item.LandSize,
                        OrderStartDate = item.OrderStartDate,
                        OrderEndDate = item.OrderEndDate,
                        OrderPrice = item.OrderPrice,
                        FarmerId = ViewBag.FarmerId
                    };

                    LandOrdersImagesDto.Add(LandOrderImageDto);
                }

                return View(LandOrdersImagesDto);
            }
            catch (Exception)
            {

                TempData["GetAllLandOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FarmerLandOfferDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerLandOrders/GetFarmerLandOrderById/" + id).Result;
                LandOfferResponse.EnsureSuccessStatusCode();
                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<FarmerLandOrder>(LandOfferData);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + LandOfferResult.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var FarmerResult = JsonConvert.DeserializeObject<Farmer>(FarmerData);

                HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetAllLands").Result;
                LandResponse.EnsureSuccessStatusCode();
                string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                var ListOfLands = JsonConvert.DeserializeObject<List<Land>>(LandData);
                var WantedLand = ListOfLands.FirstOrDefault(a => a.LandId == LandOfferResult.LandId && a.FarmerId == LandOfferResult.FarmerId);

                HttpResponseMessage buyerFarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + LandOfferResult.BuyerFarmerId).Result;
                buyerFarmerResponse.EnsureSuccessStatusCode();
                string buyerFarmerData = buyerFarmerResponse.Content.ReadAsStringAsync().Result;
                var buyerFarmerResult = JsonConvert.DeserializeObject<BuyerFarmer>(buyerFarmerData);
                var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", buyerFarmerResult.BuyerFarmerId);
                var LandOrderImageDto = new FarmerLandOrderImageDto
                {
                    Land = WantedLand,
                    BuyerFarmer = buyerFarmerResult,
                    Id = LandOfferResult.FarmerLandOrderId,
                    LandOrderImageUrl = image.latestImageFileName,
                    LandRentStatus = LandOfferResult.LandRentStatus,
                    LandSize = LandOfferResult.LandSize,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate,
                    OrderPrice = LandOfferResult.OrderPrice,
                    FarmerId = ViewBag.FarmerId
                };

                return View(LandOrderImageDto);
            }
            catch (Exception)
            {

                TempData["GetLandOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerLandOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeFarmerLandOfferAcceptStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerLandOrders/GetFarmerLandOrderById/" + OfferId).Result;
            if (LandOfferResponse.IsSuccessStatusCode)
            {

                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<FarmerLandOrder>(LandOfferData);

                var LandOrderDto = new FarmerLandOrderDto()
                {
                    BuyerFarmerId = LandOfferResult.BuyerFarmerId,
                    FarmerId = LandOfferResult.FarmerId,
                    LandId = LandOfferResult.LandId,
                    LandRentStatus = FarmerLandRentStatus.Accepted,
                    LandSize = LandOfferResult.LandSize,
                    OrderPrice = LandOfferResult.OrderPrice,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate
                };

                HttpResponseMessage LandResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetLandById/" + LandOfferResult.LandId).Result;
                LandResponse.EnsureSuccessStatusCode();
                string LandData = LandResponse.Content.ReadAsStringAsync().Result;
                var Land = JsonConvert.DeserializeObject<Land>(LandData);
                if(LandOfferResult.LandSize <= Land.LandSize)
                {
                    var UpdatedSize = Land.LandSize - LandOfferResult.LandSize;
                    var landDto = new LandDto()
                    {
                        FarmerId = Land.FarmerId,
                        LandSize = UpdatedSize,
                        LandDescribtion = Land.LandDescribtion,
                        LandLocation = Land.LandLocation,
                        LandType = Land.LandType
                    };

                    string data = JsonConvert.SerializeObject(LandOrderDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerLandOrders/UpdateFarmerLandOrder/" + OfferId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data2 = JsonConvert.SerializeObject(landDto);
                        StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                        HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "Lands/UpdateLand/" + Land.LandId, content2).Result;
                        if (response2.IsSuccessStatusCode)
                        {

                            TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                            return RedirectToAction("GetAllFarmerLandOffers");
                        }
                        else
                        {
                            TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("GetAllFarmerLandOffers");
                        }
                    }
                    else
                    {
                        TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("GetAllFarmerLandOffers");
                    }
                }
                else
                {
                    TempData["LandError"] = "لا يمكنك قبول هذا الطلب لعدم توفر المساحه المطلوبه  ";
                    return RedirectToAction("FarmerLandOfferDetails", new { id = OfferId });
                }

            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerLandOffers");
            }

        }

        [HttpPost]
        public IActionResult ChangeFarmerLandOfferRejectStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage LandOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerLandOrders/GetFarmerLandOrderById/" + OfferId).Result;

            if (LandOfferResponse.IsSuccessStatusCode)
            {
                string LandOfferData = LandOfferResponse.Content.ReadAsStringAsync().Result;
                var LandOfferResult = JsonConvert.DeserializeObject<FarmerLandOrder>(LandOfferData);

                var LandOrderDto = new FarmerLandOrderDto()
                {
                    BuyerFarmerId = LandOfferResult.BuyerFarmerId,
                    FarmerId = LandOfferResult.FarmerId,
                    LandId = LandOfferResult.LandId,
                    LandRentStatus = FarmerLandRentStatus.Rejected,
                    LandSize = LandOfferResult.LandSize,
                    OrderPrice = LandOfferResult.OrderPrice,
                    OrderStartDate = LandOfferResult.OrderStartDate,
                    OrderEndDate = LandOfferResult.OrderEndDate
                };
                string data = JsonConvert.SerializeObject(LandOrderDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerLandOrders/UpdateFarmerLandOrder/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllFarmerLandOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllFarmerLandOffers");
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerLandOffers");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllFarmerProductOffers()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            /*
             this method will display a page which will include all the offers which came from other farmers
            to rent the land which the farmer upload for rent
            and the farmer will have the full authority to accepted the offer that he wanted
             */

            try
            {
                HttpResponseMessage getfarmer = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ViewBag.FarmerId).Result;
                getfarmer.EnsureSuccessStatusCode();
                string farmerdata = getfarmer.Content.ReadAsStringAsync().Result;
                var farmerRes = JsonConvert.DeserializeObject<Farmer>(farmerdata);


                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerProductOrders/" +
                    "GetFarmerProductOrdersByFarmerId/" + ViewBag.FarmerId).Result;
                response.EnsureSuccessStatusCode();
                string data = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<FarmerProductOrderDetailsDto>>(data);
                var PendingProductsOffers = res.Where(a => a.ProductOffersStatus == FarmerProductOffersStatus.Pending).ToList();

                var ProductOrdersImagesDto = new List<FarmerProductOrderImageDto>();

                foreach (var item in PendingProductsOffers)
                {


                    HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetProductsByFarmerId/" + ViewBag.FarmerId).Result;
                    ProductResponse.EnsureSuccessStatusCode();
                    string ProductData = ProductResponse.Content.ReadAsStringAsync().Result;
                    var Products = JsonConvert.DeserializeObject<List<Product>>(ProductData);
                    var WantedProduct = Products.FirstOrDefault(a => a.ProductName == item.ProductName && a.FarmerId == ViewBag.FarmerId);

                    HttpResponseMessage buyerFarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + item.BuyerFarmerId).Result;
                    buyerFarmerResponse.EnsureSuccessStatusCode();
                    string buyerFarmerData = buyerFarmerResponse.Content.ReadAsStringAsync().Result;
                    var buyerFarmerResult = JsonConvert.DeserializeObject<BuyerFarmer>(buyerFarmerData);
                    var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", buyerFarmerResult.BuyerFarmerId);

                    var ProductOrderImageDto = new FarmerProductOrderImageDto()
                    {
                        BuyerFarmer = buyerFarmerResult,
                        Product = WantedProduct,
                        ProductName = item.ProductName,
                        ProductOffersStatus = item.ProductOffersStatus,
                        Id = item.Id,
                        OrderPrice = item.OrderPrice,
                        OrderWeight = item.OrderWeight,
                        BuyerImage = image.latestImageFileName,
                        FarmerId = ViewBag.FarmerId
                    };

                    ProductOrdersImagesDto.Add(ProductOrderImageDto);
                }

                return View(ProductOrdersImagesDto);
            }
            catch (Exception)
            {

                TempData["GetAllProductOffers"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage", "FarmerServices");
            }

        }

        [HttpGet]
        public async Task<IActionResult> FarmerProductOfferDetails(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerProductOrders/GetFarmerProductOrderById/" + id).Result;
                ProductOfferResponse.EnsureSuccessStatusCode();
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<FarmerProductOrder>(ProductOfferData);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + ProductOfferResult.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var FarmerResult = JsonConvert.DeserializeObject<Farmer>(FarmerData);

                HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetAllProducts").Result;
                ProductResponse.EnsureSuccessStatusCode();
                string ProductData = ProductResponse.Content.ReadAsStringAsync().Result;
                var ListOfProducts = JsonConvert.DeserializeObject<List<Product>>(ProductData);
                var WantedProduct = ListOfProducts.FirstOrDefault(a => a.ProductName == ProductOfferResult.ProductName && a.FarmerId == ProductOfferResult.FarmerId);

                HttpResponseMessage BuyerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "BuyerFarmers/GetBuyerFarmerById/" + ProductOfferResult.BuyerFarmerId).Result;
                BuyerResponse.EnsureSuccessStatusCode();
                string BuyerData = BuyerResponse.Content.ReadAsStringAsync().Result;
                var BuyerResult = JsonConvert.DeserializeObject<BuyerFarmer>(BuyerData);
                var image = await _fileService.GetUserProfileLatestFileNames("buyerfarmer", BuyerResult.BuyerFarmerId);
                var ProductOrderImageDto = new FarmerProductOrderImageDto
                {
                    Product = WantedProduct,
                    BuyerFarmer = BuyerResult,
                    Id = ProductOfferResult.FarmerProductOrderId,
                    BuyerImage = image.latestImageFileName,
                    ProductOffersStatus = ProductOfferResult.ProductOffersStatus,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductName = ProductOfferResult.ProductName,
                    OrderPrice = ProductOfferResult.OrderPrice,
                    FarmerId = ViewBag.FarmerId
                };

                return View(ProductOrderImageDto);
            }
            catch (Exception)
            {

                TempData["GetProductOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerProductOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeFarmerProductOfferAcceptStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerProductOrders/GetFarmerProductOrderById/" + OfferId).Result;

            if (ProductOfferResponse.IsSuccessStatusCode)
            {
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<FarmerProductOrder>(ProductOfferData);

                var ProductOrderDto = new FarmerProductOrderDto()
                {
                    BuyerFarmerId = ProductOfferResult.BuyerFarmerId,
                    FarmerId = ProductOfferResult.FarmerId,
                    OrderPrice = ProductOfferResult.OrderPrice,
                    ProductName = ProductOfferResult.ProductName,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductOffersStatus = FarmerProductOffersStatus.Accepted
                };

                HttpResponseMessage ProductResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetAllProducts").Result;
                ProductResponse.EnsureSuccessStatusCode();
                string ProductsData = ProductResponse.Content.ReadAsStringAsync().Result;
                var Products = JsonConvert.DeserializeObject<List<Product>>(ProductsData);
                var UnderProcessingProduct = Products.FirstOrDefault(a => a.ProductName == ProductOfferResult.ProductName && a.FarmerId == ViewBag.FarmerId);

                if (ProductOfferResult.OrderWeight <= UnderProcessingProduct.ProductWeight)
                {
                    var UpdatedWeight = UnderProcessingProduct.ProductWeight - ProductOfferResult.OrderWeight;

                    var ProductDto = new ProductDto()
                    {
                        ProductWeight = UpdatedWeight,
                        FarmerId = UnderProcessingProduct.FarmerId,
                        ProductDescribtion = UnderProcessingProduct.ProductDescribtion,
                        ProductName = UnderProcessingProduct.ProductName,
                        ProductPrice = UnderProcessingProduct.ProductPrice,
                        ProductQuality = UnderProcessingProduct.ProductQuality
                    };

                    string data = JsonConvert.SerializeObject(ProductOrderDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerProductOrders/UpdateFarmerProductOrder/" + OfferId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data2 = JsonConvert.SerializeObject(ProductDto);
                        StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                        HttpResponseMessage response2 = _httpClient.PutAsync(_httpClient.BaseAddress + "Products/UpdateProduct/" + UnderProcessingProduct.ProductId, content2).Result;
                        if (response2.IsSuccessStatusCode)
                        {

                            TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                            return RedirectToAction("GetAllFarmerProductOffers");
                        }
                        else
                        {
                            TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                            return RedirectToAction("GetAllFarmerProductOffers");
                        }
                    }
                    else
                    {
                        TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("GetAllFarmerProductOffers");
                    } 
                }
                else
                {
                    TempData["WeightError"] = "لا يمكنك قبول هذا الطلب لعدم توفر الكميه في محصولك";
                    return RedirectToAction("FarmerProductOfferDetails" , new {id = OfferId});
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerProductOffers");
            }
        }

        [HttpPost]
        public IActionResult ChangeFarmerProductOfferRejectStatus(int OfferId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage ProductOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "FarmerProductOrders/GetFarmerProductOrderById/" + OfferId).Result;

            if (ProductOfferResponse.IsSuccessStatusCode)
            {
                string ProductOfferData = ProductOfferResponse.Content.ReadAsStringAsync().Result;
                var ProductOfferResult = JsonConvert.DeserializeObject<FarmerProductOrder>(ProductOfferData);

                var ProductOrderDto = new FarmerProductOrderDto()
                {
                    BuyerFarmerId = ProductOfferResult.BuyerFarmerId,
                    FarmerId = ProductOfferResult.FarmerId,
                    OrderPrice = ProductOfferResult.OrderPrice,
                    ProductName = ProductOfferResult.ProductName,
                    OrderWeight = ProductOfferResult.OrderWeight,
                    ProductOffersStatus = FarmerProductOffersStatus.Rejected
                };
                string data = JsonConvert.SerializeObject(ProductOrderDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "FarmerProductOrders/UpdateFarmerProductOrder/" + OfferId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["OfferStatusSuccess"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllFarmerProductOffers");
                }
                else
                {
                    TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllFarmerProductOffers");
                }
            }
            else
            {
                TempData["OfferStatusError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmerProductOffers");
            }
        }

        [HttpGet]
        public IActionResult CreateFarmerAccount()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage BanksResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Banks/GetAllBanks").Result;
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
        public IActionResult CreateFarmerAccount(FarmerAccountDto farmerAccountDto)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            var fees = 200;
                var newBalance = farmerAccountDto.AccountBalance - fees;
                farmerAccountDto.AccountBalance = newBalance;
                var userid = HttpContext.Session.GetInt32("UserId");
                var farmerAccount = new FarmerAccount()
                {
                    AccountBalance = farmerAccountDto.AccountBalance,
                    CvvNumber = farmerAccountDto.CvvNumber,
                    ExpireDate = farmerAccountDto.ExpireDate,
                    AccountNumber = farmerAccountDto.AccountNumber,
                    BankId = farmerAccountDto.BankId,
                    FarmerId = (int)HttpContext.Session.GetInt32("UserId")
                };

                string farmerdata = JsonConvert.SerializeObject(farmerAccount);
                StringContent farmercontent = new StringContent(farmerdata, Encoding.UTF8, "application/json");
                HttpResponseMessage farmerresponse = _httpClient.PutAsync(_httpClient.BaseAddress + "CompanyAccounts/AddCompanyAccount", farmercontent).Result;
                if (farmerresponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("FarmerHomePage");
                }
                return View(farmerAccountDto);
          
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipments(string? EquipmentName)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetAllEquipments").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var EquipmentsWithImages = new List<EquipmentImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var Equipments = JsonConvert.DeserializeObject<List<Equipment>>(data);

                    if (EquipmentName == null)
                    {
                        foreach (var Equipment in Equipments)
                        {
                            if (Equipment.FarmerId != ViewBag.FarmerId)
                            {
                                var image = await _fileService.GetUserProfileLatestFileNames("Equipment", Equipment.EquipmentId);
                                var EquipmentWithImage = new EquipmentImageDto()
                                {
                                    Equipment = Equipment,
                                    EquipmentImageUrl = image.latestImageFileName
                                };
                                EquipmentsWithImages.Add(EquipmentWithImage);

                            }
                            else
                            {
                                continue;
                            }
                        }
                        return View(EquipmentsWithImages); 
                    }
                    var FilteredEquipments = Equipments.Where(a => a.EquipmentName.Contains(EquipmentName)).ToList(); 
                    foreach (var Equipment in FilteredEquipments)
                    {
                        if (Equipment.FarmerId != ViewBag.FarmerId)
                        {
                            var image = await _fileService.GetUserProfileLatestFileNames("Equipment", Equipment.EquipmentId);
                            var EquipmentWithImage = new EquipmentImageDto()
                            {
                                Equipment = Equipment,
                                EquipmentImageUrl = image.latestImageFileName
                            };
                            EquipmentsWithImages.Add(EquipmentWithImage);

                        }
                        else
                        {
                            continue;
                        }
                    }
                    return View(EquipmentsWithImages);
                }
                else
                {
                    TempData["GetAllEquipmentsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    RedirectToAction("FarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GetAllEquipmentsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                RedirectToAction("FarmerHomePage");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EquipmentDetails(int id) 
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Equipments/GetEquipmentById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var EquipmentsWithImages = new EquipmentImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var Equipment = JsonConvert.DeserializeObject<Equipment>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("Equipment", Equipment.EquipmentId);
                    EquipmentsWithImages.Equipment = Equipment;
                    EquipmentsWithImages.EquipmentImageUrl = image.latestImageFileName;
                    var RentEquipmentViewModel = new RentEquipmentViewModel()
                    {
                        EquipmentImageDto = EquipmentsWithImages,
                        FarmerId = Equipment.FarmerId,
                        BuyerFarmerId = (int)HttpContext.Session.GetInt32("UserBuyerId"),
                        EquipmentId = Equipment.EquipmentId,
                        EquipmentPrice = Equipment.EquipmentPrice
                    };
                    return View(RentEquipmentViewModel);

                }
                else
                {
                    TempData["GetEquipmentDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    RedirectToAction("GetAllEquipments");
                }
            }
            catch (Exception ex)
            {
                TempData["GetEquipmentDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                RedirectToAction("GetAllEquipments");
            }
            return View();
        }

        [HttpPost]
        public IActionResult RentEquipment(RentEquipmentViewModel rentEquipmentViewModel) 
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");


            var farmerEquipmentDto = new FarmerEquipmentDto()
                {
                    FarmerId = rentEquipmentViewModel.FarmerId,
                    EquipmentId = rentEquipmentViewModel.EquipmentId,
                    RentStartDate = rentEquipmentViewModel.RentStartDate,
                    RentEndDate = rentEquipmentViewModel.RentEndDate,
                    RentPrice = rentEquipmentViewModel.RentPrice,
                    EquipmentRentStatus = EquipmentRentStatus.Pending,
                    BuyerFarmerId = rentEquipmentViewModel.BuyerFarmerId
                    
                };
                string data = JsonConvert.SerializeObject(farmerEquipmentDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "FarmerEquipments/AddFarmerEquipment", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["ActionResultSuccess"] = "تم ارسال طلبك بنجاح";
                    return RedirectToAction("GetAllEquipments");
                }
                else
                {
                TempData["ActionResultError"] = "حدثت مشكله اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllEquipments");

            }
          
        }

        [HttpGet]
        public async Task<ActionResult> FarmerProducts()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Products/GetProductsByFarmerId/" + ViewBag.FarmerId).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string productsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var products = JsonConvert.DeserializeObject<List<ProductDetailsDto>>(productsData);
                    if (products == null)
                    {
                        TempData["FarmerProductResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return View();
                    }
                    //var farmerProducts = products.Where(p => p.FarmerId == farmerId).ToList();
                    var returnedFarmerProducts = new List<ReturnedFarmerProductDto>();
                    foreach (var farmerProduct in products)
                    {
                        var image = await _fileService.GetUserProfileLatestFileNames("product", farmerProduct.Id);
                        var returnedFarmerProduct = new ReturnedFarmerProductDto()
                        {
                            ProductDetails = farmerProduct,
                            ProductImageUrl = image.latestImageFileName
                        };
                        returnedFarmerProducts.Add(returnedFarmerProduct);
                    }
                    return View(returnedFarmerProducts);
                }
                else
                {
                    TempData["FarmerProductResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["FarmerProductResultError"] = "حدث خطا اثناء جاب البيانات التي ترغب في عرضها";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> FarmerProductDetails(int productID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Products/GetProductById/" + productID).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string productsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var product = JsonConvert.DeserializeObject<Product>(productsData);
                    if (product == null)
                    {
                        TempData["FarmerProductResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return View();
                    }
                    var image = await _fileService.GetUserProfileLatestFileNames("product", product.ProductId);
                    var returnedFarmerProducts = new FarmerProductDetailsDto()
                    {
                        ProductDetails = product,
                        ProductImageUrl = image.latestImageFileName
                    };
                    return View(returnedFarmerProducts);
                }
                else
                {
                    TempData["FarmerProductResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                    RedirectToAction("FarmerProducts");
                }

            }
            catch (Exception ex)
            {
                TempData["FarmerProductResultError"] = "حدث خطا اثناء جاب البيانات التي ترغب في عرضها";
                RedirectToAction("FarmerProducts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditMyProduct(FarmerProductDetailsDto farmerProductDetailsDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            var productDto = new ProductDto()
            {
                FarmerId = farmerProductDetailsDto.ProductDetails.FarmerId,
                ProductDescribtion = farmerProductDetailsDto.ProductDetails.ProductDescribtion,
                ProductName = farmerProductDetailsDto.ProductDetails.ProductName,
                ProductPrice = farmerProductDetailsDto.ProductDetails.ProductPrice,
                ProductQuality = farmerProductDetailsDto.ProductDetails.ProductQuality,
                ProductWeight = farmerProductDetailsDto.ProductDetails.ProductWeight
            };

                IFormFile productImageFromView = form.Files["ProductImage"];
                IFormFile productImgae = null;

                if (productImageFromView != null)
                {
                    string data = JsonConvert.SerializeObject(productDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Products/UpdateProduct/" + farmerProductDetailsDto.ProductDetails.ProductId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var originalFileName = productImageFromView.FileName;
                        var newFileName = productDto.ProductName + Path.GetExtension(originalFileName);
                        productImgae = new FormFile(productImageFromView.OpenReadStream(), 0, productImageFromView.Length, productImageFromView.FileName, originalFileName);

                        await _fileService.UploadFile(productImgae, "product", farmerProductDetailsDto.ProductDetails.ProductId);
                        TempData["EditFarmerProductSuccess"] = "تم تعديل بيانات محصولك بنجاح";
                        return RedirectToAction("FarmerProductDetails", new { productID = farmerProductDetailsDto.ProductDetails.ProductId });
                    }
                    else
                    {
                        TempData["EditFarmerProductError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                        return RedirectToAction("FarmerProductDetails", new { productID = farmerProductDetailsDto.ProductDetails.ProductId });
                    }
                }
                else
                {
                    string data = JsonConvert.SerializeObject(productDto);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Products/UpdateProduct/" + farmerProductDetailsDto.ProductDetails.ProductId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["EditFarmerProductSuccess"] = "تم تعديل بيانات محصولك بنجاح";
                        return RedirectToAction("FarmerProductDetails", new { productID = farmerProductDetailsDto.ProductDetails.ProductId });
                    }
                TempData["EditFarmerProductError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerProductDetails", new { productID = farmerProductDetailsDto.ProductDetails.ProductId });

            }

        }

        [HttpGet]
        public async Task<ActionResult> FarmerLands()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Lands/GetLandsByFarmerId/" + ViewBag.FarmerId).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string LandsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var Lands = JsonConvert.DeserializeObject<List<LandDetailsDto>>(LandsData);
                    if (Lands == null)
                    {
                        TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return View();
                    }
                    var returnedFarmerLands = new List<ReturnedFarmerLandDto>();
                    foreach (var farmerLand in Lands)
                    {
                        var image = await _fileService.GetUserProfileLatestFileNames("Land", farmerLand.Id);
                        var returnedFarmerLand = new ReturnedFarmerLandDto()
                        {
                            LandDetails = farmerLand,
                            LandImageUrl = image.latestImageFileName
                        };
                        returnedFarmerLands.Add(returnedFarmerLand);
                    }
                    return View(returnedFarmerLands);
                }
                else
                {
                    TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> FarmerLandDetails(int LandID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Lands/GetLandById/" + LandID).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string LandsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var Land = JsonConvert.DeserializeObject<Land>(LandsData);
                    if (Land == null)
                    {
                        TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return RedirectToAction("FarmerLands");
                    }
                    var image = await _fileService.GetUserProfileLatestFileNames("Land", Land.LandId);
                    var returnedFarmerLands = new FarmerLandDetailsDto()
                    {
                        LandDetails = Land,
                        LandImageUrl = image.latestImageFileName
                    };
                    return View(returnedFarmerLands);
                }
                else
                {
                    TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                    return RedirectToAction("FarmerLands");
                }

            }
            catch (Exception ex)
            {
                TempData["FarmerLandResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                return RedirectToAction("FarmerLands");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMyLand(FarmerLandDetailsDto farmerlandDetailsDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            var landDto = new LandDto()
            {
                FarmerId = farmerlandDetailsDto.LandDetails.FarmerId,
                LandDescribtion = farmerlandDetailsDto.LandDetails.LandDescribtion,
                LandLocation = farmerlandDetailsDto.LandDetails.LandLocation,
                LandType = farmerlandDetailsDto.LandDetails.LandType,
                LandSize = farmerlandDetailsDto.LandDetails.LandSize
            };

            IFormFile landImageFromView = form.Files["LandImage"];
            IFormFile landImgae = null;

            if (landImageFromView != null)
            {
                string data = JsonConvert.SerializeObject(landDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Lands/UpdateLand/" + farmerlandDetailsDto.LandDetails.LandId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var originalFileName = landImageFromView.FileName;
                    var newFileName = landDto.LandLocation + Path.GetExtension(originalFileName);
                    landImgae = new FormFile(landImageFromView.OpenReadStream(), 0, landImageFromView.Length, landImageFromView.FileName, originalFileName);

                    await _fileService.UploadFile(landImgae, "land", farmerlandDetailsDto.LandDetails.LandId);
                    TempData["EditFarmerLandSuccess"] = "تم تعديل بيانات ارضك الزراعيه بنجاح";
                    return RedirectToAction("FarmerLandDetails", new { LandID = farmerlandDetailsDto.LandDetails.LandId });
                }
                else
                {
                    TempData["EditFarmerLandError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerLandDetails", new { LandID = farmerlandDetailsDto.LandDetails.LandId });
                }
            }
            else
            {
                string data = JsonConvert.SerializeObject(landDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Lands/UpdateLand/" + farmerlandDetailsDto.LandDetails.LandId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["EditFarmerLandSuccess"] = "تم تعديل بيانات ارضك الزراعيه بنجاح";
                    return RedirectToAction("FarmerLandDetails", new { LandID = farmerlandDetailsDto.LandDetails.LandId });
                }
                TempData["EditFarmerLandError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerLandDetails", new { LandID = farmerlandDetailsDto.LandDetails.LandId });

            }
        }

        [HttpGet]
        public async Task<ActionResult> FarmerEquipments()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Equipments/GetEquipmentsByFarmerId/" + ViewBag.FarmerId).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string EquipmentsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var Equipments = JsonConvert.DeserializeObject<List<EquipmentDetailsDto>>(EquipmentsData);
                    if (Equipments == null)
                    {
                        TempData["FarmerEquipmentResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return View();
                    }
                    var returnedFarmerEquipments = new List<ReturnedFarmerEquipmentDto>();
                    foreach (var farmerEquipment in Equipments)
                    {
                        var image = await _fileService.GetUserProfileLatestFileNames("Equipment", farmerEquipment.Id);
                        var returnedFarmerEquipment = new ReturnedFarmerEquipmentDto()
                        {
                            EquipmentDetails = farmerEquipment,
                            EquipmentImageUrl = image.latestImageFileName
                        };
                        returnedFarmerEquipments.Add(returnedFarmerEquipment);
                    }
                    return View(returnedFarmerEquipments);
                }
                else
                {
                    TempData["FarmerEquipmentResultError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerEquipments");
                }

            }
            catch (Exception ex)
            {
                TempData["FarmerEquipmentResultError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerEquipments");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FarmerEquipmentDetails(int EquipmentID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");

            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(_httpClient.BaseAddress +
                       "Equipments/GetEquipmentById/" + EquipmentID).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string EquipmentsData = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var Equipment = JsonConvert.DeserializeObject<Equipment>(EquipmentsData);
                    if (Equipment == null)
                    {
                        TempData["FarmerEquipmentResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                        return View();
                    }
                    var image = await _fileService.GetUserProfileLatestFileNames("Equipment", Equipment.EquipmentId);
                    var returnedFarmerEquipments = new FarmerEquipmentWithImageDetailsDto()
                    {
                        EquipmentDetails = Equipment,
                        EquipmentImageUrl = image.latestImageFileName
                    };
                    return View(returnedFarmerEquipments);
                }
                else
                {
                    TempData["FarmerEquipmentResultError"] = "لا يوجد لديك اي محاصيل زراعيه قمت برفعها لدينا";
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMyEquipment(FarmerEquipmentWithImageDetailsDto farmerEquipmentWithImageDetailsDto, IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }

            ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId"); ViewBag.FarmerId = HttpContext.Session.GetInt32("UserId");
            var equipmentDto = new EquipmentDto()
            {
                FarmerId = farmerEquipmentWithImageDetailsDto.EquipmentDetails.FarmerId,
                EquipmentName = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentName,
                EquipmentDescribtion = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentDescribtion,
                EquipmentPrice = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentPrice
            };

            IFormFile equipmentImageFromView = form.Files["LandImage"];
            IFormFile equipmentImgae = null;

            if (equipmentImageFromView != null)
            {
                string data = JsonConvert.SerializeObject(equipmentDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Equipments/UpdateEquipment/" + farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var originalFileName = equipmentImageFromView.FileName;
                    var newFileName = equipmentDto.EquipmentName + Path.GetExtension(originalFileName);
                    equipmentImgae = new FormFile(equipmentImageFromView.OpenReadStream(), 0, equipmentImageFromView.Length, equipmentImageFromView.FileName, originalFileName);

                    await _fileService.UploadFile(equipmentImgae, "equipment", farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId);
                    TempData["EditFarmerEquipmentSuccess"] = "تم تعديل بيانات معدتك الزراعيه بنجاح";
                    return RedirectToAction("FarmerEquipmentDetails", new { EquipmentID = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId });
                }
                else
                {
                    TempData["EditFarmerEquipmentError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerEquipmentDetails", new { EquipmentID = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId });
                }
            }
            else
            {
                string data = JsonConvert.SerializeObject(equipmentDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "Equipments/UpdateEquipment/" + farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["EditFarmerEquipmentSuccess"] = "تم تعديل بيانات معدتك الزراعيه بنجاح";
                    return RedirectToAction("FarmerEquipmentDetails", new { EquipmentID = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId });
                }
                TempData["EditFarmerEquipmentError"] = "حدث خطا ما اثناء معالجه طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerEquipmentDetails", new { EquipmentID = farmerEquipmentWithImageDetailsDto.EquipmentDetails.EquipmentId });

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLands(string? Location)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["SessionError"] = "يرجي تسجيل الدخول او انشاء حساب لك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");
            /*
                this function is related to get the data of all the avaliabe land inside our system and 
                display them to the company to make it able to see all the information which it need aobut
                any land to make it able to rent the specific land which it need
            */
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetAllLands").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var landsWithImages = new List<LandImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var lands = JsonConvert.DeserializeObject<List<Land>>(data);
                    if (Location == null)
                    {
                        foreach (var land in lands)
                        {
                            if (land.FarmerId == HttpContext.Session.GetInt32("UserId"))
                            {
                                continue;
                            }
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
                        if (land.FarmerId == HttpContext.Session.GetInt32("UserId"))
                        {
                            continue;
                        }
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
                    return RedirectToAction("FarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllLandsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage");
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");

            /*
                this function is related to get a data of a selected land from the page where we display all
                the lands data 
                according to the land id where the user do his action we will receive the id and get the data
                of that land which matched with the id
             */

            if (id == 0)
            { return BadRequest("invaild id for any land"); }
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Lands/GetLandById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var landWithImage = new LandImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var land = JsonConvert.DeserializeObject<Land>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("land", land.LandId);
                    landWithImage.Land = land;
                    landWithImage.LandImageUrl = image.latestImageFileName;
                    var LandViewModel = new FarmerRentLandViewModel()
                    {
                        LandImageDto = landWithImage,
                        BuyerFarmerId = (int)HttpContext.Session.GetInt32("UserBuyerId"),
                        LandId = landWithImage.Land.LandId,
                        FarmerId = land.FarmerId,
                        LandSize = land.LandSize,
                    };
                    return View(LandViewModel);

                }
                else
                {
                    TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllLands", "FarmerServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetLandDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllLands", "FarmerServices");
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");
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
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "FarmerLandOrders/AddFarmerLandOrder", content).Result;
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");
            /*
                this function is related to get the data of all the avaliabe products inside our system and 
                display them to the company to make it able to see all the information which it need about
                any product to make it able to but the specific product which it need
            */
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetAllProducts").Result;
                if (respone.IsSuccessStatusCode)
                {
                    var ProductsWithImages = new List<ProductImageDto>();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var Products = JsonConvert.DeserializeObject<List<Product>>(data);
                    if (ProductName == null)
                    {
                        foreach (var Product in Products)
                        {
                            if (Product.FarmerId == HttpContext.Session.GetInt32("UserId"))
                            {
                                continue;
                            }
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
                        return View(ProductsWithImages); 
                    }
                    var FilterdProducts = Products.Where(a => a.ProductName.Contains(ProductName)).ToList();
                    foreach (var Product in FilterdProducts)
                    {
                        if (Product.FarmerId == HttpContext.Session.GetInt32("UserId"))
                        {
                            continue;
                        }
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
                    return View(ProductsWithImages);
                }
                else
                {
                    TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("FarmerHomePage");
                }
            }
            catch (Exception ex)
            {
                TempData["GelAllProductsError"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("FarmerHomePage");
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");
            /*
                this function will get the id of the product where the user show it's data in more details from the 
                last function 
                and according to that id we will display to him a new page include form which make him able
                to fill the data out in that for making a request to buy that product
             */

            if (id == 0)
            { return BadRequest($"this is not a vaild id :- {id}"); }
            try
            {
                HttpResponseMessage respone = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetProductById/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    var productWithImage = new ProductImageDto();
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var product = JsonConvert.DeserializeObject<Product>(data);
                    var image = await _fileService.GetUserProfileLatestFileNames("product", product.ProductId);
                    productWithImage.Product = product;
                    productWithImage.ProductImageUrl = image.latestImageFileName;
                    var ProductViewModel = new FarmerBuyProductViewModel()
                    {
                        FarmerId = product.FarmerId,
                        ProductImageDto = productWithImage,
                        ProductName = product.ProductName,
                        BuyerFarmerId = (int)HttpContext.Session.GetInt32("UserBuyerId"),
                        CurrentWeight = product.ProductWeight,
                        CurrentPrice = product.ProductPrice
                    };
                    return View(ProductViewModel);

                }
                else
                {
                    TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllProducts", "FarmerServices");
                }
            }
            catch (Exception ex)
            {
                TempData["GetProductDetails"] = "حدث خطا اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllProducts", "FarmerServices");
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
            HttpResponseMessage checkResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                 + "LogingUsers/GetLogingUserByEmail/" + HttpContext.Session.GetString("UserEmail")).Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                string checkDate = checkResponse.Content.ReadAsStringAsync().Result;
                var logingUser = JsonConvert.DeserializeObject<LogingUser>(checkDate);

                if (logingUser != null && logingUser.UserRole.ToLower() != "farmer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.BuyerFarmerId = HttpContext.Session.GetInt32("UserBuyerId");
            /*
                this function recive the actual order and actual data that the user enter in the form to make
                a request about renting that land and this request will still pending untill the owner of the land
                change it to any other status either accept or reject
             */

            HttpResponseMessage productmessage = _httpClient.GetAsync(_httpClient.BaseAddress + "Products/GetProductsByFarmerId/" + buyProductViewModel.FarmerId).Result;
            productmessage.EnsureSuccessStatusCode();
            string productdata = productmessage.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<List<Product>>(productdata);
            var WantedProduct = products.FirstOrDefault(a => a.ProductName == buyProductViewModel.ProductName && a.FarmerId == buyProductViewModel.FarmerId);


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
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "FarmerProductOrders/AddFarmerProductOrder", content).Result;
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
    }

}

