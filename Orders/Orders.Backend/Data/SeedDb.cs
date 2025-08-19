using Orders.Shared.Entities;

namespace Orders.Backend.Data
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
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Herramientas" });
                _context.Categories.Add(new Category { Name = "Zapatillas" });
                _context.Categories.Add(new Category { Name = "Automóviles" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _ = _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = [
                        new State(){
                            Name = "Antioquía",
                            Cities = [
                                new () { Name = "Medellin"},
                                new () { Name = "Itagui"},
                                new () { Name = "Envigado"},
                                new () { Name = "Bello"},
                                new () { Name = "Rionegro"},
                                ]
                        },
                            new State(){
                            Name = "Bogotá",
                            Cities = [
                                new () { Name = "Usaquen"},
                                new () { Name = "Champinero"},
                                new () { Name = "Santa Fe"},
                                new () { Name = "Useme"},
                                new () { Name = "Bosa"},
                                ]
                        },
                        ]
                });
                _ = _context.Countries.Add(new Country
                {
                    Name = "Argentina",
                    States = [
                        new State(){
                            Name = "Cdad Buenos Aires",
                            Cities = [
                                new () { Name = "Almagro"},
                                new () { Name = "Boedo"},
                                new () { Name = "Caballito"},
                                new () { Name = "Coghland"},
                                new () { Name = "Flores"},
                                ]
                        },
                            new State(){
                            Name = "Buenos Aires",
                            Cities = [
                                new () { Name = "La Plata"},
                                new () { Name = "Berazategui"},
                                new () { Name = "Campana"},
                                new () { Name = "Diaureaux"},
                                new () { Name = "Ensenada"},
                                ]
                        },
                        ]
                });



                await _context.SaveChangesAsync();
            }
        }
    }
}