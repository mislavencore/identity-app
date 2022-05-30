using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectMVC.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ProjectMVC.Controllers
{
    public class EmailController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailController(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Confirmation(string token, string email)
        {
            var baseUrl = @"https://localhost:44340/api/Authentication/EmailVerification?token={0}&email={1}";
            var url = string.Format(baseUrl, HttpUtility.UrlEncode(token), email);
            var result = await _httpClientFactory.CreateClient().GetAsync(url);
            result.EnsureSuccessStatusCode();

            var responseString = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<EmailConfirmationViewModel>(responseString);

            return View(data);
        }
    }
}
