using System.Text.Json;
using System.Text.Json.Serialization;
using dotnet_api.Hubs;
using dotnet_api.Repositories;
using dotnet_api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddSingleton<MessageHub>();
Console.WriteLine("sqlite connection string: " + configuration["ConnectionString"]);
builder.Services.AddTransient<IDataRepository>(s => new DataRepository(configuration["ConnectionString"]!));
builder.Services.AddTransient<IDataService, DataService>();

HubConnectionHandler hubConnectionHandler = new HubConnectionHandler(configuration?["SignalR:Endpoint"] + configuration?["SignalR:Route"]);
hubConnectionHandler.StartAsync();

builder.Services.AddSingleton<IHubConnectionHandler>(hubConnectionHandler);

var app = builder.Build();
app.UseCors(b =>
        b.WithOrigins(["http://localhost:4200", "http://localhost:5173", "http://localhost:3000"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()  // Add AllowCredentials for SignalR to work with cookies or authorization headers
);
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>(configuration?["SignalR:Route"]);
});
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();
