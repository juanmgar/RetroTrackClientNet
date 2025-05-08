using RetroTrack.Services;

var builder = WebApplication.CreateBuilder(args);

// ======== CONFIGURACIÓN DE SERVICIOS ========

// Añade soporte para controladores y vistas (MVC)
builder.Services.AddControllersWithViews();

// Permite acceder a HttpContext (útil para leer info de la petición)
builder.Services.AddHttpContextAccessor();

// Registra un cliente HTTP para consumir APIs REST
builder.Services.AddHttpClient<ApiRestClientService>();

// Registra un cliente SOAP como singleton (una única instancia para toda la app)
builder.Services.AddSingleton<ApiSoapClientService>();

// Configura sesión para poder guardar datos como el token JWT entre peticiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // tiempo de inactividad antes de expirar
    options.Cookie.HttpOnly = true;                  // protege la cookie frente a acceso por JS
    options.Cookie.IsEssential = true;              // asegura que siempre se guarde la cookie
});

var app = builder.Build();

// ======== IGNORAR VALIDACIÓN DE CERTIFICADOS SOLO EN DESARROLLO ========
System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

// ======== CONFIGURACIÓN DEL PIPELINE ========

// Muestra página de error amigable en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Permite servir archivos estáticos (css, js, imágenes, etc.)
app.UseStaticFiles();

// Habilita enrutamiento para controladores
app.UseRouting();

// Activa el middleware de sesión para almacenar info entre peticiones
app.UseSession();

// Activa la autorización (aunque no tengas [Authorize], siempre debe estar configurado)
app.UseAuthorization();

// Define la ruta por defecto del proyecto MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Arranca la aplicación web
app.Run();
