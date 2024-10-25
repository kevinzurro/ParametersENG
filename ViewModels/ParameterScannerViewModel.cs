using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Windows;
using Parameters.Base;
using Parameters.Models;
using System.Diagnostics;

namespace Parameters.ViewModels
{
    public class ParameterScannerViewModel : ViewModelBase
    {
        private ParameterScannerModel model;
        private ObservableCollection<ElementoParametro> parametros;
        private ElementoParametro parametro;
        private string valorParametro = string.Empty;

        public ParameterScannerViewModel() { }

        public ParameterScannerViewModel(ParameterScannerModel mo)
        {
            Modelo = mo;

            Parametros = Modelo.TodosLosParametros();

            if (Parametros.Count > 0)
            {
                Parametro = Parametros.FirstOrDefault();
            }
        }

        public ParameterScannerModel Modelo
        {
            get { return this.model; }
            set 
            { 
                this.model = value; 
            }
        }

        public ObservableCollection<ElementoParametro> Parametros
        {
            get { return parametros; }
            set
            {
                parametros = value;
                OnPropertyChanged(nameof(Parametros));
            }
        }

        public ElementoParametro Parametro
        {
            get { return parametro; }
            set
            {
                if (Parametros.Contains(value))
                {
                    parametro = value;
                    OnPropertyChanged(nameof(Parametro));
                }
            }
        }

        public string ValorParametro
        {
            get { return valorParametro; }
            set
            {
                valorParametro = value;
                OnPropertyChanged(nameof(ValorParametro));
            }
        }

        public RelayCommand AislarCommand => new RelayCommand(execute => AislarElementos(execute), canExecute => { return true; });

        public RelayCommand SeleccionarCommand => new RelayCommand(execute => SeleccionarElementos(execute), canExecute => { return true; });

        private void AislarElementos(object ventana)
        {
            if (ValorParametro is null)
            {
                ValorParametro = string.Empty;
            }

            Modelo.AislarElementos(Parametro, ValorParametro);

            try
            {
                (ventana as Window).Activate();
            }
            catch (Exception) { }
        }

        private void SeleccionarElementos(object ventana)
        {
            Modelo.SeleccionarElementos(Parametro);

            if (ventana is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
