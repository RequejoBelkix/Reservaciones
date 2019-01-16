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
    public class MotivoCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Motivo")]
        public string Nombre { get; set; }
    }
    [MetadataType(typeof(MotivoCE))]
    public partial class Motivo
    {
        public string NombreS { get; set; }

    }
}
