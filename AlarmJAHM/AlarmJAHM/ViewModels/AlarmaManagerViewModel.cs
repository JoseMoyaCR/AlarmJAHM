using AlarmJAHM.Models;
using AlarmJAHM.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		Data.SQLiteDb Database { get => DependencyService.Get<Data.SQLiteDb>(); }

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


		private static Alarma _Cargando = new Alarma { ID = -1, Titulo = "Cargando..." };
		public ObservableCollection<Alarma> DatosAlarmas { get; } = new ObservableCollection<Alarma>(
            new Alarma[] { _Cargando }
		);


		public AlarmaManagerViewModel(INavigation mainPageNav) 
		{
			_Navigation = mainPageNav;
			DatosAlarmas = new ObservableCollection<Alarma>();

			// Inicializamos el comando para cargar datos
			RefrescarCommand = new Command(async () => {
				IsBusy = true;
				await ConsultarDatosAPI();
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

			await _Navigation.PushAsync(new FrmAlarmaPage());
		}


		private async Task EditarAlarma(object alarma)
		{
			if (alarma != null)
			{
				await _Navigation.PushAsync(new FrmAlarmaPage((Alarma)alarma));
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
				Console.WriteLine("3- Eliminando una alarma. " + alarmaModel.ID.ToString());

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
					// Limpiamos la caché
					//DatosAlarmas.Remove(alarmaModel);
					//await Database.DeleteAsync<Alarma>(alarmaModel);
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
			try
			{
				var localDataAlarmas = await Database.ListAsync<Alarma>();

				DatosAlarmas.Clear();

				// Verificamos si la caché tiene datos
				if (localDataAlarmas.Any())
				{
					//foreach (var alarma in localDataAlarmas)
					//{
					//	DatosAlarmas.Add(alarma);
					//}

					await ConsultarDatosAPI();
				}
				else
				{
					await ConsultarDatosAPI();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}


		private async Task ConsultarDatosAPI()
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


				// Limpiamos la BD
				//await Database.DeleteAllAsync<Alarma>();
				// Agregamos a la base de datos local - Cache
				//await Database.InsertAllAsync(DatosAlarmas.ToArray());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
