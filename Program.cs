using RetroTrack.Services;

var builder = WebApplication.CreateBuilder(args);

// ======== CONFIGURACI�N DE SERVICIOS ========

// A�ade soporte para controladores y vistas (MVC)
builder.Services.AddControllersWithViews();

// Permite acceder a HttpContext (�til para leer info de la petici�n)
builder.Services.AddHttpContextAccessor();

// Registra un cliente HTTP para consumir APIs REST
builder.Services.AddHttpClient<ApiRestClientService>();

// Registra un cliente SOAP como singleton (una �nica instancia para toda la app)
builder.Services.AddSingleton<ApiSoapClientService>();

// Configura sesi�n para poder guardar datos como el token JWT entre peticiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // tiempo de inactividad antes de expirar
    options.Cookie.HttpOnly = true;                  // protege la cookie frente a acceso por JS
    options.Cookie.IsEssential = true;              // asegura que siempre se guarde la cookie
});

var app = builder.Build();

// ======== IGNORAR VALIDACI�N DE CERTIFICADOS SOLO EN DESARROLLO ========
System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

// ======== CONFIGURACI�N DEL PIPELINE ========

// Muestra p�gina de error amigable en producci�n
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Permite servir archivos est�ticos (css, js, im�genes, etc.)
app.UseStaticFiles();

// Habilita enrutamiento para controladores
app.UseRouting();

// Activa el middleware de sesi�n para almacenar info entre peticiones
app.UseSession();

// Activa la autorizaci�n (aunque no tengas [Authorize], siempre debe estar configurado)
app.UseAuthorization();

// Define la ruta por defecto del proyecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Arranca la aplicaci�n web
app.Run();
