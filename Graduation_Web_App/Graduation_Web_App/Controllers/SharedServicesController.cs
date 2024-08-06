using Graduation_Web_App.Models;
using Graduation_Web_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;

namespace Graduation_Web_App.Controllers
{
    public class SharedServicesController : Controller
    {
        readonly Uri ApiAddress = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _httpClient;
        dynamic mymodel = new ExpandoObject();
        private readonly IFileService _fileService;
        public SharedServicesController(HttpClient httpClient, IFileService fileService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = ApiAddress;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("HomePage");
        }

        [HttpPost]
        public IActionResult Login(UserView userView)
        {

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress
                + "LogingUsers/GetLogingUserByEmail/" + userView.UserEmail.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<LogingUser>(data);
                if (user.UserRole == "Company" && user.UserPassword == userView.UserPassword)
                {
                    HttpResponseMessage companiesResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                    + "Companies/GetAllCompanies").Result;
                    companiesResponse.EnsureSuccessStatusCode();
                    string companyData = companiesResponse.Content.ReadAsStringAsync().Result;
                    var companies = JsonConvert.DeserializeObject<List<Company>>(companyData);
                    var company = companies.Where(x => x.CompanyEmail == user.UserEmail && x.CompanyPassword == user.UserPassword).First();
                    HttpContext.Session.SetString("UserEmail", company.CompanyEmail);
                    HttpContext.Session.SetString("UserRole", company.CompanyName);
                    HttpContext.Session.SetInt32("UserId", company.CompanyId);
                    return RedirectToAction("CompanyHomePage", "CompaniesServices");
                }
                else if (user.UserRole == "Engineer" && user.UserPassword == userView.UserPassword)
                {
                    HttpResponseMessage engineersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                   + "Engineers/GetAllEngineers").Result;
                    engineersResponse.EnsureSuccessStatusCode();
                    string EngineerData = engineersResponse.Content.ReadAsStringAsync().Result;
                    var companies = JsonConvert.DeserializeObject<List<Engineer>>(EngineerData);
                    var Engineer = companies.Where(x => x.EngineerEmail == user.UserEmail && x.EngineerPassword == user.UserPassword).First();
                    HttpContext.Session.SetString("UserEmail", Engineer.EngineerEmail);
                    HttpContext.Session.SetString("UserRole", Engineer.EngineerName);
                    HttpContext.Session.SetInt32("UserId", Engineer.EngineerId);
                    return RedirectToAction("EngineerHomePage", "EngineersServices");
                }
                else if (user.UserRole == "Farmer" && user.UserPassword == userView.UserPassword)
                {
                    HttpResponseMessage farmersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                   + "Farmers/GetAllFarmers").Result;
                    farmersResponse.EnsureSuccessStatusCode();
                    string FarmerData = farmersResponse.Content.ReadAsStringAsync().Result;
                    var companies = JsonConvert.DeserializeObject<List<Farmer>>(FarmerData);
                    var Farmer = companies.Where(x => x.FarmerEmail == user.UserEmail && x.FarmerPassword == user.UserPassword).First();
                    HttpContext.Session.SetString("UserEmail", Farmer.FarmerEmail);
                    HttpContext.Session.SetString("UserRole", Farmer.FarmerName);
                    HttpContext.Session.SetInt32("UserId", Farmer.FarmerId);

                    HttpResponseMessage buyerFarmersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                  + "BuyerFarmers/GetAllBuyerFarmers").Result;
                    buyerFarmersResponse.EnsureSuccessStatusCode();
                    string BuyerFarmerData = buyerFarmersResponse.Content.ReadAsStringAsync().Result;
                    var buyers = JsonConvert.DeserializeObject<List<BuyerFarmer>>(BuyerFarmerData);
                    var buyersFarmers = buyers.Where(x => x.FarmerEmail == user.UserEmail && x.FarmerPassword == user.UserPassword).First();
                    //HttpContext.Session.SetString("UserBuyerEmail", buyersFarmers.FarmerEmail);
                    //HttpContext.Session.SetString("UserBuyerRole", buyersFarmers.FarmerName);
                    HttpContext.Session.SetInt32("UserBuyerId", buyersFarmers.BuyerFarmerId);

                    return RedirectToAction("FarmerHomePage", "FarmerServices");
                }
                else if (user.UserRole == "BuyerFarmer" && user.UserPassword == userView.UserPassword)
                {
                    HttpResponseMessage farmersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                  + "BuyerFarmers/GetAllBuyerFarmers").Result;
                    farmersResponse.EnsureSuccessStatusCode();
                    string FarmerData = farmersResponse.Content.ReadAsStringAsync().Result;
                    var companies = JsonConvert.DeserializeObject<List<BuyerFarmer>>(FarmerData);
                    var Farmer = companies.Where(x => x.FarmerEmail == user.UserEmail && x.FarmerPassword == user.UserPassword).First();
                    HttpContext.Session.SetString("UserEmail", Farmer.FarmerEmail);
                    HttpContext.Session.SetString("UserRole", Farmer.FarmerName);
                    HttpContext.Session.SetInt32("UserId", Farmer.BuyerFarmerId);

                    return RedirectToAction("BuyerFarmerHomePage", "BuyerFarmers");
                }
                else
                {
                    TempData["SignUpResult"] = "كلمه السر او الايميل قد تكون غير صحيحه ";
                    return RedirectToAction("HomePage");

                }
            }
            else
            {
                TempData["SignUpResult"] = "للاسف ليس لديك حساب لدينا يرجي انشاء حساب لك";
                return RedirectToAction("HomePage");
            }



        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserView userView, IFormCollection form)
        {

            IFormFile userImageFromView = form.Files["UserImage"];
            IFormFile userImgae = null;

            if (userImageFromView != null)
            {
                var originalFileName = userImageFromView.FileName;
                var newFileName = userView.UserName + Path.GetExtension(originalFileName);
                userImgae = new FormFile(userImageFromView.OpenReadStream(), 0,
                    userImageFromView.Length, userImageFromView.FileName, originalFileName);

                if (userView.UserType == UserTypes.Company)
                {
                    HttpResponseMessage checkEmailResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "Companies/GetAllCompanies").Result;
                    checkEmailResponse.EnsureSuccessStatusCode();
                    string emailData = checkEmailResponse.Content.ReadAsStringAsync().Result;
                    var companiesWithEmails = JsonConvert.DeserializeObject<List<Company>>(emailData);

                    if (companiesWithEmails.Any(a => a.CompanyEmail == userView.UserEmail) == true)
                    {
                        TempData["SignUpResultError"] = "هذا الايميل مستخدم يرجي اختيار ايميل اخر";
                        return RedirectToAction("HomePage");
                    }

                    var postedCompany = new CompanyDto()
                    {
                        CompanyAddress = userView.UserAddress,
                        CompanyEmail = userView.UserEmail,
                        CompanyName = userView.UserName,
                        CompanyPassword = userView.UserPassword,
                        CompanyType = userView.CompanyType
                    };

                    string data = JsonConvert.SerializeObject(postedCompany);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage companyResponse = _httpClient.PostAsync(_httpClient.BaseAddress
                        + "Companies/AddCompany", content).Result;

                    if (companyResponse.IsSuccessStatusCode)
                    {
                        HttpResponseMessage companiesResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "Companies/GetAllCompanies").Result;
                        companiesResponse.EnsureSuccessStatusCode();
                        string companydata = companiesResponse.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<List<Company>>(companydata);

                        var wantedCoompany = res.Where(wf => wf.CompanyName == userView.UserName
                        && wf.CompanyEmail == userView.UserEmail && wf.CompanyPassword == userView.UserPassword
                        && wf.CompanyAddress == userView.UserAddress).First();

                        await _fileService.UploadFile(userImgae, "company", wantedCoompany.CompanyId);

                        var logingUser = new LogingUserDto()
                        {
                            UserEmail = userView.UserEmail,
                            UserPassword = userView.UserPassword,
                            UserRole = "Company"
                        };

                        string addToLoginData = JsonConvert.SerializeObject(logingUser);
                        StringContent addToLoginContent = new StringContent(addToLoginData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress
                            + "LogingUsers/AddLogingUser", addToLoginContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SignUpResultSuccess"] = "تمت اضافه حسابك بنجاح يمكنك تسجيل الدخول الان";
                            return RedirectToAction("HomePage", "SharedServices");
                        }
                        else
                        {
                            TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                            return RedirectToAction("HomePage");
                        }

                    }
                    else
                    {
                        TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                        return RedirectToAction("HomePage");
                    }
                }

                else if (userView.UserType == UserTypes.Engineer)
                {
                    HttpResponseMessage checkEmailResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                                    + "Engineers/GetAllEngineers").Result;
                    checkEmailResponse.EnsureSuccessStatusCode();
                    string emailData = checkEmailResponse.Content.ReadAsStringAsync().Result;
                    var Emails = JsonConvert.DeserializeObject<List<Engineer>>(emailData);

                    if (Emails.Any(a => a.EngineerEmail == userView.UserEmail) == true)
                    {
                        TempData["SignUpResultError"] = "هذا الايميل مستخدم يرجي اختيار ايميل اخر";
                        return RedirectToAction("HomePage");
                    }

                    if (Emails.Any(a => a.EngineerPhone == userView.UserPhone))
                    {
                        TempData["SignUpResultError"] = "رقم الهاتف مستخدم يرجي اختيار رقم اخر";
                        return RedirectToAction("HomePage");
                    }

                    var postedEngineer = new EngineerDto()
                    {
                        EngineerAddress = userView.UserAddress,
                        EngineerEmail = userView.UserEmail,
                        EngineerName = userView.UserName,
                        EngineerPassword = userView.UserPassword,
                        EngineerPhone = userView.UserPhone
                    };

                    string data = JsonConvert.SerializeObject(postedEngineer);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage engineerResponse = _httpClient.PostAsync(_httpClient.BaseAddress
                        + "Engineers/AddEngineer", content).Result;

                    if (engineerResponse.IsSuccessStatusCode)
                    {
                        HttpResponseMessage engineersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "Engineers/GetAllEngineers").Result;
                        engineersResponse.EnsureSuccessStatusCode();
                        string engineerdata = engineersResponse.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<List<Engineer>>(engineerdata);

                        var wantedEngineer = res.Where(wf => wf.EngineerName == userView.UserName
                        && wf.EngineerEmail == userView.UserEmail && wf.EngineerPassword == userView.UserPassword
                        && wf.EngineerAddress == userView.UserAddress).First();

                        await _fileService.UploadFile(userImgae, "engineer", wantedEngineer.EngineerId);

                        var logingUser = new LogingUserDto()
                        {
                            UserEmail = userView.UserEmail,
                            UserPassword = userView.UserPassword,
                            UserRole = "Engineer"
                        };

                        string addToLoginData = JsonConvert.SerializeObject(logingUser);
                        StringContent addToLoginContent = new StringContent(addToLoginData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress
                            + "LogingUsers/AddLogingUser", addToLoginContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SignUpResultSuccess"] = "تمت اضافه حسابك بنجاح يمكنك تسجيل الدخول الان";
                            return RedirectToAction("HomePage", "SharedServices");
                        }
                        else
                        {
                            TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                            return RedirectToAction("HomePage");
                        }
                    }
                    else
                    {
                        TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                        return RedirectToAction("HomePage");
                    }
                }

                else if (userView.UserType == UserTypes.Farmer)
                {
                    HttpResponseMessage checkEmailResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                                    + "Farmers/GetAllFarmers").Result;
                    checkEmailResponse.EnsureSuccessStatusCode();
                    string emailData = checkEmailResponse.Content.ReadAsStringAsync().Result;
                    var Emails = JsonConvert.DeserializeObject<List<Farmer>>(emailData);

                    HttpResponseMessage checkEmailResponse2 = _httpClient.GetAsync(_httpClient.BaseAddress
                                                    + "BuyerFarmers/GetAllBuyerFarmers").Result;
                    checkEmailResponse2.EnsureSuccessStatusCode();
                    string emailData2 = checkEmailResponse2.Content.ReadAsStringAsync().Result;
                    var Emails2 = JsonConvert.DeserializeObject<List<BuyerFarmer>>(emailData2);

                    if (Emails.Any(a => a.FarmerEmail == userView.UserEmail) == true && Emails2.Any(a => a.FarmerEmail == userView.UserEmail) == true)
                    {
                        TempData["SignUpResultError"] = "هذا الايميل مستخدم يرجي اختيار ايميل اخر";
                        return RedirectToAction("HomePage");
                    }

                    if (Emails.Any(a => a.FarmerPhone == userView.UserPhone) && Emails2.Any(a => a.FarmerPhone == userView.UserPhone) == true)
                    {
                        TempData["SignUpResultError"] = "رقم الهاتف مستخدم يرجي اختيار رقم اخر";
                        return RedirectToAction("HomePage");
                    }

                    var postedFarmer = new FarmerDto()
                    {
                        FarmerAddress = userView.UserAddress,
                        FarmerEmail = userView.UserEmail,
                        FarmerName = userView.UserName,
                        FarmerPassword = userView.UserPassword,
                        FarmerPhone = userView.UserPhone
                    };

                    string data = JsonConvert.SerializeObject(postedFarmer);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage farmerResponse = _httpClient.PostAsync(_httpClient.BaseAddress
                        + "Farmers/AddFarmer", content).Result;

                    var postedBuyerFarmer = new BuyerFarmerDto()
                    {
                        FarmerAddress = userView.UserAddress,
                        FarmerEmail = userView.UserEmail,
                        FarmerName = userView.UserName,
                        FarmerPassword = userView.UserPassword,
                        FarmerPhone = userView.UserPhone
                    };

                    string data2 = JsonConvert.SerializeObject(postedBuyerFarmer);
                    StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                    HttpResponseMessage farmerResponse2 = _httpClient.PostAsync(_httpClient.BaseAddress
                        + "BuyerFarmers/AddBuyerFarmer", content2).Result;

                    if (farmerResponse.IsSuccessStatusCode && farmerResponse2.IsSuccessStatusCode)
                    {
                        HttpResponseMessage farmersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "Farmers/GetAllFarmers").Result;
                        farmersResponse.EnsureSuccessStatusCode();
                        string farmerdata = farmersResponse.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<List<Farmer>>(farmerdata);

                        var wantedFarmer = res.Where(wf => wf.FarmerName == userView.UserName
                        && wf.FarmerEmail == userView.UserEmail && wf.FarmerPassword == userView.UserPassword
                        && wf.FarmerAddress == userView.UserAddress).First();

                        await _fileService.UploadFile(userImgae, "farmer", wantedFarmer.FarmerId);

                        HttpResponseMessage farmersResponse2 = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "BuyerFarmers/GetAllBuyerFarmers").Result;
                        farmersResponse2.EnsureSuccessStatusCode();
                        string farmerdata2 = farmersResponse2.Content.ReadAsStringAsync().Result;
                        var res2 = JsonConvert.DeserializeObject<List<BuyerFarmer>>(farmerdata2);

                        var wantedFarmer2 = res2.Where(wf => wf.FarmerName == userView.UserName
                        && wf.FarmerEmail == userView.UserEmail && wf.FarmerPassword == userView.UserPassword
                        && wf.FarmerAddress == userView.UserAddress).First();

                        await _fileService.UploadFile(userImgae, "buyerfarmer", wantedFarmer2.BuyerFarmerId);

                        var logingUser = new LogingUserDto()
                        {
                            UserEmail = userView.UserEmail,
                            UserPassword = userView.UserPassword,
                            UserRole = "Farmer"
                        };

                        string addToLoginData = JsonConvert.SerializeObject(logingUser);
                        StringContent addToLoginContent = new StringContent(addToLoginData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress
                            + "LogingUsers/AddLogingUser", addToLoginContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SignUpResultSuccess"] = "تمت اضافه حسابك بنجاح يمكنك تسجيل الدخول الان";
                            return RedirectToAction("HomePage", "SharedServices");
                        }
                        else
                        {
                            TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                            return RedirectToAction("HomePage");
                        }
                    }
                    else
                    {
                        TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                        return RedirectToAction("HomePage");
                    }
                }
                else if (userView.UserType == UserTypes.BuyerFarmer)
                {
                    HttpResponseMessage checkEmailResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                                                    + "BuyerFarmers/GetAllBuyerFarmers").Result;
                    checkEmailResponse.EnsureSuccessStatusCode();
                    string emailData = checkEmailResponse.Content.ReadAsStringAsync().Result;
                    var Emails = JsonConvert.DeserializeObject<List<BuyerFarmer>>(emailData);

                    if (Emails.Any(a => a.FarmerEmail == userView.UserEmail) == true)
                    {
                        TempData["SignUpResultError"] = "هذا الايميل مستخدم يرجي اختيار ايميل اخر";
                        return RedirectToAction("HomePage");
                    }

                    if (Emails.Any(a => a.FarmerPhone == userView.UserPhone))
                    {
                        TempData["SignUpResultError"] = "رقم الهاتف مستخدم يرجي اختيار رقم اخر";
                        return RedirectToAction("HomePage");
                    }

                    var postedFarmer = new BuyerFarmerDto()
                    {
                        FarmerAddress = userView.UserAddress,
                        FarmerEmail = userView.UserEmail,
                        FarmerName = userView.UserName,
                        FarmerPassword = userView.UserPassword,
                        FarmerPhone = userView.UserPhone
                    };

                    string data = JsonConvert.SerializeObject(postedFarmer);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage farmerResponse = _httpClient.PostAsync(_httpClient.BaseAddress
                        + "BuyerFarmers/AddBuyerFarmer", content).Result;

                    if (farmerResponse.IsSuccessStatusCode)
                    {
                        HttpResponseMessage farmersResponse = _httpClient.GetAsync(_httpClient.BaseAddress
                            + "BuyerFarmers/GetAllBuyerFarmers").Result;
                        farmersResponse.EnsureSuccessStatusCode();
                        string farmerdata = farmersResponse.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<List<BuyerFarmer>>(farmerdata);

                        var wantedFarmer = res.Where(wf => wf.FarmerName == userView.UserName
                        && wf.FarmerEmail == userView.UserEmail && wf.FarmerPassword == userView.UserPassword
                        && wf.FarmerAddress == userView.UserAddress).First();

                        await _fileService.UploadFile(userImgae, "buyerfarmer", wantedFarmer.BuyerFarmerId);

                        var logingUser = new LogingUserDto()
                        {
                            UserEmail = userView.UserEmail,
                            UserPassword = userView.UserPassword,
                            UserRole = "BuyerFarmer"
                        };

                        string addToLoginData = JsonConvert.SerializeObject(logingUser);
                        StringContent addToLoginContent = new StringContent(addToLoginData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress
                            + "LogingUsers/AddLogingUser", addToLoginContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SignUpResultSuccess"] = "تمت اضافه حسابك بنجاح يمكنك تسجيل الدخول الان";
                            return RedirectToAction("HomePage", "SharedServices");
                        }
                        else
                        {
                            TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                            return RedirectToAction("HomePage");
                        }
                    }
                    else
                    {
                        TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                        return RedirectToAction("HomePage");
                    }

                }
                else
                {
                    TempData["SignUpResultError"] = "حدث خطا ما اثناء اتمام طلبك";
                    return RedirectToAction("HomePage");
                }

            }
            else
            {
                TempData["SignUpResultError"] = "يرجي اضافه صوره شخصيه من اجل مصداقيتك";
                return RedirectToAction("HomePage");
            }
        }
    }
}
