
namespace service_bus_api.Models
{
    using Newtonsoft.Json.Linq;
    using System.Dynamic;

    public class APIRequest
    {
        public string ConnectionString { get; set; }
        public ExpandoObject Model { get; set; }

        public APIRequest(JObject request)
        {
            this.Model = (null != request && null != request["Model"]) ? request["Model"].ToObject<ExpandoObject>() : null;
        }
    }
}
