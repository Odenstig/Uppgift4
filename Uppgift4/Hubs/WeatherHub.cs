using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Text.Json;
using Uppgift4.Server.Models;
using Microsoft.AspNetCore.DataProtection;

namespace Uppgift4.Server.Hubs
{
    public class WeatherHub : Hub
    {
        private readonly ILogger<WeatherHub> _logger;
        private readonly IDataProtectionProvider _dpProvider;
        private readonly string _group = "WeatherChannel";

        public WeatherHub(ILogger<WeatherHub> logger, IDataProtectionProvider dpProvider)
        {
            _logger = logger;
            _dpProvider = dpProvider;
        }

        //Subscribe to the Weather Channel :-)
        public async Task JoinWeatherChannel()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _group);
        }

        public async Task CurrentWeather(string encryptedWeather)
        
        {
            try
            {
                await Task.Delay(5000);

                var test = _dpProvider.CreateProtector("Weather.Purpose");

                var decrypted = test.Unprotect(encryptedWeather);

                var deserializedWeather = JsonSerializer.Deserialize<WeatherData>(decrypted);

                _logger.LogInformation($"New Weather data acquired! : \r\n{deserializedWeather.City} \r\n {deserializedWeather.Temp} \r\n {deserializedWeather.Date} \n Sending To subscribers..");

                var serializedWeather = JsonSerializer.Serialize(deserializedWeather);

                await Clients.Group(_group).SendAsync("Send", serializedWeather);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
        }
    }
}
