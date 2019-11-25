using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using FabsAFTrigBindSess.Utilities;

namespace FabsAFTrigBindSess.httpToCosmosDB
{
    public static class HttpToCosmosCRUD
    {
        [FunctionName("CreateAccolade")]
        public static async Task<IActionResult> CreateAccolade([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "accolade")] HttpRequest req,
            [CosmosDB(
            databaseName: "FunctWithFabsDb",
            collectionName: "Accolades",
            ConnectionStringSetting = "FunctWithFabsCosmosDBConn")] IAsyncCollector<ColleagueAccolade> createAccoladeOut,
            ILogger log)
        {
            log.LogInformation("Entry into the Create Function has occured...");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ColleagueAccolade input = JsonConvert.DeserializeObject<ColleagueAccolade>(requestBody);
            log.LogInformation($"Input Payload is: {input}");
            var acc = new ColleagueAccolade()
            {
                ColleagueName = input.ColleagueName,
                ColleagueUPNEmail = input.ColleagueUPNEmail,
                AccoladeStatement = input.AccoladeStatement,
                MyName = input.MyName,
                MyUPNEmail = input.MyUPNEmail,
                AccoladeLifeValidStatus = input.AccoladeLifeValidStatus,
                WorkBucksWorthy = input.WorkBucksWorthy,
                WorkBucksAmount = input.WorkBucksAmount,
                WorkBucksId = input.WorkBucksId,
                OfficeLocation = input.OfficeLocation
            };
            await createAccoladeOut.AddAsync(acc);

            log.LogInformation($"Session Record Added... Work is Finished");

            return acc != null
                ? (ActionResult)new OkObjectResult(acc)
                : new BadRequestObjectResult("Please pass a valid Accolade JSON Payload in the request body");
        }

        [FunctionName("GetAllAccolades")]
        public static IActionResult GetAllAccolades([HttpTrigger(AuthorizationLevel.Anonymous, "get",
                Route = "accolade")]HttpRequest req,
            [CosmosDB(
                databaseName: "FunctWithFabsDb",
                collectionName: "Accolades",
                ConnectionStringSetting = "FunctWithFabsCosmosDBConn",
                SqlQuery = "SELECT * FROM a order by a._ts desc")] IEnumerable<ColleagueAccolade> allAccolades,
            ILogger log)
            {
                log.LogInformation("Getting all Accolades that are Valid...");
                return new OkObjectResult(allAccolades);
            }
    }
}
