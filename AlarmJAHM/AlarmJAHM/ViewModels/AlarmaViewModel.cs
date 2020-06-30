using AlarmJAHM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlarmJAHM.ViewModels
{
    public class AlarmaViewModel : BindableObject
    { 
        private bool _IsBusy;

        private INavigation _Navigation;

        private Alarma _Alarma;


        public ObservableCollection<Alarma> DatosAlarmas { get; set; }

        public Command CancelarCommand { get; set; }

        public Command GuardarAlarmaCommand { get; set; }


        public bool IsBusy
        {
            get => _IsBusy;
            set
            {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }


        public Alarma AlarmaModel 
        { 
            get => _Alarma;
            set
            {
                _Alarma = value;
                OnPropertyChanged(nameof(AlarmaModel));
            }
        }


        private void InicializarCommands()
        {
            CancelarCommand = new Command(async () => await Cancelar(), () => !IsBusy);
            GuardarAlarmaCommand = new Command(async () => await GuardarAlarma(), () => !IsBusy);
        }

        public AlarmaViewModel(INavigation mainPageNav, ref ObservableCollection<Alarma> datosAlarmas)
        {
            _Navigation = mainPageNav;
            DatosAlarmas = datosAlarmas;
            AlarmaModel = new Alarma();

            InicializarCommands();
        }

        public AlarmaViewModel(INavigation mainPageNav, ref ObservableCollection<Alarma> datosAlarmas, Alarma alarma)
        {
            _Navigation = mainPageNav;
            DatosAlarmas = datosAlarmas;
            AlarmaModel = alarma;

            InicializarCommands();
        }


        private async Task GuardarAlarma()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return;
                }

                if (AlarmaModel != null && AlarmaModel.ID > 0)
                {
                    Console.WriteLine("5- Editar alarma " + AlarmaModel.ID.ToString());

                    if (!string.IsNullOrEmpty(AlarmaModel.Titulo))
                    {
                        var hc = new HttpClient()
                        {
                            BaseAddress = new Uri("http://apphacienda.sea.co.cr/API/Alarmas")
                        };

                        AlarmaModel.Hora = AlarmaModel.HoraTimeSpan.Ticks;
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(AlarmaModel), Encoding.UTF8, "application/json");

                        var alarmasAPI = hc.PutAsync("Alarmas", content);
                        alarmasAPI.Wait();

                        var resultAPI = alarmasAPI.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            //DatosAlarmas.Add(AlarmaModel);
                            await _Navigation.PopToRootAsync();
                        }
                        else
                        {
                            Console.WriteLine("4- Ocurrió un error al editar la alarma!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("4- Debe de agregar un título");
                    }
                }
                else
                {
                    Console.WriteLine("4- Crear alarma.");
                    if (!string.IsNullOrEmpty(AlarmaModel.Titulo))
                    {
                        var hc = new HttpClient()
                        {
                            BaseAddress = new Uri("http://apphacienda.sea.co.cr/API/Alarmas")
                        };

                        AlarmaModel.Hora = AlarmaModel.HoraTimeSpan.Ticks;
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(AlarmaModel), Encoding.UTF8, "application/json");

                        var alarmasAPI = hc.PostAsync("Alarmas", content);
                        alarmasAPI.Wait();

                        var resultAPI = alarmasAPI.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            //DatosAlarmas.Add(AlarmaModel);
                            await _Navigation.PopToRootAsync();
                        }
                        else
                        {
                            Console.WriteLine("4- Ocurrió un error al crear la alarma!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("4- Debe de agregar un título");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("-- ERROR: " + ex.Message);
            }
        }


        private async Task Cancelar()
        {
            await _Navigation.PopToRootAsync();
        }


    }
}
