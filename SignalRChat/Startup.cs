using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]
namespace SignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here

            //En attente pour l'instant
            //The hubConfiguration bit is only needed if I want to change the default settings
            var hubConfiguration = new HubConfiguration;
            //{
            //    EnableDetailedErrors = true,
            //    EnableJavaScriptProxies = false
            //};

            //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}