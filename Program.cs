using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PortfolioTracking;
using PortfolioTracking.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuration du service HttpClient pour effectuer des appels HTTP
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Enregistrement de votre service PortfolioService pour l'injection de dépendances
builder.Services.AddScoped<PortfolioService>();

await builder.Build().RunAsync();  // Construction et exécution de l'application

