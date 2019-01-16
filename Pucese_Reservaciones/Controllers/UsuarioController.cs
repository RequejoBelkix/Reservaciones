using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Model;

namespace Pucese_Reservaciones.Controllers
{

    public class UsuarioController : Controller
    {
        ReservacionesEntities db = new ReservacionesEntities();
        Usuario user = new Usuario();
        private string cedula;

         

        public string Cedula { get => cedula; set => cedula = value; }
        public string contraseña = "";

        //Realizacion de tiempo de ejecucion

        // GET: Usuario
        public ActionResult Index()
        {
            try
            {
                string sql = @"SELECT Usuario.Id, Usuario.Cedula, Usuario.Nombre, Usuario.Apellido, Usuario.Genero, Escuela.Nombre AS NombreEscuela, Profesion.Nombre AS Profesiones,
                Usuario.Ttoken, Usuario.Email, Roles.Nombre AS Rol, Usuario.Estado, CONCAT(Provincia.Nombre, ', ', Canton.Nombre, ', ', Parroquia.Nombre)
                AS Direcciones FROM Usuario

                INNER JOIN Escuela ON Usuario.IdEscuela = Escuela.Id 
                INNER JOIN Profesion ON Usuario.IdProfesion = Profesion.Id
                INNER JOIN Roles ON Usuario.IdRol = Roles.Id 
                INNER JOIN Parroquia ON Usuario.Direccion = Parroquia.Id
                INNER JOIN Canton ON Parroquia.IdCant = Canton.Id 
                INNER JOIN Provincia ON Canton.IdProv = Provincia.Id";
                using (var db = new ReservacionesEntities())
                {
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                   if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "VISUALIZO LISTA DE USUARIOS");
                    }
                    //Auditoria(Cedula, "Usuario", "VISUALIZO LISTA DE USUARIOS");
                    return View(db.Database.SqlQuery<UsuarioCE>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Usario()
        {
            try
            {
                string sql = @"SELECT Laboratorio.Nombre AS Laboratorios FROM Reserva 
	            INNER JOIN Laboratorio ON Reserva.IdLaboratorio = Laboratorio.Id WHERE IdUsuario=1004601009";
                using (var db = new ReservacionesEntities())
                {
                   
                    
                    return View(db.Database.SqlQuery<Reserva>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }






        public ActionResult Edita(string id)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    //Alumno al = db.Alumno.Where(a => a.id == id).FirstOrDefault();
                    Usuario al2 = db.Usuarios.Find(id);
                    return View(al2);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Editar al Alumno", ex);
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edita(Usuario a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    // Auditoria(Cedula, "Usuario", "Edito un Usuario");
                    Usuario al = db.Usuarios.Find(a.Cedula);
                    al.Cedula = a.Cedula;
                    al.Nombre = a.Nombre;
                    al.Apellido = a.Apellido;
                    al.Genero = a.Genero;
                    al.IdEscuela = a.IdEscuela;
                    al.IdProfesion = a.IdProfesion;
                    al.Ttoken = a.Ttoken;
                    al.Email = a.Email;
                    al.IdRol = a.IdRol;
                    al.Estado = a.Estado;
                    al.Direccion = a.Direccion;
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "EDITO UN USUARIO");
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Editar al Usuario", ex);
                return View();
            }
        }



        public ActionResult Delete(Usuario usu)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    usu = db.Usuarios.Find(usu.Cedula);
                    db.Usuarios.Remove(usu);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                     {
                         Auditoria(K.Cedula, "Usuario", "ELIMINO UN USUARIO");
                     }
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");

                }

            }
            catch
            {
                return View();
            }
        }





        public ActionResult Detalle(Usuario al)
        {
            var K = db.Usuarios.Single(u => u.Estado == "1");
            try
            {
                using (var db = new ReservacionesEntities())
                {
                   Usuariolab(al.Cedula);
                   Usuario alu = db.Usuarios.Find(al.Cedula);
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "DETALLE USUARIO");
                    }

                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Encontrar al Usuario", ex);
                return View();
            }
        }

        // GET: Usuario
        public ActionResult AgregarUsuario(Usuario a)
        {
            //verificar si sle modelo es valido

            var K = db.Usuarios.Single(u => u.Estado == "1");
           

            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {
                   
                        //Auditoria(Cedula, "Usuario", "AGREGO UN USUARIO");
                        db.Usuarios.Add(a);
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "ELIMINO UN USUARIO");
                    }
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar a los Usuarios", ex);
                return View();
            }
        }

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


        public int con = 1;
        public void Auditoria(string cedula, string tabla, string descripcion)
        {
            
            IDbConnection _conn = DBComon.Conexion();
            _conn.Open();
            SqlCommand _Comand = new SqlCommand("AGREGAR_AUDITORIA", _conn as SqlConnection);
            _Comand.CommandType = CommandType.StoredProcedure;
            _Comand.Parameters.Add(new SqlParameter("@IDUSUARIO", cedula));
            _Comand.Parameters.Add(new SqlParameter("@TBAUDITORIA", tabla));
            _Comand.Parameters.Add(new SqlParameter("@DESCRIPCION", descripcion));
            _Comand.Parameters.Add(new SqlParameter("@FECHA", DateTime.Now));
            _Comand.Parameters.Add(new SqlParameter("@HORA", DateTime.Now));
            int Resultado = _Comand.ExecuteNonQuery();
            _conn.Close();



        }
        //Login 
        [HttpGet]
        public ActionResult Login()
        {

            return View();

        }
        public void SendTe(string contraseña, string tToken, string chat_id)
        {

            Telegram.bot bot = new Telegram.bot();
            Telegram.bot.token = tToken;

            Telegram.bot.update = "true";
            Telegram.bot.sendMessage.send(chat_id, "Hola! el usuario " + Convert.ToString(contraseña) + " ha iniciado Sesion");
            Telegram.bot.update = "true";
        }


        public ActionResult Login(Usuario user)
        {
            string message = "Inicio", Estado = "";

            using (ReservacionesEntities db = new ReservacionesEntities())
            {
                // var v = db.Usuarios.Where(a => a.Estado == Estado);
                var usr = db.Usuarios.Single(u => u.Email == user.Email && u.Pass == user.Pass);
                if (usr != null)
                {
                    if (usr.Estado == "0")
                    {
                        Auditoria(usr.Cedula, "Usuario", "Inicio de Sesion");
                        usr.Estado = "1";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Inicio de sesion";
                        Session["Id"] = usr.Id.ToString();
                        Session["Email"] = usr.Nombre.ToString();

                        // enviar mensaje a telegram
                        SendTe(usr.Nombre, usr.Ttoken,usr.Chat_id);
                        return RedirectToAction("loggedIn");
                    }
                    else
                    {
                        message = "Ya ha iniciado sesion";
                    }
                }
                else
                {
                    ModelState.AddModelError(" ", "CREDENCIALES INCORRECTAS");
                }

            }

            ViewBag.Message = message;
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Logout()
        {
            string Estado = "1";
            using (ReservacionesEntities db = new ReservacionesEntities())
            {
                var v = db.Usuarios.Single(a => a.Estado == Estado);
                if (v != null)
                {
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "CERRO SESION");
                    }
                    v.Estado = "0";
                    v.Estado.ToUpper();

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    FormsAuthentication.SignOut();
                    
                    //Auditoria(Cedula, "Usuario", "CERRO SESION");
                    return RedirectToAction("Login", "Usuario");

                }


            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Usuario");
        }
        public int bandera = 0;

        public ActionResult ActualizarContraseña(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarContraseña(ResetPasswordModel users)
        {
            var message = "";
            if (bandera == 1)
            {
                message = "El link para restaurar su contraseña ah sido enviada a su Correo";
                ViewBag.Message = message;
            }
            var usr = db.Usuarios.Single(a => a.ResetCode == users.ResetCode);
            message = "Fuera del If null";
            if (usr != null)
            {
                message = "Fuera del If pass passold";
                if (users.Pass == users.PassOld)
                {
                    message = "Adentro";
                    Auditoria(Cedula, "Usuario", "ACTUALIZO SU CONTRASEÑA");
                    usr.Pass = users.Pass;
                    usr.PassOld = "NULL";
                    usr.ResetCode = "NULL";
                    usr.Estado = "0";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    message = "Antes de save";
                    db.SaveChanges();
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "ACTUALIZO SU CONTRASEÑA");
                    }
                    message = "Despues de save";
                    return RedirectToAction("Login", "Usuario");
                }

            }
            else
            {
                message = "Contraseñas incorrectas";
            }

            ViewBag.Message = message;
            return View();

        }


        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (ReservacionesEntities dc = new ReservacionesEntities())
            {
                var v = dc.Usuarios.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }







        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string contraseña)
        {
            var verifyUrl = "/Usuario/" + "ActualizarContraseña";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("maritzarequejo2208@gmail.com", "Activacion de cuenta");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Maritza1969"; // Replace with actual password
            string subject = "";
            string body = "";

            subject = "Reiniciar Contraseña";

            body = "Hola<br/<br/>" + " tu contraseña es: " + contraseña + "<br/<br/> Vemos que quieres recuperar tu contraseña. Has clic en el link de abajo para resetear tu contraseña antigua" +
                "<br/><br/><a href= " + link + "> Link de Reseteo de Contraseña</a>";



            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        //Part 3 - Forgot Password

        public void SendTelegram(string contraseña, string tToken, string chat_id)
        {

            Telegram.bot bot = new Telegram.bot();
            Telegram.bot.token = tToken;

            Telegram.bot.update = "true";
            Telegram.bot.sendMessage.send(chat_id, "Hola! su contraseña de recuperacion es: " + Convert.ToString(contraseña) + " Introduzcala para cambiar su contraseña");
            Telegram.bot.update = "true";
        }
        public ActionResult Recuperacion()
        {
            ViewBag.Message = message;
            return View();
        }
        public ActionResult Recuperar_ContraseñaToken()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperar_ContraseñaToken(string Cedula)
        {
            bandera = 0;
            ResetPasswordModel rs = new ResetPasswordModel();
            
            contraseña = Generarcontraseña();
            rs.ResetCode = contraseña;


            using (ReservacionesEntities dc = new ReservacionesEntities())
            {
                var account = dc.Usuarios.Where(a => a.Cedula == Cedula).First();
                if (account != null)
                {
                    //Auditoria(Cedula, "Usuario", "RECUPERO SU CONTRASEÑA");
                    string resetCode = Guid.NewGuid().ToString();
                    SendTelegram(contraseña, account.Ttoken, account.Chat_id);
                    //SendVerificationLinkEmail(account.Email, resetCode, contraseña);
                    account.ResetCode = contraseña;
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "RECUPERO SU CONTRASEÑA");
                    }
                    bandera = 1;
                }
                else
                {
                    message = "Cuenta no encontrada";
                }
            }
            ViewBag.Message = message;
            return RedirectToAction("ActualizarContraseña", "Usuario");
        }


        public ActionResult Recuperar_Contraseña()
        {
            return View();
        }
        string message = "";

        [HttpPost]
        public ActionResult Recuperar_Contraseña(string EmailID)
        {


            ResetPasswordModel rs = new ResetPasswordModel();
           
            contraseña = Generarcontraseña();
            rs.ResetCode = contraseña;

            using (ReservacionesEntities dc = new ReservacionesEntities())
            {
                bandera = 0;
                var account = dc.Usuarios.Where(a => a.Email == EmailID).First();
                if (account != null)
                {
                    // Auditoria(Cedula, "Usuario", "ERECUPERO SU CONTRASEÑA");
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, contraseña);
                    account.ResetCode = contraseña;
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Usuario", "RECUPERO SU CONTRASEÑA");
                    }
                    bandera = 1;
                }
                else
                {
                    message = "Cuenta no encontrada";
                }
            }
            ViewBag.Message = message;
            return RedirectToAction("ActualizarContraseña", "Usuario");
        }








        public string Generarcontraseña()
        {
            string[] a = new string[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"};
            string[] b = new string[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t",
                "u","v","w","x","y","z"};

            string[] c = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string[] d = new string[] { "$", "!", "#", "%", "&", "/", "(", ")", "=", "-", "_", ".", ":", ",", ";", "+", "*", "@", "<", ">" };

            Random r = new Random();
            int v;
            v = r.Next(0, a.Length);
            contraseña = contraseña + a[v];
            v = r.Next(0, a.Length);
            contraseña = contraseña + a[v];
            v = r.Next(0, b.Length);
            contraseña = contraseña + b[v];
            v = r.Next(0, b.Length);
            contraseña = contraseña + b[v];
            v = r.Next(0, c.Length);
            contraseña = contraseña + c[v];
            v = r.Next(0, c.Length);
            contraseña = contraseña + c[v];
            v = r.Next(0, d.Length);
            contraseña = contraseña + d[v];
            v = r.Next(0, d.Length);
            contraseña = contraseña + d[v];

            return contraseña;


        }



        //DECLARACION DE LAS LISTAS
        public ActionResult ListaEscuelas()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Escuelas.ToList());
            }
        }


        public ActionResult ListaProfesion()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Profesions.ToList());
            }
        }

        public ActionResult ListaRoles()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Roles.ToList());
            }
        }

        public ActionResult ListaDireccion()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Parroquias.ToList());
            }
        }

        public ActionResult ListaCanton()
        {
            using (var db = new ReservacionesEntities())
            {
                //SELECT Canton.Nombre, Provincia.Id, Canton.Id FROM Canton, Provincia WHERE Canton.IdProv = (SELECT Provincia.Id FROM Provincia WHERE Provincia.Nombre = 'ESMERALDAS')  and Provincia.Id = (SELECT Provincia.Id FROM Provincia WHERE Provincia.Nombre = 'ESMERALDAS')< 
                return PartialView(db.Cantons.ToList());
            }
        }

        public ActionResult ListaReserva()
        {
            using (var db = new ReservacionesEntities())
            {
                //SELECT Canton.Nombre, Provincia.Id, Canton.Id FROM Canton, Provincia WHERE Canton.IdProv = (SELECT Provincia.Id FROM Provincia WHERE Provincia.Nombre = 'ESMERALDAS')  and Provincia.Id = (SELECT Provincia.Id FROM Provincia WHERE Provincia.Nombre = 'ESMERALDAS')< 
                return PartialView(db.Reservas.ToList());
            }
        }

        public ActionResult ListaProvincia()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Provincias.ToList());



            }
        }
    }
}