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
        private ObservableCollection<ElementoParametro> todosParametros;
        private List<Element> todosElementos = new List<Element>();

        public ParameterScannerModel(UIApplication UIApp)
        {
            this.UIApp = UIApp;
            this.UIDoc = UIApp.ActiveUIDocument;
            this.Appli = UIApp.Application;
            this.Doc = UIDoc.Document;

            vistaActual = Doc.ActiveView;
            
            todosElementos = new FilteredElementCollector(Doc).WhereElementIsNotElementType().Cast<Element>().ToList();

            todosParametros = TodosLosParametros();
        }

        public ObservableCollection<ElementoParametro> TodosParametros
        {
            get { return todosParametros; }
        }

        private ObservableCollection<ElementoParametro> TodosLosParametros()
        {
            try
            {
                ObservableCollection<ElementoParametro> obsParam = new ObservableCollection<ElementoParametro>();

                List<ParameterElement> colector = new FilteredElementCollector(Doc).
                                                      OfClass(typeof(ParameterElement)).
                                                      Cast<ParameterElement>().
                                                      OrderBy(x=>x.Name).
                                                      ToList();

                foreach (ParameterElement defi in colector)
                {
                    if (defi != null)
                    {
                        obsParam.Add(new ElementoParametro(defi));
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

        public void AislarElementos(ElementoParametro parametro, object valor)
        {
            if(vistaActual is View3D ||
               vistaActual is ViewPlan &&
               vistaActual != null)
            {
                using(Transaction tr = new Transaction(Doc, "Isolate elements"))
                {
                    tr.Start();

                    vistaActual.TemporaryViewModes.DeactivateAllModes();

                    List<ElementId> elementos = new List<ElementId>();

                    foreach (Element elem in todosElementos)
                    {
                        Parameter param = elem.LookupParameter(parametro.Nombre);

                        if (param != null)
                        {
                            if (valor.ToString() == null || valor.ToString() == string.Empty)
                            {
                                elementos.Add(elem.Id);
                            }
                            else
                            {
                                string valorParametro = param.AsString() != null ? param.AsString() : param.AsValueString();

                                if (valorParametro == valor.ToString())
                                {
                                    elementos.Add(elem.Id);
                                }
                            }
                        }
                    }

                    vistaActual.IsolateElementsTemporary(elementos);

                    tr.Commit();
                }
            }
        }

        public void SeleccionarElementos(ElementoParametro Parametro)
        {

        }
    }
}
