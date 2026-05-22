// Categorías de IA (imagen, código, etc.)
namespace AIHubBackend.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relación: una categoría tiene muchas herramientas
    public ICollection<Tool> Tools { get; set; } = new List<Tool>();
}