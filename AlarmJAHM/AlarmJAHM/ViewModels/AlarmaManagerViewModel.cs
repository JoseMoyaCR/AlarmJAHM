using AlarmJAHM.Models;
using AlarmJAHM.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;

namespace AlarmJAHM.ViewModels
{
    public class AlarmaManagerViewModel : BindableObject
    {
		private bool _IsBusy;

		INavigation _Navigation;

		public Command RefrescarCommand { get; set; }

		public Command AgregarAlarmaCommand { get; set; }

		public Command EditarAlarmaCommand { get; set; }

		public Command EliminarAlarmaCommand { get; set; }


		public bool IsBusy
		{
			get => _IsBusy;
			set
			{
				_IsBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}

		private ObservableCollection<Alarma> _DatosAlarmas;
		private Alarma _Cargando = new Alarma { ID = -1, Titulo = "Cargando..." };
		public ObservableCollection<Alarma> DatosAlarmas 
		{ 
			get => _DatosAlarmas;
			set
			{
				_DatosAlarmas = value;
			}
		}


		public AlarmaManagerViewModel(INavigation mainPageNav) 
		{
			_Navigation = mainPageNav;
			DatosAlarmas = new ObservableCollection<Alarma>();

			// Inicializamos el comando para cargar datos
			RefrescarCommand = new Command(async () => {
				IsBusy = true;
				await CargarDatos();
				IsBusy = false;
			});

			// Inicializamos los comandos
			AgregarAlarmaCommand = new Command(async () => await AgregarAlarma(), () => !IsBusy);
			EditarAlarmaCommand = new Command(async (idAlarma) => await EditarAlarma(idAlarma), (idAlarma) => !IsBusy);
			EliminarAlarmaCommand = new Command(async (idAlarma) => await EliminarAlarma(idAlarma), (idAlarma) => !IsBusy);
		}


		private async Task AgregarAlarma()
		{
			Console.WriteLine("1- Agregando una alarma.");

			await _Navigation.PushAsync(new FrmAlarmaPage(ref _DatosAlarmas));
		}


		private async Task EditarAlarma(object alarma)
		{
			if (alarma != null)
			{
				await _Navigation.PushAsync(new FrmAlarmaPage(ref _DatosAlarmas, (Alarma)alarma));
			}
			else
            {
				// Mostrar mensaje que hubo un problema
            }
		}

		private async Task EliminarAlarma(object alarma)
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return;
			}

			if (alarma != null)
			{
				Alarma alarmaModel = ((Alarma)alarma);
				Console.WriteLine("3- Eliminando una alarma." + alarmaModel.ID.ToString());

				var hc = new HttpClient()
				{
					BaseAddress = new Uri("http://apphacienda.sea.co.cr/API/Alarmas")
				};

				alarmaModel.Hora = alarmaModel.HoraTimeSpan.Ticks;
				HttpContent content = new StringContent(JsonConvert.SerializeObject(alarmaModel), Encoding.UTF8, "application/json");

				var alarmasAPI = hc.DeleteAsync("Alarmas?id=" + alarmaModel.ID);
				alarmasAPI.Wait();

				var resultAPI = alarmasAPI.Result;
				if (resultAPI.IsSuccessStatusCode)
				{
					await CargarDatos();
				}
				else
				{
					Console.WriteLine("4- Ocurrió un error al editar la alarma!!!");
				}
			}
			else
			{
				// Mostrar mensaje que hubo un problema
			}
		}


		public async Task CargarDatos()
		{
			// TODO: Checkear el internet
			if (Connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return;
			}


			try
			{
				DatosAlarmas.Clear();

				var http = new HttpClient()
				{
					BaseAddress = new Uri("http://apphacienda.sea.co.cr/API/Alarmas")
					//BaseAddress = new Uri("http://localhost:21417/API/Alarmas")
				};

				var alarmasAPI = await http.GetStringAsync("Alarmas");
				var data = JsonConvert.DeserializeObject<Alarma[]>(alarmasAPI);

				foreach (var alarma in data)
				{
					if (alarma.Hora != null)
					{
						alarma.HoraTimeSpan = new TimeSpan(alarma.Hora ?? 0);
					}
					
					DatosAlarmas.Add(alarma);
				}

				//var horaActual = DateTime.Now;
				//horaActual = horaActual.AddSeconds(-horaActual.Second).AddMilliseconds(-horaActual.Millisecond);

				//DatosAlarmas.Add(new Alarma { ID = 111, Titulo = "Alarma 1", Descripcion = "Desc 1", Hora = horaActual.AddMinutes(1).TimeOfDay });
				//DatosAlarmas.Add(new Alarma { ID = 222, Titulo = "Alarma 2", Descripcion = "Desc 22", Hora = horaActual.AddMinutes(2).TimeOfDay });
				//DatosAlarmas.Add(new Alarma { ID = 333, Titulo = "Alarma 3", Descripcion = "Desc 333", Hora = horaActual.AddMinutes(3).TimeOfDay });

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

	}
}
