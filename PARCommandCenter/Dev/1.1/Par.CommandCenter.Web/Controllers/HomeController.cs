using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly ITokenAcquisition tokenAcquisition;

        public HomeController(ITokenAcquisition tokenAcquisition)
        {
           this.tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var baseUrl = this.Request.GetDisplayUrl();
                return Redirect($"{baseUrl}index");
            });
        }

        [Route("Claims")]
        [HttpGet]
        public IActionResult Claims()
        {
            return View();
        }


        [Route("CallApi")]
        [HttpGet]
        public async Task<IActionResult> CallApi()
        {
            // Acquire the access token.
            string[] scopes = new string[] { "user.read" };
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            accessToken = await this.Request.HttpContext.GetTokenAsync("access_token");

            // Use the access token to call a protected web API.
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync("https://products-web-api.ase-int-prod2.appserviceenvironment.net/api/Products");

            ViewBag.Json = JArray.Parse(content).ToString();

            return View("json");
        }

        //[Route("Profile")]
        ////[AuthorizeForScopes(Scopes = new[] { "user.read" })]
        //public async Task<IActionResult> Profile()
        //{
        //    // Acquire the access token.
        //    string[] scopes = new string[] { "api://2775b3c0-7c92-44c6-8f0c-cc753e48f816/.default" };
        //    string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        //    // Use the access token to call a protected web API.
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    string json = await client.GetStringAsync("https://products-web-api.ase-int-prod2.appserviceenvironment.net/api/Products");

        //    return View(json);
        //}

    }
}
