using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using Uppgift4.Sim;

var serviceCollection = new ServiceCollection();
serviceCollection.AddDataProtection()
    .SetApplicationName("Uppgift4");
var services = serviceCollection.BuildServiceProvider();


var protectorProvider = services.GetRequiredService<IDataProtectionProvider>();

var protector = protectorProvider.CreateProtector("Weather.Purpose");

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7123/WeatherHub")
    .WithAutomaticReconnect()
    .Build();

Console.WriteLine("IoT Sim Waiting.. (Press Enter)");
Console.ReadLine();
Console.Clear();

await connection.StartAsync();

Console.WriteLine("Simulated IoT Connected!");

while (true)
{
    var weather = new WeatherData()
    {
        City = RandomCity(),
        Temp = RandomTemp(),
        Date = DateTime.Now.ToString(),
    };

    var serialized = JsonSerializer.Serialize(weather);

    var encrypted = protector.Protect(serialized);

    await connection.SendAsync("CurrentWeather", encrypted, CancellationToken.None);
}



string RandomCity()
{
    int pick = new Random().Next(0, 3);

    if (pick == 0)
        return "Stockholm";
    else if (pick == 1)
        return "Uppsala";
    else
        return "Nynäshamn";
}

string RandomTemp()
{
    int pick = new Random().Next(-30, 31);

    return pick + " °C";
}

