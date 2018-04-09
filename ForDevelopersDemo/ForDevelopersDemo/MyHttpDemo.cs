
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ForDevelopersDemo
{
    public static class MyHttpDemo
    {
        [FunctionName("MyHttpDemo")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "saveMessage")]HttpRequest req,
            [Queue("demomessages", Connection = "queueConn")] ICollector<string> outputMessage, TraceWriter log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            Person person = JsonConvert.DeserializeObject<Person>(requestBody);
            if (person == null || person.FirstName == null || person.LastName == null)
            {
                return new BadRequestObjectResult("Prz222ekaz poprawne dane osobowe");
            }




            string message = person.FirstName + " " + person.LastName;
            outputMessage.Add(message);
            return new OkObjectResult($"Zapisano wiadomosc: {message}");
        }
    }

}
