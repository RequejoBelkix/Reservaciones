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
    public class ProfesionCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Profesion")]
        public string Nombre { get; set; }
    }
    [MetadataType(typeof(ProfesionCE))]
    public partial class Profesion
    {
        public string NombreS { get; set; }

    }
}
