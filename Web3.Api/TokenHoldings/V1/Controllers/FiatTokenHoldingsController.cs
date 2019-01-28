using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Api.TokenHoldings.V1.Dtos;
using Web3.Core.Services.Exchange;
using Web3.Core.TokenHoldings.Models;
using Web3.Core.Utils;
using Web3.Infra.Exceptions;
using Web3.Infra.Repositories;

namespace Web3.Api.TokenHoldings.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/fiat-token-holdings")]
    [Produces("application/json")]
    public class FiatTokenHoldingsController : ControllerBase
    {
        private readonly ILogger<FiatTokenHoldingsController> _logger;
        private readonly IAddressValidator _addressValidator;
        private readonly IQueryableRepository<TokenInfo, string> _tokenInfosRepository;
        private readonly IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> _tokenHoldingsRepository;
        private readonly IExchangeService _exchangeService;

        public FiatTokenHoldingsController(
            ILogger<FiatTokenHoldingsController> logger,
            IAddressValidator addressValidator,
            IQueryableRepository<TokenInfo, string> tokenInfosRepository,
            IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> tokenHoldingsRepository,
            IExchangeService exchangeService)
        {
            _logger = logger;
            _addressValidator = addressValidator;
            _tokenInfosRepository = tokenInfosRepository;
            _tokenHoldingsRepository = tokenHoldingsRepository;
            _exchangeService = exchangeService;
        }
        
        /// <summary>
        /// Gets fiat token holdings info.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tokenId"></param>
        /// <param name="currencySymbol"></param>
        /// <returns>The fiat token holdings info for the provided address provided ERC-20 token and currency symbol</returns>
        /// <response code="200">Returns fiat token holdings info for the provided address provided ERC-20 token and currency symbol</response>
        /// <response code="400">If the address is incorrect</response>
        /// <response code="404">If the error occured while fiat token holdings info was getting</response>
        /// <response code="500">If some error occured while fiat token holdings info was getting</response>
        [HttpGet("{id}")]
        [ActionName("get")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(
            string id = "0x4E9ce36E442e55EcD9025B9a6E0D88485d628A67",
            string tokenId = "0xB8c77482e45F1F44dE1745F52C74426C631bDD52",
            string currencySymbol = "USD")
        {
            if (!_addressValidator.Validate(id))
            {
                _logger.LogInformation($"Provided address:{id} is invalid");

                return new BadRequestResult();
            }

            if (!_addressValidator.Validate(tokenId))
            {
                _logger.LogInformation($"Provided token address:{tokenId} is invalid");

                return new BadRequestResult();
            }

            try
            {
                return new OkObjectResult(await GetFiatTokenHoldingsInternalAsync(id, tokenId, currencySymbol));
            }
            catch (AppException ex)
            {
                _logger.LogError($"Can not get fiat token holdings for address:{id}, tokenAddress:{tokenId} and currency:{currencySymbol}", ex);

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Some error occured during fiat token holdings getting for address:{id}, tokenAddress:{tokenId} and currency:{currencySymbol}", ex);
                
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<FiatTokenHoldingsDto> GetFiatTokenHoldingsInternalAsync(
            string address,
            string tokenAddress,
            string currencySymbol)
        {
            var tokenInfoTask = _tokenInfosRepository.GetAsync(tokenAddress);
            var tokenHoldingsInfoTask = _tokenHoldingsRepository.GetAsync((address, tokenAddress));

            await Task.WhenAll(tokenInfoTask, tokenHoldingsInfoTask);

            var tokenInfo = await tokenInfoTask;
            var tokenHoldingsInfo = await tokenHoldingsInfoTask;

            var exchangeResult = await _exchangeService.ExchangeAsync(
                new ExchangeValue {Symbol = tokenInfo.Symbol, Value = tokenHoldingsInfo.Value}, currencySymbol);

            var fiatTokenHoldings = new FiatTokenHoldingsDto
            {
                Address = address,
                Token = new TokenHoldingInfoDto
                {
                    Address = tokenAddress,
                    Title = tokenInfo.Title,
                    Symbol = tokenInfo.Symbol,
                    Value = tokenHoldingsInfo.Value
                },
                Fiat = new FiatDto
                {
                    Symbol = currencySymbol,
                    Value = exchangeResult.To.Value
                }
            };
            return fiatTokenHoldings;
        }
    }
}