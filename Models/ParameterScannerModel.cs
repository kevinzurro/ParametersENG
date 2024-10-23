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
using Autodesk.Revit.DB.Electrical;
using System.Drawing;

namespace Parameters.Models
{
    public class ParameterScannerModel : ModelBase
    {
        private View vistaActual;
        private ObservableCollection<ElementoParametro> todosParametros;
        private List<Element> todosElementos = new List<Element>();
        private List<ElementId> elementosSeleccionados = new List<ElementId>();

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
                    if (defi != null && obsParam.Where(x => x.Nombre == defi.Name).ToList().Count == 0)
                    {
                        obsParam.Add(new ElementoParametro(defi));
                    }
                }

                return obsParam;
            }

            catch (Exception){ return null; }
        }

        public void AislarElementos(ElementoParametro parametro, object value)
        {
            ViewType tipoVista = vistaActual.ViewType;

            string valor = value.ToString();

            if (tipoVista is ViewType.ThreeD ||
                tipoVista is ViewType.CeilingPlan ||
                tipoVista is ViewType.FloorPlan)
            {
                using(Transaction tr = new Transaction(Doc, "Isolate elements"))
                {
                    tr.Start();

                    vistaActual.TemporaryViewModes.DeactivateAllModes();

                    foreach (Element elem in todosElementos)
                    {
                        try
                        {
                            Parameter param = elem.LookupParameter(parametro.Nombre);

                            if (param != null)
                            {
                                if (valor == null || valor == string.Empty)
                                {
                                    elementosSeleccionados.Add(elem.Id);
                                }
                                else
                                {
                                    string valorParametro = param.AsString() != null ? param.AsString() : param.AsValueString();

                                    if (valorParametro == valor.ToString())
                                    {
                                        elementosSeleccionados.Add(elem.Id);
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                    }

                    if (elementosSeleccionados.Count > 0)
                    {
                        vistaActual.IsolateElementsTemporary(elementosSeleccionados);
                        
                        TaskDialog.Show("Parameter Scanner", elementosSeleccionados.Count.ToString() + " items were found with the value " + valor);
                    }

                    tr.Commit();
                }
            }
        }

        public void SeleccionarElementos(ElementoParametro Parametro)
        {
            if (elementosSeleccionados.Count > 0)
            {
                UIDoc.Selection.SetElementIds(elementosSeleccionados);
            }
        }
    }
}
