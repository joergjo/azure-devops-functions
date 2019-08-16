using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gates
{
    public class RandomApproval
    {
        private readonly Random _random;

        public RandomApproval(Random random)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        [FunctionName(nameof(RandomApproval))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("RandomApproval triggered.");

            string requestBody;

            using (var reader = new StreamReader(request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            logger.LogInformation("Body: {Body}", requestBody);

            bool isApproved = (_random.Next(0, 2) == 1);
            logger.LogInformation("Gate approves: {Approval}.", isApproved);
            return isApproved ? new OkResult() : new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
        }
    }
}
