using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Web1.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();

            // client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/StatelessBackendService/");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)

            {

                //throw new Exception("500 Internal Server Error occurred");
                throw new Exception($"500 Internal Server Error occurred.\r\nCorrelation Id: {Activity.Current.RootId}");

            }

            var responseBody = response.Content.ReadAsStringAsync().Result;

            dynamic responseBodyJSON = JsonConvert.DeserializeObject(responseBody);
            string number = responseBodyJSON.count;



            ViewBag.Number = number;



            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            ViewBag.CorrelationId = Activity.Current.RootId;

            return View();
        }
    }
}
