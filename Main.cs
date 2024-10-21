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
        public Result OnShutdown(UIControlledApplication application)
        {
            string RutaDelEnsamblado = System.Reflection.Assembly.GetExecutingAssembly().Location;

            RibbonPanel panelDetalleArmado = application.CreateRibbonPanel("Parameters", "Parameters");

            PushButton botonParameter = panelDetalleArmado.AddItem(new PushButtonData("btnParameter", "Parameter Scanner", RutaDelEnsamblado, "Parameters.cmdParameterScanner")) as PushButton;

            botonParameter.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Jump;component/Resources/Ico.png"));

            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
