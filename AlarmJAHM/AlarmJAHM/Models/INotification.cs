using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmJAHM.Models
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
