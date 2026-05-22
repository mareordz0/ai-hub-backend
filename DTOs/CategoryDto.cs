// Datos de entrada para crear o actualizar una categoría
using System.ComponentModel.DataAnnotations;

namespace AIHubBackend.DTOs;

public class CategoryDto
{
    [Required][StringLength(50)] public string Name { get; set; } = string.Empty;
    [StringLength(200)] public string Description { get; set; } = string.Empty;
}