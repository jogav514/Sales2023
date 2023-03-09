using Sales.Shared.Entities;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await ChackCountriesAsync();
        }

        private async Task ChackCountriesAsync()
        {
        if(!_context.countries.Any())
            {
                _context.countries.Add(new Country { Name = "Colombia" });
                _context.countries.Add(new Country { Name = "Perú" });
                _context.countries.Add(new Country { Name = "Chile" });

                await _context.SaveChangesAsync();

            }

        }
    }
}