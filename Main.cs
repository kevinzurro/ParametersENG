using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Parameters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Main : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string RutaDelEnsamblado = System.Reflection.Assembly.GetExecutingAssembly().Location;

            application.CreateRibbonTab("Parameters");

            RibbonPanel panelScanner = application.CreateRibbonPanel("Parameters", "Parameters");

            PushButton botonParameter = panelScanner.AddItem(new PushButtonData("btnParameter", "Parameter\nScanner", RutaDelEnsamblado, "Parameters.ParameterScanner")) as PushButton;

            botonParameter.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Parameters;component/Resources/Icons32x32.png"));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
