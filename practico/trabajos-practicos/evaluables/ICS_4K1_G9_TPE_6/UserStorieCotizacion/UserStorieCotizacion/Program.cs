using Microsoft.EntityFrameworkCore;
using System.Text;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserStorieCotizacion.Models.Common;
using Microsoft.IdentityModel.Tokens;
using UserStorieCotizacion.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configura la cadena de conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("Conexion");

// Agrega el DbContext al contenedor de servicios
builder.Services.AddDbContext<ISWContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
    builder => builder.WithOrigins("http://localhost:4200")  // url  angular
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .SetIsOriginAllowed(origin => true) // Permite todas las solicitudes
                      .AllowCredentials()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar el servicio CotizacionService al contenedor de servicios
builder.Services.AddScoped<CotizacionService>();
// Agregar el servicio PagoService al contenedor de servicios
builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<PersonaService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddSignalR();





var appSettingsSection = builder.Configuration.GetSection("AppSettings");

builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(d =>
{
    d.RequireHttpsMetadata = false;
    d.SaveToken = true;
    d.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(llave),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});








var app = builder.Build();
app.MapHub<NotificacionesHub>("/notificacionesHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebapp");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
