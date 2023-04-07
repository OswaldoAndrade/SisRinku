using System;
using System.Collections.Generic; 
using System.Web.Http; 
using RinkuAPI.Models;
using RinkuAPI.Classes;
using System.Data;
using System.Data.SqlClient;

namespace RinkuAPI.Controllers
{
    public class RolController : ApiController
    {
        public DatabaseConnection dbconn = new DatabaseConnection();

        public List<Rol> Get()
        {
            dbconn = new DatabaseConnection();
            dbconn.OpenConection();

            SqlDataReader dataReader = dbconn.DataReader("GetRoles");
            List<Rol> listaRoles = new List<Rol>(); 

            while (dataReader.Read())
            {
                listaRoles.Add(new Rol
                { 
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Descripcion = dataReader["Descripcion"].ToString(),
                    Estatus = Convert.ToBoolean(dataReader["Estatus"])
                });
            }

            return listaRoles;
        }
        //    // GET: Rol
        //    public ActionResult Index()
        //    {
        //        return View();
        //    }

        //    // GET: Rol/Details/5
        //    public ActionResult Details(int id)
        //    {
        //        return View();
        //    }

        //    // GET: Rol/Create
        //    public ActionResult Create()
        //    {
        //        return View();
        //    }

        //    // POST: Rol/Create
        //    [HttpPost]
        //    public ActionResult Create(FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add insert logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // GET: Rol/Edit/5
        //    public ActionResult Edit(int id)
        //    {
        //        return View();
        //    }

        //    // POST: Rol/Edit/5
        //    [HttpPost]
        //    public ActionResult Edit(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add update logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // GET: Rol/Delete/5
        //    public ActionResult Delete(int id)
        //    {
        //        return View();
        //    }

        //    // POST: Rol/Delete/5
        //    [HttpPost]
        //    public ActionResult Delete(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add delete logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
        //}
    }
}
