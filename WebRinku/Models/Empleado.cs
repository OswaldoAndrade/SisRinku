using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRinku.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public int Rol { get; set; }
        public bool Estatus { get; set; }

    }
}