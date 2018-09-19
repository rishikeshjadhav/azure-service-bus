
namespace service_bus_api
{
    using System.Web.Http;

    [RoutePrefix("api/servicebus")]
    public class ServiceBusController : ApiController
    {
        [HttpGet, Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Ping successful");
        }
    }
}
