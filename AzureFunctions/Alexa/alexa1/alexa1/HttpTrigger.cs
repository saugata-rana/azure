using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using Newtonsoft.Json.Linq;

namespace AlexaCalculatorSkill
{
    public static class Calculator
    {
        [FunctionName("Calculator")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            switch (data.request.type.ToString())
            {
                case "IntentRequest":
                    var n1 = Convert.ToDouble(data.request.intent.slots["firstnum"].value.ToString());
                    var n2 = Convert.ToDouble(data.request.intent.slots["secondnum"].value.ToString());
                    var result = 0d;
                    string intentName = data.request.intent.name;
                    log.Warning($"intentName={intentName}");
                    switch (intentName)
                    {
                        case "AddIntent":
                            result = n1 + n2;
                            return CreateResponse(req, "The answer is:", $"The answer is {result.ToString()}");
                        case "SubtractIntent":
                            result = n1 - n2;
                            return CreateResponse(req, "The answer is:", $"The answer is {result.ToString()}");
                        default:
                            return CreateResponse(req, "Oops!", "Sorry, I couldn't understand the question.");
                    }
                case "LaunchRequest":
                default:
                    log.Info($"Default LaunchRequest made");
                    return CreateResponse(req, "Welcome to the Alexa Calculator", "Try asking: what is 3 plus 4?");
            }
        }

        private static HttpResponseMessage CreateResponse(HttpRequestMessage req, string title, string text)
        {
                string responseContent = "{version: 1.0,\n"+
                "\tsessionAttributes: {},\n"+
                "\t\tresponse: {\n"+
                "\t\t\toutputSpeech: {\n"+
                "\t\t\t\ttype: PlainText,\n"+
                "\t\t\t\ttext:"+ text + ",\n"+
                "\t\t\t},\n"+
                "\t\t\tcard: {\n"+
                "\t\t\t\ttype: Simple,\n"+
                "\t\t\t\ttitle:"+ title +",\n"+
                "\t\t\t\tcontent:"+ text  +",\n"+
                "\t\t\t}\n"+
                "\t\t},\n"+
                "\t\tshouldEndSession: true\n"+
                "\t}\n";
            
            //JObject responseContent1 =
                //new JObject(
                //    new JProperty("version", "1.0"),
                //    new JObject(
                //        new JProperty("sessionAttributes"),
                //        new JObject(
                //        new JProperty("response"),
                //            new JObject(
                //                new JProperty("outputSpeech"),
                //                new JObject(
                //                    new JProperty("type", "PlainText"),
                //                    new JProperty("text", text)
                //                ),
                //                new JProperty("card"),
                //                new JObject(
                //                    new JProperty("type", "Simple"),
                //                    new JProperty("title", title),
                //                    new JProperty("content", text)
                //                )
                //            ),
                //            new JProperty("shouldEndSession", "true")
                //        )
                //    )
                //);
                    
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) };
            //return req.CreateResponse(HttpStatusCode.OK, new
            //{
            //    version = "1.0",
            //    sessionAttributes = new { },
            //    response = new
            //    {
            //        outputSpeech = new
            //        {
            //            type = "PlainText",
            //            text = text
            //        },
            //        card = new
            //        {
            //            type = "Simple",
            //            title = title,
            //            content = text
            //        },
            //        shouldEndSession = false
            //    }
            //});
        }
    }
}

