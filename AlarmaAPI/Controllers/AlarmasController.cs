using System;
using System.Linq;
using System.Web.Http;
using AlarmaAPI.Models;

namespace AlarmaAPI.Controllers
{
    public class AlarmasController : ApiController
    {
        DBAlarmaDataContext _DBContext = new DBAlarmaDataContext();

        // GET: Alarmas
        public IHttpActionResult GetAlarmas()
        {
            
            var alarmas = _DBContext.Alarmas.ToList();
            return Ok(alarmas);
        }


        public IHttpActionResult GetAlarmas(int id)
        {
            try
            {
                var alarma = (from m in _DBContext.Alarmas where m.ID == id select m).FirstOrDefault();
                return Ok(alarma);
            }
            catch (Exception ex)
            {
                Bitacora.EscribirLog(ex.Message);
                return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable));
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] AlarmaModel alarma)
        {
            try
            {
                if (alarma != null)
                {
                    _DBContext.Alarmas.InsertOnSubmit(new Alarma { Titulo = alarma.Titulo, Descripcion = alarma.Descripcion, Hora = alarma.Hora });
                    _DBContext.SubmitChanges();
                }
                else
                {
                    return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent));
                }
            }
            catch (Exception ex)
            {
                Bitacora.EscribirLog(ex.Message);
                return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable));
            }
            
            return Ok();
        }


        public IHttpActionResult Put([FromBody] AlarmaModel alarma)
        {            
            try
            {
                var result = (from m in _DBContext.Alarmas where m.ID == alarma.ID select m).FirstOrDefault();

                if (result != null)
                {
                    result.Titulo = alarma.Titulo;
                    result.Descripcion = alarma.Descripcion;
                    result.Hora = alarma.Hora;
                    _DBContext.SubmitChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Bitacora.EscribirLog(ex.Message);
                return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable));
            }

            return Ok();
        }


        public IHttpActionResult Delete(int id)
        {
            try
            {
                var alarma = (from m in _DBContext.Alarmas where m.ID == id select m).FirstOrDefault();
                if (alarma != null)
                {
                    _DBContext.Alarmas.DeleteOnSubmit(alarma);
                    _DBContext.SubmitChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Bitacora.EscribirLog(ex.Message);
                return ResponseMessage(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable));
            }

            return Ok();
        }
    }
}