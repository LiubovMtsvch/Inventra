using Microsoft.EntityFrameworkCore;
using CourseProjectitr.Models;

namespace CourseProjectitr.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<InventoryPermission> InventoryPermissions { get; set; }
        public DbSet<InventoryField> InventoryFields { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemFieldValue> ItemFieldValues { get; set; }

        public DbSet<Category> Categories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Inventory>()
            .HasMany(i => i.Tags)
            .WithMany(t => t.Inventories)
            .UsingEntity(j => j.ToTable("InventoryTag"));




            //
            modelBuilder.Entity<Item>()
            .HasOne(i => i.Inventory)
            .WithMany(inv => inv.Items)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<InventoryPermission>()
            .HasOne(p => p.Inventory)
            .WithMany(inv => inv.Permissions) // ← вот это важно!
            .HasForeignKey(p => p.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
}