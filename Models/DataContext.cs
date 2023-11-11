using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrato>()
                .HasKey(c => c.IdContrato);
            
            modelBuilder.Entity<Propietario>()
                .HasKey(c => c.IdPropietario);

            modelBuilder.Entity<Inmueble>()
                .HasKey(c => c.IdInmueble);

            modelBuilder.Entity<Pago>()
                .HasKey(c => c.IdPago);

            modelBuilder.Entity<Inquilino>()
                .HasKey(c => c.IdInquilino);
        }
        public DbSet<Propietario> Propietario { get; set; }
        public DbSet<Inmueble> Inmueble { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Pago> Pago { get; set; }
    }
}
