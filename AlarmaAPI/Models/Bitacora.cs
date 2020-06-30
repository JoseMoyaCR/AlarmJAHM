using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AlarmaAPI.Models
{
    public class Bitacora
    {

        public static void EscribirLog(string clase, string metodo, string mensaje)
        {
            try
            {
                string fileName = @"C:\Logs\AppTestLogs\Log_"  + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }

                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine("");
                    sw.WriteLine("####################################################################################################################");
                    sw.WriteLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    sw.WriteLine("Clase: " + clase);
                    sw.WriteLine("Método: " + metodo);
                    sw.WriteLine("Mensaje: " + mensaje);
                    sw.WriteLine("####################################################################################################################");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }



        public static void EscribirLog(string mensaje)
        {
            try
            {
                string fileName = @"C:\Logs\AppTestLogs\Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }

                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine("");
                    sw.WriteLine("####################################################################################################################");
                    sw.WriteLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    sw.WriteLine("Mensaje: " + mensaje);
                    sw.WriteLine("####################################################################################################################");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}