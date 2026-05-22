// Datos de entrada para crear o actualizar una herramienta
using System.ComponentModel.DataAnnotations;

namespace AIHubBackend.DTOs;

public class ToolDto
{
    [Required][StringLength(100)] public string Name { get; set; } = string.Empty;
    [StringLength(500)] public string Description { get; set; } = string.Empty;
    [Required][Url] public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    [Required] public int CategoryId { get; set; }
}