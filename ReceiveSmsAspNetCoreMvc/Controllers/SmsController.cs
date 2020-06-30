using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Nexmo.Api.Messaging;
using ReceiveSmsAspNetCoreMvc.Hubs;
using System.IO;
using System.Threading.Tasks;

namespace ReceiveSmsAspNetCoreMvc.Controllers
{
    public class SmsController : Controller
    {
        /// <summary>
        /// Allows access to all browser clients subscribed through the /smsHub
        /// </summary>
        public IHubContext<SmsHub> HubContext { get; set; }

        public SmsController(IHubContext<SmsHub> hub)
        {
            HubContext = hub;
        }

        [HttpPost("/webhooks/inbound-sms")]
        public async Task<IActionResult> HandleInboundSms()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();
                var inbound = JsonConvert.DeserializeObject<InboundSms>(json);
                await HubContext.Clients.All.SendAsync("InboundSms", inbound.Msisdn, inbound.Text);
            }
            return NoContent();
        }
    }
}