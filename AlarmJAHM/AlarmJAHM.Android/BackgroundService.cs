using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace AlarmJAHM.Droid
{
    [Service(Label = "BackgroundService")]
    public class BackgroundService : Service
    {
        private int _Counter = 0;
        private bool _IsRunningTimer = true;


        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {

                MessagingCenter.Send<string>(_Counter.ToString(), "horaActual");
                _Counter += 1;

                return _IsRunningTimer;
            });

            return StartCommandResult.Sticky;
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


        public override void OnDestroy()
        {
            StopSelf();
            _Counter = 0;
            _IsRunningTimer = false;
            base.OnDestroy();
        }
    }
}