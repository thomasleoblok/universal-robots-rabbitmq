using Microsoft.AspNetCore.Mvc;

namespace Api_universal_robots_rmq.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public static readonly List<string> messages = new List<string>
        {
        "hej"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<string> Get()
        {
            return messages;
        }
    }
}