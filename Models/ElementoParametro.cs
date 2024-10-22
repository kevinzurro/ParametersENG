using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Parameters.Models
{
    public class ElementoParametro
    {
        private int id;
        private string nombre;

        public ElementoParametro(Parameter param)
        {
            ID = param.Id.IntegerValue;
            Nombre = param.Definition.Name;
        }

        public ElementoParametro(ParameterElement param)
        {
            ID = param.Id.IntegerValue;
            Nombre = param.Name;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string NombreID
        {
            get
            {
                return nombre + " <" + id + ">";
            }
        }

        public override string ToString()
        {
            return NombreID;
        }
    }
}
