using Fantasy.Backend.Data;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.shared.DTOs;
using Fantasy.shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Repositories.Implementations;

// Implementación concreta de la interfaz IUsersRepository.
public class UsersRepository : IUsersRepository
{
    private readonly DataContext _context; // Contexto de la base de datos.
    private readonly UserManager<User> _userManager; // Administra las operaciones relacionadas con usuarios.
    private readonly RoleManager<IdentityRole> _roleManager; // Administra los roles del sistema.
    private readonly SignInManager<User> _signInManage; // Administra las operaciones de inicio de sesión.

    // Constructor que recibe las dependencias necesarias.
    public UsersRepository(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManage)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManage = signInManage;
    }

    // Añade un nuevo usuario a la base de datos con una contraseña.
    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    // Añade un usuario a un rol específico.
    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }

    // Verifica si un rol existe, y si no, lo crea.
    public async Task CheckRoleAsync(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName
            });
        }
    }

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    // Obtiene un usuario a partir de su correo electrónico.
    public async Task<User> GetUserAsync(string email)
    {
        var user = await _context.Users
            .Include(u => u.Country) // Incluye el país relacionado.
            .FirstOrDefaultAsync(x => x.Email == email); // Busca el usuario por email.
        return user!;
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Country)
            .FirstOrDefaultAsync(x => x.Id == userId.ToString());
        return user!;
    }

    // Verifica si un usuario está en un rol específico.
    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    // Inicia sesión con las credenciales del usuario.
    public async Task<SignInResult> LoginAsync(LoginDTO model)
    {
        return await _signInManage.PasswordSignInAsync(model.Email, model.Password, false, false);
    }

    // Cierra la sesión del usuario actual.
    public async Task LogoutAsync()
    {
        await _signInManage.SignOutAsync();
    }
}