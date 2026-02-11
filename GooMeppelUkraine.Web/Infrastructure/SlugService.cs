using GooMeppelUkraine.Web.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Infrastructure
{
    public class SlugService
    {
        private readonly ApplicationDbContext _db;

        public SlugService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string> GenerateUniqueSlugAsync(string title, string language, int? excludeArticleId = null)
        {
            var baseSlug = SlugHelper.Generate(title);
            var slug = baseSlug;
            var i = 2;

            while (await _db.Articles.AnyAsync(a =>
                       a.Language == language &&
                       a.Slug == slug &&
                       (excludeArticleId == null || a.Id != excludeArticleId.Value)))
            {
                slug = $"{baseSlug}-{i}";
                i++;
            }

            return slug;
        }
    }
}
