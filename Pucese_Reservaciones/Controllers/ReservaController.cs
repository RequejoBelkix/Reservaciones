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
    public class ReservaController : Controller
    {
        ReservacionesEntities db = new ReservacionesEntities();
        // GET: Reserva
        public ActionResult Index()
        {
            try
            {
                string sql = @"SELECT Reserva.Id, Reserva.Fecha, Motivo.Nombre AS Motivos,Reserva.Ciclo,Reserva.Hora_Iinicio, Reserva.Hora_Fin,
         Laboratorio.Nombre AS Laboratorios,Reserva.Estado,Usuario.Nombre AS Usuarios,Reserva.Novedades FROM Reserva

                INNER JOIN Motivo ON Reserva.IdMotivo = Motivo.Id
                INNER JOIN Laboratorio ON Reserva.IdLaboratorio = Laboratorio.Id
                INNER JOIN Usuario ON Reserva.IdUsuario = Usuario.Cedula";
                using (var db = new ReservacionesEntities())
                {
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Reserva", "VISUALIZO LISTA DE LAS RESERVAS");
                    }
                    return View(db.Database.SqlQuery<ReservaCE>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Details(Reserva al)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Reserva alu = db.Reservas.Find(al.Id);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Reserva", "DETALLE DE UNA RESERVA");
                    }
                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Encontrar Reserva", ex);
                return View();
            }
        }
        public ActionResult CreateReserva(Reserva a)
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
                   db.Reservas.Add(a);
                    /*if (K != null)
                    {
                        Auditoria(K.Cedula, "Reserva", "CREO UNA RESERVA");
                    */
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar una Reserva", ex);
                return View();
            }
        }
       
        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    //Alumno al = db.Alumno.Where(a => a.id == id).FirstOrDefault();
                    Reserva al2 = db.Reservas.Find(id);
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
        public ActionResult Editar(Reserva a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    // Auditoria(Cedula, "Usuario", "Edito un Usuario");
                    Reserva al = db.Reservas.Find(a.Id);
                    al.Fecha = a.Fecha;
                    al.Ciclo= a.Ciclo;
                    al.Hora_Iinicio = a.Hora_Iinicio;
                    al.Hora_Fin = a.Hora_Fin;
                    al.IdLaboratorio= a.IdLaboratorio;
                    al.IdMotivo = a.IdMotivo;
                    al.IdUsuario = a.IdUsuario;
                    al.Novedades = a.Novedades;
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Reserva", "EDITO UNA RESERVA");
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

        
        public ActionResult Delete( Reserva usu)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    usu = db.Reservas.Find(usu.Id);
                    db.Reservas.Remove(usu);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Reserva", "ELIMINO UNA RESERVA");
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


      

        public ActionResult ListaLaboratorio()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Laboratorios.ToList());
            }
        }
        public ActionResult ListaUsuario()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Usuarios.ToList());
            }
        }
        public ActionResult ListaMotivo()
        {
            using (var db = new ReservacionesEntities())
            {
                return PartialView(db.Motivoes.ToList());
            }
        }
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
    }
}
