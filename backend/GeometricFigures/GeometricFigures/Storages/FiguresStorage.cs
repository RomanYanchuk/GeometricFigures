using GeometricFigures.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeometricFigures.Storages
{
    public class FiguresStorage : DbContext
    {
        public DbSet<Figure> Figures { get; set; }

        public FiguresStorage(DbContextOptions<FiguresStorage> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Figure>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}