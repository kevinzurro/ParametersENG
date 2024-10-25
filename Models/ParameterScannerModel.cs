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
        }

        public ObservableCollection<ElementoParametro> TodosLosParametros()
        {
            ObservableCollection<ElementoParametro> obsParam = new ObservableCollection<ElementoParametro>();

            List<ParameterElement> colector = new FilteredElementCollector(Doc).
                                                  OfClass(typeof(ParameterElement)).
                                                  Cast<ParameterElement>().
                                                  OrderBy(x => x.Name).
                                                  ToList();

            foreach (ParameterElement paraElem in colector)
            {
                try
                {
                    if (paraElem != null && obsParam.Where(x => x.Nombre == paraElem.Name).ToList().Count == 0)
                    {
                        obsParam.Add(new ElementoParametro(paraElem));
                    }
                }
                catch (Exception) { }
            }

            return obsParam;
        }

        public void AislarElementos(ElementoParametro parametro, string value)
        {
            elementosSeleccionados.Clear();

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
                                string valorParametro = ObtenerValorDeParametro(param);

                                if (valorParametro == valor)
                                {
                                    elementosSeleccionados.Add(elem.Id);
                                }
                            }
                        }
                        catch (Exception) { }
                    }

                    if (elementosSeleccionados.Count > 0)
                    {
                        vistaActual.IsolateElementsTemporary(elementosSeleccionados);
                    }

                    tr.Commit();
                }

                if (elementosSeleccionados.Count > 0)
                {
                    TaskDialog.Show("Parameter Scanner", elementosSeleccionados.Count.ToString() + " items were found with the value " + valor);
                }
                else
                {
                    TaskDialog.Show("Parameter Scanner", "No items were found with the value");
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

        private string ObtenerValorDeParametro(Parameter param)
        {
            string valorParametro = string.Empty;

            switch (param.StorageType)
            {
                case StorageType.Double:
                    valorParametro = param.AsDouble().ToString();
                    break;

                case StorageType.ElementId:
                    valorParametro = param.AsValueString().ToString();
                    break;

                case StorageType.Integer:
                    if (param.Definition.ParameterType == ParameterType.YesNo)
                    {
                        valorParametro = param.AsValueString().ToString();
                    }
                    else
                    {
                        valorParametro = param.AsInteger().ToString();
                    }
                    break;

                case StorageType.String:
                    if (param.AsString() != null)
                    {
                        valorParametro = param.AsString();
                    }
                    else if (param.AsValueString() != null)
                    {
                        valorParametro = param.AsValueString();
                    }
                    else
                    {
                        valorParametro = string.Empty;
                    }
                    break;

                default:
                    valorParametro = string.Empty;
                    break;
            }

            return valorParametro;
        }
    }
}
