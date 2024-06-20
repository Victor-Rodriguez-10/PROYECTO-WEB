using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string? Inf { get { return $"{Direccion} - {Nombre} - {Ci} - {Celular}"; } }

        public virtual List<Venta>? Ventas { get; set; }
    }
}
