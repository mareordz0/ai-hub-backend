// Herramientas IA, cada una ligada a una categoría
namespace AIHubBackend.Models;

public class Tool
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    // Relación con categoría
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}