using CourseProjectitr.Models;

namespace CourseProjectitr.Data
{
    public static class DbInitializer
    {
        public static void SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Equipment" },
                    new Category { Name = "Furniture" },
                    new Category { Name = "Book" },
                    new Category { Name = "Other" }
                );
                context.SaveChanges();
            }
        }
    }
}
