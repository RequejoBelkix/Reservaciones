using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CEAuditoria
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuarios")]
        public string IdUsuario { get; set; }
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
    [MetadataType(typeof(CEAuditoria))]
    public partial class Auditoria
    {
        public string Usuarios { get; set; }
    }
}
