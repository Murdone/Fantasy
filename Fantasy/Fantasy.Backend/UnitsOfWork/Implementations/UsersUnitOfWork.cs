using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.interfaces;
using Fantasy.shared.DTOs;
using Fantasy.shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fantasy.Backend.UnitsOfWork.Implementations
{
    // Implementación concreta de la interfaz IUsersUnitOfWork.
    public class UsersUnitOfWork : IUsersUnitOfWork
    {
        private readonly IUsersRepository _usersRepository; // Repositorio de usuarios.

        // Constructor que recibe el repositorio de usuarios.
        public UsersUnitOfWork(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // Añade un nuevo usuario a la base de datos.
        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _usersRepository.AddUserAsync(user, password);

        // Añade un usuario a un rol específico.
        public async Task AddUserToRoleAsync(User user, string roleName) => await _usersRepository.AddUserToRoleAsync(user, roleName);

        // Verifica si un rol existe y lo crea si no.
        public async Task CheckRoleAsync(string roleName) => await _usersRepository.CheckRoleAsync(roleName);

        // Obtiene un usuario a partir de su correo electrónico.
        public async Task<User> GetUserAsync(string email) => await _usersRepository.GetUserAsync(email);

        // Verifica si un usuario está en un rol específico.
        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _usersRepository.IsUserInRoleAsync(user, roleName);

        // Inicia sesión con las credenciales del usuario.
        public async Task<SignInResult> LoginAsync(LoginDTO model) => await _usersRepository.LoginAsync(model);

        // Cierra la sesión del usuario actual.
        public async Task LogoutAsync() => await _usersRepository.LogoutAsync();

        public async Task<User> GetUserAsync(Guid userId) => await _usersRepository.GetUserAsync(userId);

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) => await _usersRepository.GenerateEmailConfirmationTokenAsync(user);

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) => await _usersRepository.ConfirmEmailAsync(user, token);
    }
}