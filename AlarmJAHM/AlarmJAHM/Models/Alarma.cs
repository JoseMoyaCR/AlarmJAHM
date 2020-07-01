using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmJAHM.Models
{
    [Table("Alarmas")]
    public class Alarma
    {
        [PrimaryKey, Column("ID")]
        public int ID { get; set; }

        [Column("Titulo")]
        public string Titulo { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Hora")]
        public long? Hora { get; set; }

        [JsonIgnore]
        public TimeSpan HoraTimeSpan { get; set; }


        public bool Notificado { get; set; }  
    }
}
