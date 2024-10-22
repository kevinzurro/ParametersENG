using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Parameters.Base
{
    public class ModelBase
    {
        private UIApplication uiApp = null;
        private UIDocument uiDoc = null;
        private Application app = null;
        private Document doc = null;
        private string idiomaDelPrograma;

        public UIApplication UIApp
        {
            get { return uiApp; }
            set { uiApp = value; }
        }

        public UIDocument UIDoc
        {
            get { return uiDoc; }
            set { uiDoc = value; }
        }

        public Application Appli
        {
            get { return app; }
            set { app = value; }
        }

        public Document Doc
        {
            get { return doc; }
            set { doc = value; }
        }
    }
}
