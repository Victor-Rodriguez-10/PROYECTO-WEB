using System.ComponentModel.DataAnnotations;

namespace VentaDeVehiculos.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Celular { get; set; }
        public int Ci { get; set; }
        public string? Direccion { get; set; }

        public virtual List<Venta>? Ventas { get; set; }
    }
}
