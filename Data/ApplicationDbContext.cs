using Microsoft.EntityFrameworkCore;
using CourseProjectitr.Models; // подключаем модели

namespace CourseProjectitr.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        


        // Таблицы в базе данных
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Category> Categories { get; set; }

        // Дополнительно: можно настроить связи, ограничения и т.д.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Пример настройки связи один-ко-многим
            modelBuilder.Entity<Inventory>()
            .HasMany(i => i.Tags)
            .WithMany(t => t.Inventories);

        }
    }
}
