using AlarmJAHM.Models;
using AlarmJAHM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlarmJAHM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmasManagerPage : ContentPage
    {
        private AlarmaManagerViewModel _ViewModel;

        public AlarmasManagerPage()
        {
            BindingContext = _ViewModel = new AlarmaManagerViewModel(Navigation);

            InitializeComponent();

            MessagingCenter.Unsubscribe<string>(this, "horaActual");
            MessagingCenter.Subscribe<string>(this, "horaActual", (value) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    var tmp = value;

                    if (_ViewModel != null && _ViewModel.DatosAlarmas != null)
                    {
                        foreach (var alarma in _ViewModel.DatosAlarmas)
                        {
                            var horaActual = DateTime.Now;
                            horaActual = horaActual.AddSeconds(-horaActual.Second).AddMilliseconds(-horaActual.Millisecond);

                            if (alarma.Hora != null && alarma.HoraTimeSpan.Subtract(horaActual.TimeOfDay).TotalMilliseconds < 100)
                            {
                                if (!alarma.Notificado)
                                {
                                    alarma.Notificado = true;
                                    DependencyService.Get<INotification>().CreateNotification(alarma.Titulo, alarma.Descripcion);
                                }
                            }
                        }
                    }
                });
            });

            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _ViewModel.CargarDatos();

            MessagingCenter.Send<string>("1", "AlarmServiceJAHM");
        }
    }
}