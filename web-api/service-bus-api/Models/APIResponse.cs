
namespace service_bus_api.Models
{
    using System;

    public class APIResponse
    {
        public bool Success { get; set; }
        public dynamic Result { get; set; }

        public APIResponse()
        {
            this.Success = true;
        }

        public APIResponse(Exception exception)
        {
            this.Success = false;
        }
    }
}
