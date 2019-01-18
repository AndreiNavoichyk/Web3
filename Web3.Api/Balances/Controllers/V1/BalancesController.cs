using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Core.Models;
using Web3.Core.Repositories;

namespace Web3.Api.Balances.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BalancesController : ControllerBase
    {
        private readonly ILogger<BalancesController> _logger;
        private readonly IAddressValidator _addressValidator;
        private readonly IBalancesRepository _repository;

        public BalancesController(
            ILogger<BalancesController> logger,
            IAddressValidator addressValidator,
            IBalancesRepository repository)
        {
            _logger = logger;
            _addressValidator = addressValidator;
            _repository = repository;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Get(string address)
        {
            if (_addressValidator.Validate(address))
            {
                try
                {
                    return new OkObjectResult(await GetBalance(address));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Can not get balance for address:{address}", ex);

                    return new NotFoundResult();
                }
            }
            else
            {
                _logger.LogInformation($"Provided address:{address} is incorrect");

                return BadRequest();
            }
        }

        private async Task<Balance> GetBalance(string address)
        {
            _logger.LogTrace($"Getting balance for address:{address}");

            var balance = await _repository.GetAsync(address);

            _logger.LogInformation($"Got balance for address:{address}");

            return balance;
        }
    }
}