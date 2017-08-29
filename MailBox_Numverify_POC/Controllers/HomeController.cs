using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Net.Http;
using System.Net.Http.Headers;
using MailBox_Numverify_POC.Models;
using Newtonsoft.Json;

namespace MailBox_Numverify_POC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }
		public ActionResult NumVerify([FromBody]FormNumber phone)
		{

			using (var client = HttpClientFactory.Create())
			{
				var BaseUrl = "http://apilayer.net/";
				var accessToken = "78cfd17470c7ae97205fdab21a172225";
                string url = BaseUrl + "api/validate?access_key=" + accessToken + "&number=" + phone.Number + "&country_code="+phone.CountryCode +"&smtp=1&format=1";
				//Passing service base url  
				client.BaseAddress = new Uri(BaseUrl);

				client.DefaultRequestHeaders.Clear();
				//Define request data format  
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//Sending request to find web api REST service resource GetAllEmployees using HttpClient  
				HttpResponseMessage response = client.GetAsync(url).Result;
				var numInfo = new Number();
				//Checking the response is successful or not which is sent using HttpClient  
				if (response.IsSuccessStatusCode)
				{
					//Storing the response details recieved from web api   
					var APIResponse = response.Content.ReadAsStringAsync().Result;

					//Deserializing the response recieved from web api and storing into the Employee list  
                    numInfo = JsonConvert.DeserializeObject<Number>(APIResponse);
				}
                //returning the employee list to view 
                if(numInfo.valid){
                    return Json("Number valid and the location is "+numInfo.location+" and the carrier is "+numInfo.carrier, JsonRequestBehavior.AllowGet);
				}
                else{
                    return Json("Number invalid", JsonRequestBehavior.AllowGet);
                }
			}
		}
		
        public ActionResult MailboxLayer([FromBody]FormMail mail)
		{
			//var email = HttpContext.Request.Params.Get("email");
            using (var client = HttpClientFactory.Create())
			{
                var BaseUrl = "http://apilayer.net/";
                var accessToken = "ef162e6dcffdb79586637208f3037a56";
                string url = BaseUrl + "api/check?access_key=" + accessToken + "&email=" + mail.email + "&smtp=1&format=1";
				//Passing service base url  
                client.BaseAddress = new Uri(BaseUrl);

				client.DefaultRequestHeaders.Clear();
				//Define request data format  
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//Sending request to find web api REST service resource GetAllEmployees using HttpClient  
				HttpResponseMessage response = client.GetAsync(url).Result;
                var emailInfo = new Email();
				//Checking the response is successful or not which is sent using HttpClient  
				if (response.IsSuccessStatusCode)
				{
					//Storing the response details recieved from web api   
					var APIResponse = response.Content.ReadAsStringAsync().Result;

					//Deserializing the response recieved from web api and storing into the Employee list  
                    emailInfo = JsonConvert.DeserializeObject<Email>(APIResponse);
				}
				//returning the employee list to view  
                if (emailInfo.free && emailInfo.smtp_check)
				{
					return Json("This is a free email and it actually exists", JsonRequestBehavior.AllowGet);
				}
				else if(!emailInfo.free && emailInfo.smtp_check)
				{
					return Json("This is coorporate email and it actually exists", JsonRequestBehavior.AllowGet);
				}
				if (emailInfo.free && !emailInfo.smtp_check)
				{
					return Json("This is a free email and it does not exists", JsonRequestBehavior.AllowGet);
				}
				else if (!emailInfo.free && !emailInfo.smtp_check)
				{
					return Json("This is coorporate email and it does not exists", JsonRequestBehavior.AllowGet);
				}
                else
                {
                    return Json("Something went wrong :(");
                }
			}
		}
    }
}
