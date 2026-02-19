public interface IArhivaService
{
    Task<int> ArhivirajZavrseneRezervacijeAsync(); 
    Task<bool> ArhivirajRezervacijuAsync(int rezervacijaId, int arhiviraoRecepcionerId);
    Task<bool> ArhivirajGostaAsync(int gostId, int arhiviraoRecepcionerId);
}