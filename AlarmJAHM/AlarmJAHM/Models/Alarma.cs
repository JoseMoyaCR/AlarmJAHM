using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmJAHM.Models
{
    public class Alarma
    {
        public int ID { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public long? Hora { get; set; }

        [JsonIgnore]
        public TimeSpan HoraTimeSpan { get; set; }


        public bool Notificado { get; set; }  
    }
}
