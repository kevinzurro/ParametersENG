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
using Parameters.Models;

namespace Parameters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class ParameterScanner : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            using(TransactionGroup tg = new TransactionGroup(doc, "Parameter Scanner"))
            {
                tg.Start();

                ParameterScannerModel model = new ParameterScannerModel(uiApp);

                ParameterScannerViewModel paramVM = new ParameterScannerViewModel(model);

                WinParameterScanner scanner = new WinParameterScanner();

                scanner.DataContext = paramVM;

                scanner.ShowDialog();

                if (scanner.DialogResult == true)
                {
                    tg.Assimilate();
                }
                else
                {
                    tg.RollBack();
                }

                scanner.Close();
            }

            return Result.Succeeded;
        }
    }
}
