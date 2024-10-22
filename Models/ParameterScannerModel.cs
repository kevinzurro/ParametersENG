using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Parameters.Views;
using Parameters.ViewModels;
using Parameters.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Data.OleDb;

namespace Parameters.Models
{
    public class ParameterScannerModel : ModelBase
    {
        private View vistaActual;
        private List<Element> todosElementos;

        public ParameterScannerModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            vistaActual = Doc.ActiveView;

            todosElementos = new FilteredElementCollector(Doc).WhereElementIsNotElementType().Cast<Element>().ToList();
        }

        public ObservableCollection<ElementoParametro> Parametros()
        {
            try
            {
                ObservableCollection<ElementoParametro> obsParam = new ObservableCollection<ElementoParametro>();

                List<ParameterElement> colector = new FilteredElementCollector(Doc).
                                                      OfClass(typeof(ParameterElement)).
                                                      Cast<ParameterElement>().ToList();

                colector = colector.OrderBy(x => x.Name).ToList();

                foreach (ParameterElement defi in colector)
                {
                    if (defi != null)
                    {
                        ElementoParametro elem = new ElementoParametro(defi);

                        obsParam.Add(elem);
                    }
                }

                return obsParam;
            }
            catch (Exception e) 
            { 
                Debug.WriteLine(e.Message + "\n" + e.StackTrace);

                return null;
            }
        }

        public void AislarElementos(ElementoParametro parametro)
        {
            if(vistaActual is View3D ||
               vistaActual is ViewPlan &&
               vistaActual != null)
            {
                using(Transaction tr = new Transaction(Doc, "Isolates elements"))
                {
                    tr.Start();

                    List<ElementId> elementos = new List<ElementId>();

                    foreach (Element elem in todosElementos)
                    {
                        Parameter param = elem.LookupParameter(parametro.Nombre);

                        if (param != null)
                        {
                            elementos.Add(elem.Id);
                        }
                    }

                    vistaActual.IsolateElementsTemporary(elementos);

                    tr.Commit();
                }
            }
        }
    }
}
