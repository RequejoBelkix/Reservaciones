using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ResetPasswordModel
    {

        [Display(Name = "Ingresela contraseña enviada ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Es necesario ingresar la clave")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])\S+$", ErrorMessage = "La contraseña debe tener  numeros, letras mayusculas, letras minusculas, caracteres especiales y 8 caracteres")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener 8 Caracteres")]
        public string ResetCode { get; set; }


        [Display(Name = "Ingrese su nueva contraseña ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Es necesario ingresar la clave")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])\S+$", ErrorMessage = "La contraseña debe tener  numeros, letras mayusculas, letras minusculas, caracteres especiales y 8 caracteres")]
        [MinLength(8, ErrorMessage = "Minimo 8 Caracteres")]
        public string Pass { get; set; }


        [Display(Name = "Confirme su contraseña ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Es necesario ingresar la clave")]
        [DataType(DataType.Password)]
        [Compare("Pass", ErrorMessage = "Las contraseñas no coinciden.")]
        public string PassOld { get; set; }



    }
}
