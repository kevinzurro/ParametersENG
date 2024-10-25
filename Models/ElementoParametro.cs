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
        private string nombre;

        public ElementoParametro(ParameterElement param)
        {
            if (param != null)
            {
                Nombre = param.Name;
            }
        }

        public ElementoParametro(Parameter param)
        {
            if(param != null)
            {
                Nombre = param.Definition.Name;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
