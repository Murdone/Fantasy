using Fantasy.Backend.Data;
using Fantasy.Backend.Helpers;
using Fantasy.Backend.Repositories.Implementations;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.Implementations;
using Fantasy.Backend.UnitsOfWork.interfaces;
using Fantasy.shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //para evitar la rebundancia ciclica
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
builder.Services.AddScoped<ITeamsUnitOfWork, TeamsUnitOfWork>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    x.SignIn.RequireConfirmedEmail = true;
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    x.Lockout.MaxFailedAccessAttempts = 3;
    x.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// A�ade la autenticaci�n al servicio usando el esquema de autenticaci�n JWT (Bearer Token)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    // Configura las opciones de JWT Bearer Token
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        // Indica si se debe validar el emisor (Issuer) del token. En este caso, no se valida.
        ValidateIssuer = false,

        // Indica si se debe validar la audiencia (Audience) del token. Aqu� tambi�n no se valida.
        ValidateAudience = false,

        // Indica si se debe validar la fecha de expiraci�n del token. En este caso, se valida.
        ValidateLifetime = true,

        // Indica si se debe validar la clave secreta que firma el token. En este caso, s� se valida.
        ValidateIssuerSigningKey = true,

        // Proporciona la clave secreta que se usar� para validar la firma del token.
        // Aqu� se convierte la clave "jwtKey" almacenada en el archivo de configuraci�n en una clave de seguridad.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),

        // Configura el tiempo de tolerancia para la validaci�n del tiempo de vida del token. Se establece en cero.
        // Esto significa que no hay margen de tolerancia para la expiraci�n del token.
        ClockSkew = TimeSpan.Zero
    });
builder.Services.AddScoped<IMailHelper, MailHelper>();

var app = builder.Build();
SeedData(app);  //inyeccion a mano del seeddb

void SeedData(WebApplication app) // Definici�n del m�todo SeedData
{
    // Obtiene la f�brica de �mbitos de servicio de la aplicaci�n
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    // Crea un nuevo �mbito utilizando la f�brica
    using var scope = scopedFactory!.CreateScope();

    // Obtiene el servicio SeedDb del proveedor de servicios del �mbito creado
    var service = scope.ServiceProvider.GetService<SeedDb>();

    // Llama al m�todo as�ncrono SeedAsync() y espera a que termine
    service!.SeedAsync().Wait();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();