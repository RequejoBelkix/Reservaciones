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
    public class LaboratorioCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Laboratorio")]
        public string Nombre { get; set; }
    }
    [MetadataType(typeof(LaboratorioCE))]
    public partial class Laboratorio
    {
        public string NombreS { get; set; }

    }
}
