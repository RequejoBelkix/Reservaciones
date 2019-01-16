using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AuditoriaCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuarios")]
        public Nullable<int> IdUsuario { get; set; }
        public string Usuarios { get; set; }

        [Required]
        [Display(Name = "Tabla")]
        public string TbAuditoria { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Fecha { get; set; }

        [Required]
        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        public Nullable<System.TimeSpan> Hora { get; set; }
    }
    /*[MetadataType(typeof(AuditoriaCE))]
    public partial class Auditoria
    {
        public string Usuarios { get; set; }

    }*/
}
