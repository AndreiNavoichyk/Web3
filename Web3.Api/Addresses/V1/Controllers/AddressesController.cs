using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Core.Addresses.Models;
using Web3.Core.Utils;
using Web3.Infra.Repositories;

namespace Web3.Api.Addresses.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IAddressValidator _addressValidator;
        private readonly IQueryableRepository<AddressInfo, string> _repository;

        public AddressesController(
            ILogger<AddressesController> logger,
            IAddressValidator addressValidator,
            IQueryableRepository<AddressInfo, string> repository)
        {
            _logger = logger;
            _addressValidator = addressValidator;
            _repository = repository;
        }

        /// <summary>
        /// Gets address info.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The info for the provided address</returns>
        /// <response code="200">Returns the info for the provided address</response>
        /// <response code="400">If the address is incorrect</response>
        /// <response code="404">If the error occured while info was getting</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            if (_addressValidator.Validate(id))
            {
                try
                {
                    return new OkObjectResult(await GetAddressInfo(id));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Can not get info for address:{id}", ex);

                    return new NotFoundResult();
                }
            }
            else
            {
                _logger.LogInformation($"Provided address:{id} is incorrect");

                return BadRequest();
            }
        }

        private async Task<AddressInfo> GetAddressInfo(string id)
        {
            _logger.LogTrace($"Getting info for address:{id}");

            var addressInfo = await _repository.GetAsync(id);

            _logger.LogInformation($"Got info for address:{id}");

            return addressInfo;
        }
    }
}