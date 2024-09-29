using Fantasy.Backend.Helpers; // Importa los ayudantes necesarios para el proyecto.
using Fantasy.Backend.UnitsOfWork.interfaces; // Importa las interfaces para el Unit of Work relacionado con los usuarios.
using Fantasy.shared.Entities; // Importa las entidades compartidas, como User y otras.
using Fantasy.shared.Enums; // Importa los enumeradores compartidos, como UserType.
using Microsoft.EntityFrameworkCore; // Importa el paquete de Entity Framework Core para trabajar con la base de datos.

namespace Fantasy.Backend.Data
{
    // Clase encargada de inicializar y sembrar datos en la base de datos.
    public class SeedDb
    {
        private readonly DataContext _context; // Contexto de la base de datos.
        private readonly IFileStorage _fileStorage; // Interfaz para manejar el almacenamiento de archivos.
        private readonly IUsersUnitOfWork _usersUnitOfWork; // Unit of Work para manejar las operaciones con usuarios.

        // Constructor de la clase SeedDb, inicializa los servicios necesarios.
        public SeedDb(DataContext context, IFileStorage fileStorage, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context; // Inicializa el contexto de la base de datos.
            _fileStorage = fileStorage; // Inicializa el servicio de almacenamiento de archivos.
            _usersUnitOfWork = usersUnitOfWork; // Inicializa el servicio de Unit of Work de usuarios.
        }

        // Método principal que realiza las operaciones de seed (sembrado de datos).
        public async Task SeedAsync()
        {
            // Asegura que la base de datos esté creada antes de operar sobre ella.
            await _context.Database.EnsureCreatedAsync();

            // Verifica y llena los países si no existen.
            await CheckCountriesAsync();

            // Verifica y llena los equipos si no existen.
            await CheckTeamsAsync();

            // Verifica que los roles (Admin, User) estén creados.
            await CheckRolesAsync();

            // Verifica que un usuario de tipo Admin esté creado, en caso contrario lo crea.
            await CheckUserAsync("Percibal", "Vasquez", "vasquez.percibal@gmail.com", "+569 714 39 152", UserType.Admin);
        }

        // Método que verifica la existencia de roles y los crea si es necesario.
        private async Task CheckRolesAsync()
        {
            // Verifica si el rol Admin existe, y lo crea si no es así.
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());

            // Verifica si el rol User existe, y lo crea si no es así.
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        // Método que verifica la existencia de un usuario y lo crea si no existe.
        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
        {
            // Intenta obtener el usuario a partir de su correo electrónico.
            var user = await _usersUnitOfWork.GetUserAsync(email);

            // Si el usuario no existe, se procede a crearlo.
            if (user == null)
            {
                // Busca el país 'Chile' en la base de datos.
                var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Chile");

                // Crea un nuevo objeto User con la información proporcionada.
                user = new User
                {
                    FirstName = firstName, // Asigna el nombre.
                    LastName = lastName, // Asigna el apellido.
                    Email = email, // Asigna el correo electrónico.
                    UserName = email, // Asigna el nombre de usuario (igual al correo electrónico).
                    PhoneNumber = phone, // Asigna el número de teléfono.
                    Country = country!, // Asigna el país (aquí se asume que Colombia fue encontrado).
                    UserType = userType, // Asigna el tipo de usuario (Admin o User).
                };

                // Agrega el nuevo usuario a la base de datos y le asigna una contraseña.
                await _usersUnitOfWork.AddUserAsync(user, "123456");

                // Asigna el rol al nuevo usuario (Admin o User).
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            }

            // Devuelve el usuario (ya sea el existente o el recién creado).
            return user;
        }

        // Método que verifica la existencia de países en la base de datos y los crea si no existen.
        private async Task CheckCountriesAsync()
        {
            // Si no hay países en la tabla, se ejecuta el seed.
            if (!_context.Countries.Any())
            {
                // Lee un archivo SQL que contiene los datos de los países.
                var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");

                // Ejecuta el script SQL para llenar los países.
                await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            }
        }

        // Método que verifica la existencia de equipos y los crea si no existen.
        private async Task CheckTeamsAsync()
        {
            // Si no hay equipos en la base de datos, se ejecuta el seed.
            if (!_context.Teams.Any())
            {
                // Recorre todos los países existentes en la base de datos.
                foreach (var country in _context.Countries)
                {
                    var imagePath = string.Empty; // Variable para almacenar la ruta de la imagen del equipo.

                    // Genera la ruta del archivo de la bandera del país.
                    var filePath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{country.Name}.png";

                    // Si el archivo de la bandera existe, lo sube al almacenamiento.
                    if (File.Exists(filePath))
                    {
                        var fileBytes = File.ReadAllBytes(filePath); // Lee el archivo de imagen.
                        imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "teams"); // Guarda la imagen y obtiene la ruta en el almacenamiento.
                    }

                    // Crea un nuevo equipo para el país con la imagen subida.
                    _context.Teams.Add(new Team { Name = country.Name, Country = country!, Image = imagePath });
                }

                // Guarda los cambios en la base de datos.
                await _context.SaveChangesAsync();
            }
        }
    }
}