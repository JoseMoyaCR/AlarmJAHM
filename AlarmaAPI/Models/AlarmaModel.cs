using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlarmaAPI.Models
{
    public class AlarmaModel
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Titulo")]
        public string Titulo { get; set; }

        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("Hora")]
        public long? Hora { get; set; }

        [JsonProperty("HoraTimeSpan")]
        public TimeSpan HoraTimeSpan { get; set; }

        [JsonProperty("Notificado")]
        public bool Notificado { get; set; }
    }
}