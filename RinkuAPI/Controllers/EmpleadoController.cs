using System;
using System.Collections.Generic; 
using System.Web.Http; 
using System.Data;
using System.Data.SqlClient;

using RinkuAPI.Models;
using RinkuAPI.Classes;

namespace RinkuAPI.Controllers
{
    public class EmpleadoController : ApiController
    {
        public DatabaseConnection dbconn = new DatabaseConnection();


        //static Dictionary<int, Empleado> empleados= new Dictionary<int, Empleado>();

        public List<Empleado> Get()
        {
            dbconn = new DatabaseConnection();
            dbconn.OpenConection(); 

            SqlDataReader dataReader = dbconn.DataReader("GetEmpleados");
            List<Empleado> listaEmpleados = new List<Empleado>();

            while (dataReader.Read())
            {
                listaEmpleados.Add(new Empleado
                {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Nombre = dataReader["Nombre"].ToString(),
                    IdRol = Convert.ToInt32(dataReader["IdRol"]),
                    Rol = dataReader["Rol"].ToString(),
                    Estatus = Convert.ToBoolean(dataReader["Estatus"])
                });
            }

            return listaEmpleados;
        }

        public Empleado Post([FromBody] Empleado empleado)
        {
            dbconn = new DatabaseConnection();
            dbconn.OpenConection();

            SqlDataReader dataReader = dbconn.DataReader($"AddEmpleado '{empleado.Nombre}', {empleado.Rol}");
            Empleado emp = new Empleado();

            if (dataReader.Read())
            {
                emp.Id = (Int32)dataReader["Id"];
                emp.Nombre = dataReader["Nombre"].ToString();
            }

            return emp;
        }

        public bool Put([FromBody] Empleado empleado)
        {
            bool res = false;
            dbconn = new DatabaseConnection();
            dbconn.OpenConection();

            SqlDataReader dataReader = dbconn.DataReader($"UpdateEmpleado {empleado.Id}, '{empleado.Nombre}', {empleado.Rol}, { empleado.Estatus}" );


            if (dataReader.Read())
            {
                res = Convert.ToBoolean(dataReader["Exito"]);
            }

            return res;
        }

        //public Empleado Get(int Id)
        //{
        //    Empleado emp;
        //    empleados.TryGetValue(Id, out emp);
        //    return emp;
        //}

        //public bool Post([FromBody] Empleado empleado)
        //{
        //    Empleado emp;
        //    empleados.TryGetValue(empleado.Id, out emp);
        //    if(emp == null)
        //    {
        //        empleados.Add(empleado.Id, empleado);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //// GET: Empleado
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Empleado/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Empleado/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Empleado/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Empleado/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Empleado/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Empleado/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Empleado/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
