using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Core.Addresses.Models;
using Web3.Core.Utils;
using Web3.Infra.Exceptions;
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
        /// <response code="500">If some error occured while info was getting</response>   
        [HttpGet("{id}")]
        [ActionName("get")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(string id = "0xDc45F6F4D6220bBA0a046AAF5cc1D1D086aCe4D0")
        {
            if (_addressValidator.Validate(id))
            {
                try
                {
                    return new OkObjectResult(await GetAddressInfo(id));
                }
                catch (AppException ex)
                {
                    _logger.LogError($"Can not get info for address:{id}", ex);

                    return new NotFoundResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Some error occured during address info getting for address:{id}", ex);
                    
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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