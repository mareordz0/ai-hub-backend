// Controlador de herramientas IA
// Maneja operaciones CRUD con soporte de filtrado y paginación
using AIHubBackend.Data;
using AIHubBackend.DTOs;
using AIHubBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIHubBackend.Controllers;

[ApiController]
[Route("api/tools")]
public class ToolsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ToolsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/tools
    // Público — soporta filtrado por categoría y paginación
    // Ejemplo: /api/tools?category=1&page=1&limit=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? category,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var query = _context.Tools.Include(t => t.Category).AsQueryable();

        if (category.HasValue)
            query = query.Where(t => t.CategoryId == category.Value);

        var total = await query.CountAsync();
        var tools = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Ok(new { total, page, limit, tools });
    }

    // GET /api/tools/{id}
    // Público
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tool = await _context.Tools
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tool == null)
            return NotFound("Herramienta no encontrada.");

        return Ok(tool);
    }

    // POST /api/tools
    // Solo admin
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(ToolDto dto)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            return BadRequest("La categoría especificada no existe.");

        var tool = new Tool
        {
            Name = dto.Name,
            Description = dto.Description,
            Url = dto.Url,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        _context.Tools.Add(tool);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { id = tool.Id }, tool);
    }

    // PUT /api/tools/{id}
    // Solo admin
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, ToolDto dto)
    {
        var tool = await _context.Tools.FindAsync(id);
        if (tool == null)
            return NotFound("Herramienta no encontrada.");

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            return BadRequest("La categoría especificada no existe.");

        tool.Name = dto.Name;
        tool.Description = dto.Description;
        tool.Url = dto.Url;
        tool.ImageUrl = dto.ImageUrl;
        tool.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return Ok(tool);
    }

    // DELETE /api/tools/{id}
    // Solo admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var tool = await _context.Tools.FindAsync(id);
        if (tool == null)
            return NotFound("Herramienta no encontrada.");

        _context.Tools.Remove(tool);
        await _context.SaveChangesAsync();
        return Ok("Herramienta eliminada correctamente.");
    }
}