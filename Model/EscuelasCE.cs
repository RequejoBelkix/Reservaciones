using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EscuelasCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Escuela")]
        public string Nombre { get; set; }
    }
    [MetadataType(typeof(EscuelasCE))]
    public partial class Escuela
    {
        public string NombreS{ get; set; }

    }
}

