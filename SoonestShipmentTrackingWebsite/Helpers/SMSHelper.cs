using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class SMSHelper
    {
        private static string _apiKey;
        private static string _senderName;

        public static async Task<string> SendDeliveryOnTheWaySMS(string contactNumber, string customerName, string shipmentControlNo)
        {
            _apiKey = ConfigurationManager.AppSettings["SemaphoreApiKey"];
            _senderName = ConfigurationManager.AppSettings["SemaphoreSenderName"];

            using (var client = new HttpClient())
            {
                var data = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("apikey", _apiKey),
                        new KeyValuePair<string, string>("number", contactNumber),
                        new KeyValuePair<string, string>("message", $"Hello {customerName}! Your package {shipmentControlNo} is out for delivery today!"),
                        new KeyValuePair<string, string>("sendername", _senderName)
                    });

                var response = await client.PostAsync("https://api.semaphore.co/api/v4/messages", data);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}