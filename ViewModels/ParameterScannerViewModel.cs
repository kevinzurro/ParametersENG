using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parameters.Base;
using Parameters.Models;

namespace Parameters.ViewModels
{
    public class ParameterScannerViewModel : ViewModelBase
    {
        private ParameterScannerModel model;
        private ObservableCollection<ElementoParametro> parametros;
        private ElementoParametro parametro;

        public ParameterScannerViewModel() { }

        public ParameterScannerViewModel(ParameterScannerModel mo)
        {
            Modelo = mo;
            Parametros = Modelo.Parametros();
            Parametro = Parametros.First();
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
                parametro = value;
                OnPropertyChanged(nameof(Parametro));
            }
        }

        public RelayCommand AislarCommand => new RelayCommand(execute => AislarElementos(), canExecute => { return true; });

        public RelayCommand SeleccionarCommand => new RelayCommand(execute => SeleccionarElementos(), canExecute => { return true; });

        private void AislarElementos()
        {
            Modelo.AislarElementos(Parametro);
        }

        private void SeleccionarElementos()
        {

        }
    }
}
