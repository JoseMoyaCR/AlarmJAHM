using AlarmJAHM.Models;
using AlarmJAHM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlarmJAHM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrmAlarmaPage : ContentPage
    {
        private AlarmaViewModel _ViewModel;

        public FrmAlarmaPage(ref ObservableCollection<Alarma> datosAlarmas)
        {
            // Indica que se va a crear una alarma/recordatorio
            BindingContext = _ViewModel = new AlarmaViewModel(Navigation, ref datosAlarmas, new Alarma { ID = -1 });

            InitializeComponent();
        }

        public FrmAlarmaPage(ref ObservableCollection<Alarma> datosAlarmas, Alarma alarma)
        {
            // Indica que se va a editar una alarma/recordatorio
            BindingContext = _ViewModel = new AlarmaViewModel(Navigation, ref datosAlarmas, alarma);

            InitializeComponent();
        }
    }
}