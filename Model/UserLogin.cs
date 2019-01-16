using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserLogin
    {
        [Display(Name = "Ingrese su Email ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Es necesario ingresar su correo")]
        public string Email { get; set; }

        [Display(Name = "Ingrese su contraseña ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Es necesario ingresar su contraseña")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

    }
}