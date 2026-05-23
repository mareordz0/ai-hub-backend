using Microsoft.AspNetCore.Identity;

// Almacena usuarios (admin/user)
namespace AIHubBackend.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    // Constantes de roles disponibles ára usar en todo el proyecto
    public static class Roles
    {
        public const string Admin = "admin";
        public const string User = "user";
    }
}