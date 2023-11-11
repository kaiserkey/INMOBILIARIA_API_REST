using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Inmobiliaria.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001")//permite escuchar SOLO peticiones locales
builder.WebHost.UseUrls("http://localhost:5200", "http://*:5200", "http://192.168.1.139:5200");//permite escuchar peticiones locales y remotas

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(
    options =>
    {
        options.LoginPath = "/Usuario/login";
        options.LogoutPath = "/Usuario/logout";
        options.AccessDeniedPath = "/Home/Restringido";
    }
)
.AddJwtBearer(options =>//la api web valida con token
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["TokenAuthentication:Issuer"],
        ValidAudience = configuration["TokenAuthentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
            configuration["TokenAuthentication:SecretKey"])),
    };
    // opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Leer el token desde el query string
            var accessToken = context.Request.Query["access_token"];
            // Si el request es para el Hub u otra ruta seleccionada...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (
                 path.StartsWithSegments("/chatsegurohub") ||
                 path.StartsWithSegments("/api/propietarios/reset") ||
                 path.StartsWithSegments("/api/propietarios/token")))
            {//reemplazar las urls por las necesarias ruta ⬆
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(
    options => options.UseMySql(
        configuration["ConnectionStrings:MySql"],
        ServerVersion.AutoDetect(configuration["ConnectionStrings:MySql"]))
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Empleado",policy => policy.RequireRole("Empleado","Administrador"));
    options.AddPolicy("Administrador",policy => policy.RequireRole("Administrador"));
});

builder.Services.AddMvc();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("login", "entrar/{**accion}", new { controller = "Usuario", action = "Login" });

app.Run();
