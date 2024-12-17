using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string AuthorizeCode = "CWA-6F379A4F-0BA7-4E01-A1A2-B9E21E63EC3E";

        private readonly ILogger<WeatherForecastController> _logger;

        public TestController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherOnline")]
        public Object? Get()
        {
            HttpClient Client = new HttpClient();
            string formURLFormat = "https://opendata.cwa.gov.tw/api/v1/rest/datastore/F-C0032-001";

            var values = new Dictionary<string, string>
            {
                {"Authorization", $"{AuthorizeCode}"},
                {"limit", $"100"},
            };

            var requestUri = QueryHelpers.AddQueryString(formURLFormat, values);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var result = Client.SendAsync(request).Result;
            var json = result.Content.ReadAsStreamAsync().Result;
            var s = JsonSerializer.Deserialize<Object>(json);

            return s;
        }
    }
}
