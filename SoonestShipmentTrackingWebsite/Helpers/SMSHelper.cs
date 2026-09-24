using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class SMSHelper
    {
        private static string _apiKey;
        private static string _senderName;

        public static async Task<string> SendDeliveryOnTheWaySMS(string contactNumber, string customerName, string shipmentControlNo)
        {
            string apiToken =
                ConfigurationManager.AppSettings["PhilSmsApiToken"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Authorization",
                    "Bearer " + apiToken);

                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Accept",
                    "application/json");

                var requestData = new
                {
                    recipient = contactNumber,
                    sender_id = "PhilSMS",
                    type = "plain",
                    message = $"Hello {customerName}! Your package {shipmentControlNo} is out for delivery today!"
                };

                string json =
                    JsonConvert.SerializeObject(requestData);

                using (var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"))
                {
                    var response = await client.PostAsync(
                        "https://dashboard.philsms.com/api/v3/sms/send",
                        content);

                    string responseBody =
                        await response.Content.ReadAsStringAsync();

                    return $"HTTP {(int)response.StatusCode}: {responseBody}";
                }
            }
        }
    }
}