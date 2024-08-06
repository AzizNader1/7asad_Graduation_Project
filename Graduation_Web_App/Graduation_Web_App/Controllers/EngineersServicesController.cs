using Graduation_Web_App.Models;
using Graduation_Web_App.Services;
using GraduationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Text;

namespace Graduation_Web_App.Controllers
{
    public class EngineersServicesController : Controller
    {
        readonly Uri ApiAddress = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _httpClient;
        dynamic mymodel = new ExpandoObject();
        private readonly IFileService _fileService;
        public EngineersServicesController(HttpClient httpClient, IFileService fileService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = ApiAddress;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult EngineerHomePage() 
        {
            /* 
              this page represent the home page of the engineer and it is the start point which make the engineer
              able to go through other pages from this point 
              inside this page we first check about is there is any avalible session to the engineer who want
              to access this page or not 
              second we check about if the id which the user get fromt the login page still exist in the database or not
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EngineerProfile(int id)
        {
            /*
                this page is the get view which will display to the engineer his own data inside the profile
                and he will review his own data and has to options in this page edit profile or logout
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage engineerResponse = _httpClient.GetAsync(ApiAddress + "Engineers/GetEngineerById/" + HttpContext.Session.GetInt32("UserId")).Result;

            if (engineerResponse.IsSuccessStatusCode)
            {
                string engineerData = engineerResponse.Content.ReadAsStringAsync().Result;
                var engineer = JsonConvert.DeserializeObject<Engineer>(engineerData);

                var engineerWithImage = new EngineerImageDto();

                var image = await _fileService.GetUserProfileLatestFileNames("engineer", id);

                engineerWithImage.Engineer = engineer;
                engineerWithImage.EngineerImageUrl = image.latestImageFileName;
                return View(engineerWithImage); 
            }
            else
            {
                TempData["GetEngineerProfileError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("EngineerHomePage");
            }
        }

        [HttpGet]
        public IActionResult EditProfile()
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");

            HttpResponseMessage EngineerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Engineers/GetEngineerById/" + HttpContext.Session.GetInt32("UserId")).Result;

            if (EngineerResponse.IsSuccessStatusCode)
            {
                string EngineerData = EngineerResponse.Content.ReadAsStringAsync().Result;
                var Engineer = JsonConvert.DeserializeObject<Engineer>(EngineerData);
                var EngineerDto = new EngineerDto()
                {
                    EngineerAddress = Engineer.EngineerAddress,
                    EngineerEmail = Engineer.EngineerEmail,
                    EngineerName = Engineer.EngineerName,
                    EngineerPassword = Engineer.EngineerPassword,
                    EngineerPhone = Engineer.EngineerPhone
                };

                return View(EngineerDto); 
            }
            else
            {
                TempData["GetEditEngineerProfileError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("EngineerProfile", new {id= ViewBag.EngineerId});
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EngineerDto engineerDto, IFormCollection form)
        {
            /*
             this method is related to the request which came from the engineer to edit the profile data
            which include engineername, email, address, and all other data
             */
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            try
            {
                IFormFile EngineerImageFromView = form.Files["EngineerImage"];
                IFormFile EngineerImgae = null;

                if (EngineerImageFromView != null)
                {
                    string dtoData = JsonConvert.SerializeObject(engineerDto);
                    StringContent content = new StringContent(dtoData, Encoding.UTF8, "application/json");
                    HttpResponseMessage dtoResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "Engineers/UpdateEngineer/" + HttpContext.Session.GetInt32("UserId"), content).Result;
                    if (dtoResponse.IsSuccessStatusCode)
                    {
                        var originalFileName = EngineerImageFromView.FileName;
                        var newFileName = engineerDto.EngineerName + Path.GetExtension(originalFileName);
                        EngineerImgae = new FormFile(EngineerImageFromView.OpenReadStream(), 0, EngineerImageFromView.Length, EngineerImageFromView.FileName, originalFileName);

                        await _fileService.UploadFile(EngineerImgae, "Engineer", (int)HttpContext.Session.GetInt32("UserId"));
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = engineerDto.EngineerEmail,
                            UserPassword = engineerDto.EngineerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("EngineerHomePage");
                        }
                        else
                        {
                            TempData["EditEngineerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(engineerDto);
                        }
                    }
                    else
                    {
                        TempData["EditEngineerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                        return View(engineerDto);
                    }
                }
                else
                {
                    string dtoData2 = JsonConvert.SerializeObject(engineerDto);
                    StringContent content2 = new StringContent(dtoData2, Encoding.UTF8, "application/json");
                    HttpResponseMessage dtoResponse2 = _httpClient.PutAsync(_httpClient.BaseAddress + "Engineers/UpdateEngineer/" + HttpContext.Session.GetInt32("UserId"), content2).Result;
                    if (dtoResponse2.IsSuccessStatusCode)
                    {
                        var logingUserDto = new LogingUserDto()
                        {
                            UserEmail = engineerDto.EngineerEmail,
                            UserPassword = engineerDto.EngineerPassword,
                            UserRole = logingUser.UserRole
                        };

                        string logingUserData = JsonConvert.SerializeObject(logingUserDto);
                        StringContent logignUserContent = new StringContent(logingUserData, Encoding.UTF8, "application/json");
                        HttpResponseMessage logignUserResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "LogingUsers/UpdateLogingUser/" + logingUser.LogingUserId, logignUserContent).Result;
                        if (logignUserResponse.IsSuccessStatusCode)
                        {

                            HttpContext.Session.SetString("UserEmail", logingUserDto.UserEmail);
                            TempData["EditResult"] = "تم تعديل بيانات حسابك الشخصي بنجاح";
                            return RedirectToAction("EngineerHomePage");
                        }
                        else
                        {
                            TempData["EditCompanyError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                            return View(engineerDto);
                        }
                    }
                    TempData["EditEngineerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                    return View(engineerDto);
                }
            }
            
            catch (Exception e)
            {
                TempData["EditEngineerError"] = "حدث خطأ ما اثناء ارسال طلبك يرجي المحاوله في وقت لاحق نعتزر لذلك الخطأ";
                return View(engineerDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFarmersOffers(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");

            //get all services orders for this Engineer by using the data of the Engineer which we get from above method (EngineerFarmers)

            HttpResponseMessage getEngineerFarmers = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerFarmers/GetEngineerFarmersByEngineerId/" + id).Result;

            if (getEngineerFarmers.IsSuccessStatusCode)
            {
                string EngineerFarmersdata = getEngineerFarmers.Content.ReadAsStringAsync().Result;
                var EngineerFarmersRes = JsonConvert.DeserializeObject<List<EngineerFarmerDetailsDto>>(EngineerFarmersdata);
                var PendingOrders = EngineerFarmersRes.Where(EF => EF.Status == ServiceStatusEF.Pending).ToList();
                var EngineerFarmersWithImageDto = new List<EngineerFarmerImageDto>();
                if (PendingOrders.Count == 0)
                {
                    return View(EngineerFarmersWithImageDto);
                }
                foreach (var order in PendingOrders)
                {
                    var image = await _fileService.GetUserProfileLatestFileNames("farmer", order.FarmerId);

                    HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + order.FarmerId).Result;
                    FarmerResponse.EnsureSuccessStatusCode();
                    string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                    var Farmer = JsonConvert.DeserializeObject<Farmer>(FarmerData);
                    var EngineerFarmerImageDto = new EngineerFarmerImageDto
                    {

                        EngineerName = order.EngineerName,
                        EngnieerId = order.EngnieerId,
                        Id = order.Id,
                        ServicePrice = order.ServicePrice,
                        ServiveDate = order.ServiveDate,
                        FarmerImage = image.latestImageFileName,
                        Farmer = Farmer,
                    };
                    EngineerFarmersWithImageDto.Add(EngineerFarmerImageDto);
                }
                return View(EngineerFarmersWithImageDto); 
            }
            else
            {
                TempData["GetFarmersOffersError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("EngineerHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FarmerOfferDetails(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerFarmerOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerFarmers/GetEngineerFarmerById/" + id).Result;

            if (EngineerFarmerOfferResponse.IsSuccessStatusCode)
            {
                string EngineerFarmerOfferData = EngineerFarmerOfferResponse.Content.ReadAsStringAsync().Result;
                var EngineerFarmerOffer = JsonConvert.DeserializeObject<EngineerFarmer>(EngineerFarmerOfferData);

                var image = await _fileService.GetUserProfileLatestFileNames("farmer", EngineerFarmerOffer.FarmerId);

                HttpResponseMessage FarmerResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Farmers/GetFarmerById/" + EngineerFarmerOffer.FarmerId).Result;
                FarmerResponse.EnsureSuccessStatusCode();
                string FarmerData = FarmerResponse.Content.ReadAsStringAsync().Result;
                var Farmer = JsonConvert.DeserializeObject<Farmer>(FarmerData);
                var EngineerFarmerImageDto = new EngineerFarmerImageDto
                {

                    EngnieerId = EngineerFarmerOffer.EngineerId,
                    ServicePrice = EngineerFarmerOffer.ServicePrice,
                    ServiveDate = EngineerFarmerOffer.ServiveDate,
                    Id = EngineerFarmerOffer.EngineerFarmerId,
                    FarmerImage = image.latestImageFileName,
                    Farmer = Farmer,
                };
                return View(EngineerFarmerImageDto); 
            }
            else
            {
                TempData["GetFarmerOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmersOffers" , new {id = ViewBag.EngineerId});
            }

        }

        [HttpPost]
        public IActionResult ChangeFarmerOfferAcceptStatus(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerFarmerOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerFarmers/GetEngineerFarmerById/" + id).Result;

            if (EngineerFarmerOfferResponse.IsSuccessStatusCode)
            {
                string EngineerFarmerOfferData = EngineerFarmerOfferResponse.Content.ReadAsStringAsync().Result;
                var EngineerFarmerOffer = JsonConvert.DeserializeObject<EngineerFarmer>(EngineerFarmerOfferData);

                var EngineerFarmerDto = new EngineerFarmerDto()
                {
                    ServiveDate = EngineerFarmerOffer.ServiveDate,
                    ServicePrice = EngineerFarmerOffer.ServicePrice,
                    Status = ServiceStatusEF.Accepted,
                    EngnieerId = EngineerFarmerOffer.EngineerId,
                    FarmerId = EngineerFarmerOffer.FarmerId

                };

                string dtoData = JsonConvert.SerializeObject(EngineerFarmerDto);
                StringContent content = new StringContent(dtoData, Encoding.UTF8, "application/json");
                HttpResponseMessage dtoResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "EngineerFarmers/UpdateEngineerFarmer/" + id, content).Result;
                if (dtoResponse.IsSuccessStatusCode)
                {
                    TempData["OfferStatus"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllFarmersOffers", new { id = EngineerFarmerOffer.EngineerId });
                }
                else
                {
                    TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllFarmersOffers", new { id = ViewBag.EngineerId });
                } 
            }
            else
            {
                TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmersOffers", new { id = ViewBag.EngineerId });
            }
        }

        [HttpPost]
        public IActionResult ChangeFarmerOfferRejectStatus(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerFarmerOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerFarmers/GetEngineerFarmerById/" + id).Result;

            if (EngineerFarmerOfferResponse.IsSuccessStatusCode)
            {
                string EngineerFarmerOfferData = EngineerFarmerOfferResponse.Content.ReadAsStringAsync().Result;
                var EngineerFarmerOffer = JsonConvert.DeserializeObject<EngineerFarmer>(EngineerFarmerOfferData);

                var EngineerFarmerDto = new EngineerFarmerDto()
                {
                    ServiveDate = EngineerFarmerOffer.ServiveDate,
                    ServicePrice = EngineerFarmerOffer.ServicePrice,
                    Status = ServiceStatusEF.Rejected,
                    EngnieerId = EngineerFarmerOffer.EngineerId,
                    FarmerId = EngineerFarmerOffer.FarmerId

                };

                string dtoData = JsonConvert.SerializeObject(EngineerFarmerDto);
                StringContent content = new StringContent(dtoData, Encoding.UTF8, "application/json");
                HttpResponseMessage dtoResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "EngineerFarmers/UpdateEngineerFarmer/" + id, content).Result;
                if (dtoResponse.IsSuccessStatusCode)
                {
                    TempData["OfferStatus"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllFarmersOffers", new { id = EngineerFarmerOffer.EngineerId });
                }
                else
                {
                    TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllFarmersOffers", new { id = ViewBag.EngineerId });
                } 
            }
            else
            {
                TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllFarmersOffers", new { id = ViewBag.EngineerId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompaniesOffers(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");

            //get all services orders for this Engineer by using the data of the Engineer which we get from above method (EngineerCompanies)

            HttpResponseMessage getEngineerCompany = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerCompanies/GetEngineerCompanysByEngineerId/" + id).Result;

            if (getEngineerCompany.IsSuccessStatusCode)
            {
                string EngineerCompaniesdata = getEngineerCompany.Content.ReadAsStringAsync().Result;
                var EngineerCompaniesRes = JsonConvert.DeserializeObject<List<EngineerCompanyDetailsDto>>(EngineerCompaniesdata);
                var PendingOrders = EngineerCompaniesRes.Where(EF => EF.Status == ServiceStatusEC.Pending).ToList();
                var EngineerCompaniesWithImageDto = new List<EngineerCompanyImageDto>();
                if (PendingOrders.Count == 0)
                {
                    return View(EngineerCompaniesWithImageDto);
                }
                foreach (var order in PendingOrders)
                {
                    var image = await _fileService.GetUserProfileLatestFileNames("company", order.CompanyId);

                    HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + order.CompanyId).Result;
                    CompanyResponse.EnsureSuccessStatusCode();
                    string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                    var Company = JsonConvert.DeserializeObject<Company>(CompanyData);
                    var EngineerCompanyImageDto = new EngineerCompanyImageDto
                    {

                        EngineerId = order.EngineerId,
                        ServicePrice = order.ServicePrice,
                        ServiveDate = order.ServiveDate,
                        Id = order.Id,
                        EngineerName = order.EngineerName,
                        CompanyImage = image.latestImageFileName,
                        Compnay = Company,
                    };
                    EngineerCompaniesWithImageDto.Add(EngineerCompanyImageDto);
                }
                return View(EngineerCompaniesWithImageDto); 
            }
            else
            {
                TempData["GetCompaniesOffersError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("EngineerHomePage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CompanyOfferDetails(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerCompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerCompanies/GetEngineerCompanyById/" + id).Result;

            if (EngineerCompanyResponse.IsSuccessStatusCode)
            {
                string EngineerCompanyData = EngineerCompanyResponse.Content.ReadAsStringAsync().Result;
                var order = JsonConvert.DeserializeObject<EngineerCompany>(EngineerCompanyData);
                var image = await _fileService.GetUserProfileLatestFileNames("company", order.CompanyId);

                HttpResponseMessage CompanyResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Companies/GetCompanyById/" + order.CompanyId).Result;
                CompanyResponse.EnsureSuccessStatusCode();
                string CompanyData = CompanyResponse.Content.ReadAsStringAsync().Result;
                var Company = JsonConvert.DeserializeObject<Company>(CompanyData);
                var EngineerCompanyImageDto = new EngineerCompanyImageDto
                {

                    EngineerId = order.EngineerId,
                    ServicePrice = order.ServicePrice,
                    ServiveDate = order.ServiveDate,
                    Id = order.EngineerCompanyId,
                    CompanyImage = image.latestImageFileName,
                    Compnay = Company,
                };
                return View(EngineerCompanyImageDto); 
            }
            else
            {
                TempData["GetCompanyOfferDetailsError"] = "حدث خطا ما اثناء جلب البيانات التي ترغب في عرضها يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllCompaniesOffers", new { id = ViewBag.EngineerId });
            }

        }

        [HttpPost]
        public IActionResult ChangeCompanyOfferAcceptStatus(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerCompanyOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerCompanies/GetEngineerCompanyById/" + id).Result;

            if (EngineerCompanyOfferResponse.IsSuccessStatusCode)
            {
                string EngineerCompanyOfferData = EngineerCompanyOfferResponse.Content.ReadAsStringAsync().Result;
                var EngineerCompanyOffer = JsonConvert.DeserializeObject<EngineerCompany>(EngineerCompanyOfferData);

                var EngineerCompanyDto = new EngineerCompanyDto()
                {
                    CompanyId = EngineerCompanyOffer.CompanyId,
                    EngineerId = EngineerCompanyOffer.EngineerId,
                    ServicePrice = EngineerCompanyOffer.ServicePrice,
                    ServiveDate = EngineerCompanyOffer.ServiveDate,
                    Status = ServiceStatusEC.Accepted
                };

                string dtoData = JsonConvert.SerializeObject(EngineerCompanyDto);
                StringContent content = new StringContent(dtoData, Encoding.UTF8, "application/json");
                HttpResponseMessage dtoResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "EngineerCompanies/UpdateEngineerCompany/" + id, content).Result;
                if (dtoResponse.IsSuccessStatusCode)
                {
                    TempData["OfferStatus"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllCompaniesOffers", new { id = EngineerCompanyOffer.EngineerId });
                }
                else
                {
                    TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllCompaniesOffers", new { id = ViewBag.EngineerId });
                } 
            }
            else
            {
                TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllCompaniesOffers", new { id = ViewBag.EngineerId });
            }
        }

        [HttpPost]
        public IActionResult ChangeCompanyOfferRejectStatus(int id)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            HttpResponseMessage EngineerCompanyOfferResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "EngineerCompanies/GetEngineerCompanyById/" + id).Result;

            if (EngineerCompanyOfferResponse.IsSuccessStatusCode)
            {
                string EngineerCompanyOfferData = EngineerCompanyOfferResponse.Content.ReadAsStringAsync().Result;
                var EngineerCompanyOffer = JsonConvert.DeserializeObject<EngineerCompany>(EngineerCompanyOfferData);

                var EngineerCompanyDto = new EngineerCompanyDto()
                {
                    CompanyId = EngineerCompanyOffer.CompanyId,
                    EngineerId = EngineerCompanyOffer.EngineerId,
                    ServicePrice = EngineerCompanyOffer.ServicePrice,
                    ServiveDate = EngineerCompanyOffer.ServiveDate,
                    Status = ServiceStatusEC.Rejected
                };

                string dtoData = JsonConvert.SerializeObject(EngineerCompanyDto);
                StringContent content = new StringContent(dtoData, Encoding.UTF8, "application/json");
                HttpResponseMessage dtoResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "EngineerCompanies/UpdateEngineerCompany/" + id, content).Result;
                if (dtoResponse.IsSuccessStatusCode)
                {
                    TempData["OfferStatus"] = "تم تنفيذ طلبك بنجاح";
                    return RedirectToAction("GetAllCompaniesOffers", new { id = EngineerCompanyOffer.EngineerId });
                }
                else
                {
                    TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                    return RedirectToAction("GetAllCompaniesOffers", new { id = ViewBag.EngineerId });
                } 
            }
            else
            {
                TempData["OfferStatusError"] = "حدث خطا اثناء تنفيذ طلبك يرجي المحاوله مره اخري لاحقا";
                return RedirectToAction("GetAllCompaniesOffers", new { id = ViewBag.EngineerId });
            }
        }

        [HttpGet]
        public IActionResult EngineerBankAccount()
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


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
        public IActionResult EngineerBankAccount(EngineerAccountDto engineerAccountDto)
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

                if (logingUser != null && logingUser.UserRole.ToLower() != "engineer")
                {
                    return Unauthorized("You Haven't Authorizations Which Make You Able To Go Into This Page");
                }
            }
            else
            {
                TempData["SessionError"] = "للاسف لا يوجد لدينا بيانات اليك يرجي انشاء حساب ليك اولا";
                return RedirectToAction("HomePage", "SharedServices");
            }
            ViewBag.EngineerId = HttpContext.Session.GetInt32("UserId");


            var fees = 200;
            var newBalance = engineerAccountDto.AccountBalance - fees;
            engineerAccountDto.AccountBalance = newBalance;
            var userid = HttpContext.Session.GetInt32("UserId");
            var engineerAccount = new EngineerAccount()
            {
                AccountBalance = engineerAccountDto.AccountBalance,
                CvvNumber = engineerAccountDto.CvvNumber,
                ExpireDate = engineerAccountDto.ExpireDate,
                AccountNumber = engineerAccountDto.AccountNumber,
                BankId = engineerAccountDto.BankId,
                EngineerId = (int)HttpContext.Session.GetInt32("UserId")
            };

            string engineerdata = JsonConvert.SerializeObject(engineerAccount);
            StringContent engineercontent = new StringContent(engineerdata, Encoding.UTF8, "application/json");
            HttpResponseMessage engineerresponse = _httpClient.PutAsync(_httpClient.BaseAddress + "EngineerAccounts/AddCompanyAccount", engineercontent).Result;
            if (engineerresponse.IsSuccessStatusCode)
            {
                return RedirectToAction("EngineerHomePage");
            }
            return View(engineerAccountDto);
        }

    }
}
