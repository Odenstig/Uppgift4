using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using Uppgift4.Subscriber;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7123/WeatherHub")
    .WithAutomaticReconnect()
    .Build();

Console.WriteLine("Subscriber Waiting.. (Press Enter)");
Console.ReadLine();
Console.Clear();

await connection.StartAsync();
await connection.SendAsync("JoinWeatherChannel");

Console.WriteLine("Subscriber Connected! \n Receiving Data..");

connection.On<string>("Send", message =>
{
    var json = JsonSerializer.Deserialize<WeatherData>(message);

    Console.WriteLine(json.City);
    Console.WriteLine(json.Temp);
    Console.WriteLine(json.Date);
});


Console.ReadLine();