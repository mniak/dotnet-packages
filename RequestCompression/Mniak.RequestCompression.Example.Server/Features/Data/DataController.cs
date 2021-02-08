using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Mniak.RequestCompression.Example.Server.Features.Data
{
    [ApiController]
    [Route("data")]
    public class DataController : ControllerBase
    {
        private readonly ILogger logger;

        public DataController(ILogger<DataController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] IEnumerable<string> sentences)
        {
            logger.LogInformation("Received {Count} sentences. The first is '{FirstSentence}'", sentences.Count(), sentences.FirstOrDefault());
            return Ok(sentences);
        }
    }
}
