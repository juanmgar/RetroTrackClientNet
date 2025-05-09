using RetroTrack.Services;

var builder = WebApplication.CreateBuilder(args);

// ======== CONFIGURACIÓN DE SERVICIOS ========

// Añade soporte para controladores y vistas (MVC)
builder.Services.AddControllersWithViews();

// Permite acceder a HttpContext (útil para leer info de la petición)
builder.Services.AddHttpContextAccessor();

// Registra un cliente HTTP para consumir APIs REST con BaseAddress y certificado inseguro
builder.Services.AddHttpClient<ApiRestClientService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var apiRestUrl = configuration["ApiRestUrl"];
    client.BaseAddress = new Uri(apiRestUrl);
})
.ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddSingleton<ApiSoapClientService>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var soapUrl = configuration["ApiSoapUrl"];
    return new ApiSoapClientService(soapUrl);
});


// Configura sesión para poder guardar datos como el token JWT entre peticiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7095, listenOptions =>
    {
        listenOptions.UseHttps("certs/devcert.pfx", "changeit");
    });
});

builder.Logging.AddConsole();

var app = builder.Build();

System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

// ======== CONFIGURACIÓN DEL PIPELINE ========

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
