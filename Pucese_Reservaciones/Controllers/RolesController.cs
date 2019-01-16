using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;
using Model;

namespace Pucese_Reservaciones.Controllers
{
    public class RolesController : Controller
    {
        ReservacionesEntities db = new ReservacionesEntities();
        // GET: Escuela
        public ActionResult Repor()
        {
            return View();
        }

        public ActionResult GetData()
        {
            int admin = db.Roles.Where(x => x.Nombre == "AMINISTRADOR").Count();
            int recepcion =db.Roles.Where(x => x.Nombre == "RECEPCION").Count();
            int doncente = db.Roles.Where(x => x.Nombre == "DOCENTES").Count();

            Clase obj = new Clase();
            obj.admin = admin;
            obj.recepcion = recepcion;
            obj.doncente = doncente;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public class Clase
        {
            public int admin;
            public int recepcion;
            public int doncente;

        }



        public ActionResult Index()
        {
            try
            {
                string sql = @"SELECT *FROM Roles";
                using (var db = new ReservacionesEntities())
                {
                    /*var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Rol", "VISUALIZO LISTA  DE ROLES");
                    }*/
                    return View(db.Database.SqlQuery<Role>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Details(Role al)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Role alu = db.Roles.Find(al.Id);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Rol", "DETALLE DE ROLES");
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
        public ActionResult ReporteUsr()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportReserva.rpt"));

            rd.SetDataSource(db.Escuelas.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Lista_de_Escuelas.pdf");
            }
            catch
            {
                throw;
            }
        }
        public ActionResult Create(Role a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    //Auditoria(Cedula, "Usuario", "AGREGO UN USUARIO");
                    db.Roles.Add(a);

                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Rol", "CREO UN ROLE");
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar una Escuela", ex);
                return View();
            }
        }




        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    //Alumno al = db.Alumno.Where(a => a.id == id).FirstOrDefault();
                   Role al2 = db.Roles.Find(id);
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
        public ActionResult Edit(Role a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Role al = db.Roles.Find(a.Id);
                    al.Nombre = a.Nombre;
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Rol", "EDITO UN ROLE");
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Editar el motivo", ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id, Role usu)
        {
            try
            {

                usu = db.Roles.Find(usu.Id);
                db.Roles.Remove(usu);
                var K = db.Usuarios.Single(u => u.Estado == "1");
                if (K != null)
                {
                    Auditoria(K.Cedula, "Rol", "ELIMINO UN ROLE");
                }
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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