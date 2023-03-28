using Sales.Shared.Entities;
using Sales.API.Services;
using Microsoft.EntityFrameworkCore;
using Sales.Shared.Responses;
using Sales.API.Helpers;
using Sales.Shared.Enums;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await ChackCountriesAsync();
            await CheckCategoryAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Johan", "Gaviria", "jego@yopmail.com", "123 456 7890", "Calle Luna Calle Sol", UserType.Admin);

        }
        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
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

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }


        private async Task ChackCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

        }

        private async Task CheckCategoryAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category
                {
                    Name = "Accesorios para Vehículos",
                    SubCategories = new List<SubCategory>() 
                    {
                    new SubCategory(){Name ="Acc. para Carros y Camionetas"},
                    new SubCategory(){Name ="Acc. para Motos y Cuatrimotos"},
                    new SubCategory(){Name ="Accesorios para Náuticos"},
                    new SubCategory(){Name ="Accesorios para Camiones"},
                    new SubCategory(){Name ="Auidio para Vehículos"},
                    new SubCategory(){Name ="GNV"},
                    new SubCategory(){Name ="Herramientas para Vehiculos"},
                    new SubCategory(){Name ="Limpieza de vehiculos"},
                    new SubCategory(){Name ="Llantas y Accesorios"},
                    new SubCategory(){Name ="Lubricantes y Fluidos"},
                    new SubCategory(){Name ="Motos"},
                    new SubCategory(){Name ="Navegadores Gps Para vehiculos"},
                    new SubCategory(){Name ="Performance"},
                    new SubCategory(){Name ="Repuestos Carros y Camionetas"},
                    new SubCategory(){Name ="Repuestos Motos y Camionetas"},
                    },
                });
                _context.Categories.Add(new Category
                {
                    Name = "Agro",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Animales"},
                        new SubCategory(){Name ="Generadores de energía"},
                        new SubCategory(){Name ="Infraestructura Rural"},
                        new SubCategory(){Name ="Insumos Agrícolas"},
                        new SubCategory(){Name ="Insumos ganaderos"},
                        new SubCategory(){Name ="Maquinas y Herramientas"},
                        new SubCategory(){Name ="Repuestos Maquinaria Agrícola"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Alimentos y Bebidas",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Almacén"},
                        new SubCategory(){Name ="Bebidas"},
                        new SubCategory(){Name ="Comida Preparada"},
                        new SubCategory(){Name ="Congelados"},
                        new SubCategory(){Name ="Frescos"},
                        new SubCategory(){Name ="Kéfir"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Animales y Mascotas",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Aves"},
                        new SubCategory(){Name ="Caballos"},
                        new SubCategory(){Name ="Conejos"},
                        new SubCategory(){Name ="Gatos"},
                        new SubCategory(){Name ="Insectos"},
                        new SubCategory(){Name ="Peces"},
                        new SubCategory(){Name ="Perros"},
                        new SubCategory(){Name ="Reptiles y Anfíbios"},
                        new SubCategory(){Name ="Roedores"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Antigüedades y Colecciones",
                    SubCategories = new List<SubCategory>()
                    {
                       new SubCategory(){Name ="Antigüedades"},
                       new SubCategory(){Name ="Banderas"},
                       new SubCategory(){Name ="Billetes y Monedas"},
                       new SubCategory(){Name ="Coleccionables de Deportes"},
                       new SubCategory(){Name ="Esculturas"},
                       new SubCategory(){Name ="Filatelia"},
                       new SubCategory(){Name ="Militaría y Afines"},
                       new SubCategory(){Name ="Posters"},
                       new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Arte, Papelería y Mercería",
                    SubCategories = new List<SubCategory>()
                    {
                      new SubCategory(){Name ="Arte y Manualidades"},
                      new SubCategory(){Name ="Espejos Mosaicos"},
                      new SubCategory(){Name ="Mercería"},
                      new SubCategory(){Name ="Papelería"},
                      new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Bebés",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Artículos de Bebés para Baños"},
                        new SubCategory(){Name ="Artículos de Maternidad"},
                        new SubCategory(){Name ="Caminadores y Correpasillos"},
                        new SubCategory(){Name ="Chupetes y Mordedores"},
                        new SubCategory(){Name ="Comida para Bebés"},
                        new SubCategory(){Name ="Corrales para Bebé"},
                        new SubCategory(){Name ="Cuarto del Bebé"},
                        new SubCategory(){Name ="Higiene y cuidado del Bebé"},
                        new SubCategory(){Name ="Juegos y Jugetes para Bebés"},
                        new SubCategory(){Name ="Lactancia y Alimentación"},
                        new SubCategory(){Name ="Paseo del Bebé"},
                        new SubCategory(){Name ="Ropa para Bebés"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Belleza y Cuidado Personal",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Artefactos para el cabello"},
                        new SubCategory(){Name ="Artículos de Peluqueria"},
                        new SubCategory(){Name ="Barberia"},
                        new SubCategory(){Name ="Cuidado para la Piel"},
                        new SubCategory(){Name ="Cuidado del cabello"},
                        new SubCategory(){Name ="Depilación"},
                        new SubCategory(){Name ="Farmacia"},
                        new SubCategory(){Name ="Higiene Personal"},
                        new SubCategory(){Name ="Manicure y Pedicure"},
                        new SubCategory(){Name ="Maquillaje"},
                        new SubCategory(){Name ="Perfumes y Frgancias"},
                        new SubCategory(){Name ="Tratamientos de Belleza"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Boletas para Espectaculos",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Conciertos"},
                        new SubCategory(){Name ="Eventos a Beneficio"},
                        new SubCategory(){Name ="Eventos Deportivos"},
                        new SubCategory(){Name ="Eventos Finalizados"},
                        new SubCategory(){Name ="Exposiciones"},
                        new SubCategory(){Name ="Teatro"},
                        new SubCategory(){Name ="Otras Boletas"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Cámaras y Accesorios",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Accesorios para Cámaras"},
                        new SubCategory(){Name ="Álbunes y Portarretratos"},
                        new SubCategory(){Name ="Cables"},
                        new SubCategory(){Name ="Cámaras"},
                        new SubCategory(){Name ="Cámaras del Video y Deportivas"},
                        new SubCategory(){Name ="Drones Y Accesorios"},
                        new SubCategory(){Name ="Instrumentos ópticos"},
                        new SubCategory(){Name ="Lentes y Filtros"},
                        new SubCategory(){Name ="Repuestos para Cámaras"},
                        new SubCategory(){Name ="Revelado y Laboratorio"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Carros, Motos y Otros",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Camiones"},
                        new SubCategory(){Name ="Carros de Colección"},
                        new SubCategory(){Name ="Carros y Camionetas"},
                        new SubCategory(){Name ="Maquinaria Pesada"},
                        new SubCategory(){Name ="Motos"},
                        new SubCategory(){Name ="Náutica"},
                        new SubCategory(){Name ="Otros Vehiculos"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Celulares y télefonos",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Accesorios para Celulares "},
                        new SubCategory(){Name ="Celulares y Smartphones"},
                        new SubCategory(){Name ="Gafas de Realidad Virtual"},
                        new SubCategory(){Name ="Radios y Handies"},
                        new SubCategory(){Name ="Repuestos de Celulares"},
                        new SubCategory(){Name ="Smartwatches y Accesorios"},
                        new SubCategory(){Name ="Tarificadores y Cabinas"},
                        new SubCategory(){Name ="Telefonia Fija e Inalámbrica"},
                        new SubCategory(){Name ="Telefonía IP"},
                        new SubCategory(){Name ="Otros"},
                    }
                });
                _context.Categories.Add(new Category
                {
                    Name = "Computación",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory(){Name ="Accesorios de Antiestática"},
                        new SubCategory(){Name ="Accesorios para PC Gaming"},
                        new SubCategory(){Name ="Almacenamiento"},
                        new SubCategory(){Name ="Cables y Hubs USB"},
                        new SubCategory(){Name ="Componentes de PC"},
                        new SubCategory(){Name ="Conectividad y Redes"},
                        new SubCategory(){Name ="Estabilizadores y UPS"},
                        new SubCategory(){Name ="Impresión"},
                        new SubCategory(){Name ="Lectores y Scaners"},
                        new SubCategory(){Name ="Limpieza y Cuidado de PCs"},
                        new SubCategory(){Name ="Monitores y Accesorios"},
                        new SubCategory(){Name ="Palms, Agendas y Accesorios"},
                        new SubCategory(){Name ="PC de Escrotiorio"},
                        new SubCategory(){Name ="Otros"},
                    }
                    });
                _context.Categories.Add(new Category
                {
                    Name = "Consolas y Videojuegos",
                 });
                _context.Categories.Add(new Category
                {
                    Name = "Construccion",
                });
                _context.Categories.Add(new Category
                {
                    Name = "Deportes y Fitness",
                });
                _context.Categories.Add(new Category
                {
                    Name = "Electrodomésticos",
                });
                _context.Categories.Add(new Category
                {
                    Name = "Electrónica, Audio y Video",
                });
                _context.Categories.Add(new Category
                {
                    Name = "Herramientas",
                });
            }

            await _context.SaveChangesAsync();
        }

    }
}