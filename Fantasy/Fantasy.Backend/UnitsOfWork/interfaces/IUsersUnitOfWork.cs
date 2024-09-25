using Fantasy.shared.DTOs;
using Fantasy.shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fantasy.Backend.UnitsOfWork.interfaces;

public interface IUsersUnitOfWork
{
    // Inicia sesión con las credenciales del usuario.
    Task<SignInResult> LoginAsync(LoginDTO model);

    // Cierra la sesión del usuario actual.
    Task LogoutAsync();

    // Obtiene un usuario a partir de su correo electrónico.
    Task<User> GetUserAsync(string email);

    // Añade un nuevo usuario a la base de datos con una contraseña.
    Task<IdentityResult> AddUserAsync(User user, string password);

    // Verifica si un rol existe y lo crea si no.
    Task CheckRoleAsync(string roleName);

    // Añade un usuario a un rol específico.
    Task AddUserToRoleAsync(User user, string roleName);

    // Verifica si un usuario está en un rol específico.
    Task<bool> IsUserInRoleAsync(User user, string roleName);
}