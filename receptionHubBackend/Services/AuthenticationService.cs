using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Controllers.Dtos;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace receptionHubBackend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ReceptionHubDbContext _baza;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IPasswordHasher<Recepcioner> _passwordHasher;

        public AuthenticationService(
            ReceptionHubDbContext baza,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IPasswordHasher<Recepcioner> passwordHasher)
        {
            _baza = baza;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenDto?> AutentifikujAsync(LoginDto loginDto)
        {
            // Pronađi recepcionera po korisničkom imenu
            var recepcioner = await _baza.Recepcioneri
                .FirstOrDefaultAsync(r => r.KorisnickoIme == loginDto.KorisnickoIme && r.Aktivan);

            if (recepcioner == null)
            {
                return null;
            }

            // Provjeri lozinku IPasswordHasher
            var result = _passwordHasher.VerifyHashedPassword(recepcioner, recepcioner.LozinkaHash, loginDto.Lozinka);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // Ažuriraj posljednji login
            recepcioner.PosljednjiLogin = DateTime.UtcNow;
            await _baza.SaveChangesAsync();

            // Generiši JWT token
            var token = GenerisiToken(recepcioner);

            return new TokenDto
            {
                Token = token,
                Istice = DateTime.UtcNow.AddHours(8),
                Tip = "Bearer",
                Ime = recepcioner.Ime,
                Prezime = recepcioner.Prezime,
                Pozicija = recepcioner.Pozicija
            };
        }

        public Task<bool> ValidirajTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "ReceptionHubBackendApiFullStackWithAngular");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private string GenerisiToken(Recepcioner recepcioner)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "ReceptionHubBackendApiFullStackWithAngular");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, recepcioner.IdRecepcionera.ToString()),
                    new Claim(ClaimTypes.Name, recepcioner.KorisnickoIme),
                    new Claim(ClaimTypes.GivenName, recepcioner.Ime),
                    new Claim(ClaimTypes.Surname, recepcioner.Prezime),
                    new Claim(ClaimTypes.Role, recepcioner.Pozicija.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}