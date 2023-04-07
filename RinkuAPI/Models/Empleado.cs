using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinkuAPI.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        public String Nombre { get; set; }

        public int IdRol { get; set; }

        public String Rol { get; set; }

        public bool Estatus { get; set; }

    }
}