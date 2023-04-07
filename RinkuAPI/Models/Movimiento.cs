using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinkuAPI.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public String Nombre { get; set; }
        public String Rol { get; set; }
        public String Mes { get; set; }
        public int Entregas { get; set; }
        
        private decimal sueldoBruto;
        public String SueldoBruto { get { return string.Format("{0:C}", sueldoBruto); } set { sueldoBruto = Convert.ToDecimal(value); } }


        private decimal retenciones;
        public String Retenciones { get { return string.Format("{0:C}", retenciones); } set { retenciones = Convert.ToDecimal(value); } }


        private decimal bonoDespensa;
        public String BonoDespensa { get { return string.Format("{0:C}", bonoDespensa); } set { bonoDespensa = Convert.ToDecimal(value); } }


        private decimal sueldoNeto;
        public String SueldoNeto { get { return string.Format("{0:C}", sueldoNeto); } set { sueldoNeto = Convert.ToDecimal(value); } }
    }
}