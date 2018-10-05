
namespace Nitor.OSS.WebAPI
{
    using Nitor.OSS.Logger;
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Function to handle application begin request event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //// Handle preflight request in CORS calls
            if (string.Equals(Request.HttpMethod, "OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                //// End response, this will return 200 OK status for preflight requests
                Response.End();
            }
        }

        /// <summary>
        /// Function to handle application error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception exception = Server.GetLastError();
                Logger.LogException(exception);
                Response.End();
            }
            catch (InsufficientMemoryException exception)
            {
                Logger.LogException(exception);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}
