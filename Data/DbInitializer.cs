using CourseProjectitr.Models;

namespace CourseProjectitr.Data
{
    public static class DbInitializer
    {
        public static void SeedCategories(ApplicationDbContext context)
        {
            var existing = context.Categories.ToList();
            context.Categories.RemoveRange(existing);
            context.SaveChanges();

            context.Categories.AddRange(
                new Category { Name = "Equipment" },
                new Category { Name = "Furniture" },
                new Category { Name = "Safety" },
                new Category { Name = "Supplies" }
            );
            context.SaveChanges();
        }

    }
}
