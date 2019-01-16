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
    public class UsuarioCE
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Cedula")]
        public string Cedula { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Apellidos")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Required]
        [Display(Name = "Genero")]
        public string Genero { get; set; }

        [Required]
        [Display(Name = "Escuela:")]
        public Nullable<int> IdEscuela { get; set; }
        public string NombreEscuela { get; set; }
        [Required]
        [Display(Name = "Ingrese el Profesion")]
        public Nullable<int> IdProfesion { get; set; }
        public string Profesiones { get; set; }
        [Required]
        [Display(Name = "Token")]
        public string Ttoken { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public Nullable<int> IdRol { get; set; }
        public string Rol { get; set; }



        [Required]
        [Display(Name = "Direccion")]
        public Nullable<int> Direccion { get; set; }
        public string Direcciones { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña requerido")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimo 8 Caracteres")]
        public string Pass { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vuelva a ingresarla")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimo 8 Caracteres")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-zA-Z]).{10,50})", ErrorMessage = "La contraseña debe tener mayusculas, minusculas, numeros y caracteres especiales")]
        public string PassOld { get; set; }



       

    }
    [MetadataType(typeof(UsuarioCE))]
    public partial class Usuario
    {

        public void Usuariolab(string cedula)
        {

            IDbConnection _conn = DBComon.Conexion();
            _conn.Open();
            SqlCommand _Comand = new SqlCommand("CONSULTAR_LABORATORIOS", _conn as SqlConnection);
            _Comand.CommandType = CommandType.StoredProcedure;
            _Comand.Parameters.Add(new SqlParameter("@CEDULA", cedula));
            int Resultado = _Comand.ExecuteNonQuery();
            _conn.Close();



        }

        public string NombreEscuela { get; set; }
        public string Profesiones { get; set; }
        public string Rol { get; set; }
        public string Direcciones { get; set; }

    }

}
