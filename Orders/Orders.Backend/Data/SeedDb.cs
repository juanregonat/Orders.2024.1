using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;


        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Regonat", "juanregonat@yopmail.com", "322 311 2020", "calle XXX", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
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