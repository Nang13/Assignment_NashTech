using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;

namespace PES.Presentation.Controllers
{

    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly ICartService _cartService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        // [HttpGet(Name = "GetRedis")]
        // public async Task<IActionResult> GetCart()
        // {
        //     return Ok(await _cartService.TestRedis());
        // }
    }
}
