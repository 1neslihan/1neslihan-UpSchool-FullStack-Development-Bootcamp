using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;
using UpSchool.Wasm;
using Blazored.LocalStorage;
using Blazored.Toast;
using UpSchool.Domain.Services;
using UpSchool.Wasm.Services;
using UpSchool.Domain.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var titanicFluteApiUrl = builder.Configuration.GetConnectionString("TitanicFlute");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredToast();

builder.Services.AddScoped<IToasterService, BlazoredToastService>();

//builder.Services.AddSingleton(typeof(LoggerBase));

builder.Services.AddSingleton<IUrlHelperService>(new UrlHelperService(titanicFluteApiUrl));
//builder.Services.AddSingleton<IToasterService, ToasterService>();
//builder.Services.AddTransient<IToasterService, ToasterService>();

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});



await builder.Build().RunAsync();
