
namespace receptionHubBackend.Services;

public class AutomatskoArhiviranjeService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AutomatskoArhiviranjeService> _logger;

    public AutomatskoArhiviranjeService(
        IServiceScopeFactory scopeFactory,
        ILogger<AutomatskoArhiviranjeService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutomatskoArhiviranjeService pokrenut");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Provjeri da li je 3 sata ujutru (kada hotel ima najmanje prometa)
                var now = DateTime.Now;
                var sljedecePokretanje = DateTime.Today.AddDays(1).AddHours(3); // Sutra u 3:00

                var vrijemeDoPokretanja = sljedecePokretanje - now;
                if (vrijemeDoPokretanja < TimeSpan.Zero)
                {
                    vrijemeDoPokretanja = TimeSpan.FromHours(24); // Ako je prošlo 3:00, čekaj 24 sata
                }

                _logger.LogInformation($"Sljedeće automatsko arhiviranje za {vrijemeDoPokretanja.TotalHours:F1} sati");
                
                await Task.Delay(vrijemeDoPokretanja, stoppingToken);

                // Pokreni arhiviranje
                using (var scope = _scopeFactory.CreateScope())
                {
                    var arhivaService = scope.ServiceProvider.GetRequiredService<IArhivaService>();
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                    _logger.LogInformation("Pokretanje automatskog arhiviranja završenih rezervacija...");
                    
                    // POZIV METODE - ovo je falilo!
                    var arhivirano = await arhivaService.ArhivirajZavrseneRezervacijeAsync();
                    
                    _logger.LogInformation($"Automatsko arhiviranje završeno. Arhivirano {arhivirano} rezervacija.");
                    
                    await logService.LogInfoAsync(
                        $"Automatsko noćno arhiviranje: arhivirano {arhivirano} rezervacija",
                        "AutomatskoArhiviranjeService");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška u AutomatskoArhiviranjeService");
            }
        }
    }
}