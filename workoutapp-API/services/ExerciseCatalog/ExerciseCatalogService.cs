using Microsoft.EntityFrameworkCore;
using WorkoutApp.API.Data;
using WorkoutApp.API.Models;
using workoutapp_API.DTOs;

namespace workoutapp_API.services.Catalog
{
    public class ExerciseCatalogService : IExerciseCatalogService
    {
        private readonly WorkoutDbContext _context;

        public ExerciseCatalogService(WorkoutDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ExerciseCatalog>> GetExercisesAsync(string? name, string? level, string? muscle, string? equipment, int page, int pageSize)
        {
            var query = _context.ExerciseCatalog.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(e => e.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(level))
                query = query.Where(e => e.Level == level);

            if (!string.IsNullOrWhiteSpace(equipment))
                query = query.Where(e => e.Equipment == equipment);

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(muscle))
                items = items.Where(e => e.PrimaryMuscles.Any(m => m.Contains(muscle, StringComparison.OrdinalIgnoreCase))).ToList();

            return new PagedResult<ExerciseCatalog>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        public async Task<ExerciseCatalog?> GetExerciseByIdAsync(string id)
        {
            return await _context.ExerciseCatalog.FindAsync(id);
        }
    }
}
