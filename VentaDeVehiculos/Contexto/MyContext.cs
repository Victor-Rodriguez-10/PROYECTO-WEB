using Microsoft.EntityFrameworkCore;
using VentaDeVehiculos.Models;

namespace VentaDeVehiculos.Contexto
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
    }

}