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

        public FrmAlarmaPage()
        {
            // Indica que se va a crear una alarma/recordatorio
            BindingContext = _ViewModel = new AlarmaViewModel(Navigation, new Alarma { ID = -1 });

            InitializeComponent();
        }

        public FrmAlarmaPage(Alarma alarma)
        {
            // Indica que se va a editar una alarma/recordatorio
            BindingContext = _ViewModel = new AlarmaViewModel(Navigation, alarma);

            InitializeComponent();
        }
    }
}