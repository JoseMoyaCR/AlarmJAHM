using AlarmJAHM.Models;
using AlarmJAHM.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlarmJAHM
{
    public partial class App : Application
    {
        Data.SQLiteDb Database { get => DependencyService.Get<Data.SQLiteDb>(); }

        public App()
        {
            InitializeComponent();

            

            MainPage = new NavigationPage(new AlarmasManagerPage());
        }

        protected override async void OnStart()
        {
            await Database.IncludeAsync<Alarma>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
