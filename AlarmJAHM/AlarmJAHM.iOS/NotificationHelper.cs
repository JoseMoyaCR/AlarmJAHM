using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using AlarmJAHM.iOS;
using UIKit;
using Xamarin.Forms;
using AlarmJAHM.Models;

[assembly: Dependency(typeof(NotificationHelper))]
namespace AlarmJAHM.iOS
{
    class NotificationHelper : INotification
    {
        public void CreateNotification(string title, string message)
        {
            new NotificationDelegate().RegisterNotification(title, message);
        }
    }
}