// Controlador de categorías
// Maneja operaciones CRUD sobre las categorías de herramientas IA
using AIHubBackend.Data;
using AIHubBackend.DTOs;
using AIHubBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIHubBackend.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/categories
    // Público — cualquiera puede ver las categorías
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    // GET /api/categories/{id}
    // Público
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound("Categoría no encontrada.");

        return Ok(category);
    }

    // GET /api/categories/{id}/tools
    // Público — lista las herramientas de una categoría
    [HttpGet("{id}/tools")]
    public async Task<IActionResult> GetToolsByCategory(int id)
    {
        var exists = await _context.Categories.AnyAsync(c => c.Id == id);
        if (!exists)
            return NotFound("Categoría no encontrada.");

        var tools = await _context.Tools
            .Where(t => t.CategoryId == id)
            .ToListAsync();

        return Ok(tools);
    }

    // POST /api/categories
    // Solo admin
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { id = category.Id }, category);
    }

    // PUT /api/categories/{id}
    // Solo admin
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, CategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound("Categoría no encontrada.");

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(category);
    }

    // DELETE /api/categories/{id}
    // Solo admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound("Categoría no encontrada.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return Ok("Categoría eliminada correctamente.");
    }
}