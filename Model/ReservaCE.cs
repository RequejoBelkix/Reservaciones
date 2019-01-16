using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReservaCE
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Fecha { get; set; }
        [Required]
        [Display(Name = "Motivo")]
        public Nullable<int> IdMotivo { get; set; }
        public string Motivos { get; set; }
        [Required]
        [Display(Name = "Ciclo")]
        public string Ciclo{ get; set; }
        [Required]
        [Display(Name = "Hora_Inicio")]
        [DataType(DataType.Time)]
        public Nullable<System.TimeSpan> Hora_Iinicio { get; set; }
        [Required]
        [Display(Name = "Hora_Fin")]
        [DataType(DataType.Time)]
        public Nullable<System.TimeSpan> Hora_Fin { get; set; }
        [Required]
        [Display(Name = "Laboratorio")]
        public Nullable<int> IdLaboratorio { get; set; }
        public string Laboratorios { get; set; }

        [Display(Name = "Novedades")]
        public string Novedades { get; set; }
        [Required]
        [Display(Name = "Laboratorio")]
        public Task<string> IdUsuario { get; set; }
        public string Usuarios { get; set; }
    }

    [MetadataType(typeof(ReservaCE))]
    public partial class Reserva
    {
        public string Motivos { get; set; }
        public string Laboratorios { get; set; }
        public string Usuarios { get; set; }
    }
}
