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
    public class RolesCE
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public string Nombre { get; set; }
    }
    [MetadataType(typeof(RolesCE))]
    public partial class Role
    {
        public string NombreS { get; set; }

    }
}
