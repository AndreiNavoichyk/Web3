using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Core.Repositories;

namespace Web3.Api.Balances.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BalancesController : ControllerBase
    {
        private readonly ILogger<BalancesController> _logger;
        private readonly IBalancesRepository _repository;

        public BalancesController(
            ILogger<BalancesController> logger,
            IBalancesRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Get(string address)
        {
            _logger.LogTrace($"Getting balance for address:{address}");

            var balance = await _repository.GetAsync(address);

            _logger.LogInformation($"Got balance for address:{address}");

            return new JsonResult(balance);
        }
    }
}