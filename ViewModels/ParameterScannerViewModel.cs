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

namespace Parameters.ViewModels
{
    public class ParameterScannerViewModel : ViewModelBase
    {
        private ParameterScannerModel model;
        private ObservableCollection<ElementoParametro> parametros;
        private ElementoParametro parametro;
        private object valorParametro = string.Empty;

        public ParameterScannerViewModel() { }

        public ParameterScannerViewModel(ParameterScannerModel mo)
        {
            Modelo = mo;
            Parametros = Modelo.TodosParametros;
            Parametro = Parametros.FirstOrDefault();
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

        public object ValorParametro
        {
            get { return valorParametro; }
            set
            {
                valorParametro = value;
                OnPropertyChanged(nameof(ValorParametro));
            }
        }

        public RelayCommand AislarCommand => new RelayCommand(execute => AislarElementos(), canExecute => { return true; });

        public RelayCommand SeleccionarCommand => new RelayCommand(execute => SeleccionarElementos(execute), canExecute => { return true; });

        private void AislarElementos()
        {
            Modelo.AislarElementos(Parametro, ValorParametro);
        }

        private void SeleccionarElementos(object parameter)
        {
            Modelo.SeleccionarElementos(Parametro);

            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
