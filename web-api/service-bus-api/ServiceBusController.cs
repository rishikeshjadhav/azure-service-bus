
namespace service_bus_api
{
    using logger;
    using Newtonsoft.Json.Linq;
    using service_bus_api.Models;
    using service_bus_api.Services;
    using System;
    using System.Web.Http;

    [RoutePrefix("api/servicebus")]
    public class ServiceBusController : ApiController
    {
        [HttpGet, Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Ping successful");
        }

        [HttpPost, Route("create/queue")]
        public IHttpActionResult CreateQueue(JObject request)
        {
            APIRequest apiRequest = null;
            APIResponse apiResponse = null;
            try
            {
                apiRequest = new APIRequest(request);
                apiResponse.Result = new QueueService(apiRequest.ConnectionString).Create(apiRequest.Model);
                apiResponse = new APIResponse();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                apiResponse = new APIResponse(exception);
            }
            return Ok(apiResponse);
        }

        [HttpPost, Route("create/topic")]
        public IHttpActionResult CreateTopic(JObject request)
        {
            APIRequest apiRequest = null;
            APIResponse apiResponse = null;
            try
            {
                apiRequest = new APIRequest(request);
                apiResponse.Result = new TopicService(apiRequest.ConnectionString).Create(apiRequest.Model);
                apiResponse = new APIResponse();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                apiResponse = new APIResponse(exception);
            }
            return Ok(apiResponse);
        }

        [HttpPost, Route("create/subscription")]
        public IHttpActionResult CreateSubscription(JObject request)
        {
            APIRequest apiRequest = null;
            APIResponse apiResponse = null;
            try
            {
                apiRequest = new APIRequest(request);
                apiResponse.Result = new SubscriptionService(apiRequest.ConnectionString).Create(apiRequest.Model);
                apiResponse = new APIResponse();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                apiResponse = new APIResponse(exception);
            }
            return Ok(apiResponse);
        }
    }
}
