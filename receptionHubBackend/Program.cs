using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;
using receptionHubBackend.Services;
using System.Text;
using System.Threading.RateLimiting;

namespace receptionHubBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // BAZA
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ReceptionHubDbContext>(options =>
            options.UseNpgsql(connectionString));

        // JWT
        var jwtKey = builder.Configuration["Jwt:Key"] ?? 
            throw new InvalidOperationException("JWT Key nije konfigurisan! Koristi user-secrets.");
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AngularFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:4200",           // Angular development
                        "https://localhost:4200",          // HTTPS verzija
                        "https://receptionhub-frontend.vercel.app", // Produkcija
                        "https://receptionhub-frontend.netlify.app"  // Produkcija
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // JWT
            });
        });

        // SERVISI
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ILogService, LogService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IArhivaService, ArhivaService>();
        builder.Services.AddScoped<IPdfService, PdfService>();

        // PASSWORD HASHER
        builder.Services.AddScoped<IPasswordHasher<Recepcioner>, PasswordHasher<Recepcioner>>();

        // AUTOMATSKO ARHIVIRANJE
        builder.Services.AddHostedService<AutomatskoArhiviranjeService>();

        // KESIRANJE
        builder.Services.AddMemoryCache();

        // RATE LIMITIN
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,        // Maks 100 zahtjeva
                        Window = TimeSpan.FromMinutes(1)  // U 1 minuti
                    }));
        });

        // KONTROLERI
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        var app = builder.Build();


        app.UseHttpsRedirection();

        // CORS - mora biti prije Auth
        app.UseCors("AngularFrontend");

        // Rate limiting
        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // ========== 13. INICIJALIZACIJA BAZE (seed podaci) ==========
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ReceptionHubDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            try
            {
                // Primijeni migracije
                context.Database.Migrate();
                
                // Dodaj seed podatke ako je baza prazna
                if (!context.Recepcioneri.Any())
                {
                    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Recepcioner>>();
                    
                    context.Recepcioneri.Add(new Recepcioner
                    {
                        Ime = "Admin",
                        Prezime = "Admin",
                        KorisnickoIme = "admin",
                        Email = "admin@hotel.com",
                        LozinkaHash = passwordHasher.HashPassword(new Recepcioner(), "admin123"),
                        Pozicija = TipPozicije.Admin,
                        Aktivan = true,
                        DatumKreiranjaRacuna = DateTime.UtcNow
                    });
                    
                    context.SaveChanges();
                    logger.LogInformation("Dodan admin korisnik");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Greška pri inicijalizaciji baze");
            }
        }

        app.Run();
    }
}