using System.ComponentModel.DataAnnotations;
using VentaDeVehiculos.Datos;

namespace VentaDeVehiculos.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public RolEnum Rol { get; set; }

        public virtual List<Venta>? Ventas { get; set; }
    }
}
