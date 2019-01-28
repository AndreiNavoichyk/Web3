using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Api.TokenHoldings.V1.Dtos;
using Web3.Core.TokenHoldings.Models;
using Web3.Core.Utils;
using Web3.Infra.Exceptions;
using Web3.Infra.Repositories;

namespace Web3.Api.TokenHoldings.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/token-holdings")]
    [Produces("application/json")]
    public sealed class BalancesController : ControllerBase
    {
        private readonly ILogger<BalancesController> _logger;
        private readonly IAddressValidator _addressValidator;
        private readonly TokenHoldingsSettings _tokenHoldingsSettings;
        private readonly IQueryableRepository<TokenInfo, string> _tokenInfosRepository;
        private readonly IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> _tokenHoldingsRepository;

        public BalancesController(
            ILogger<BalancesController> logger,
            IAddressValidator addressValidator,
            TokenHoldingsSettings tokenHoldingsSettings,
            IQueryableRepository<TokenInfo, string> tokenInfosRepository,
            IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> tokenHoldingsRepository)
        {
            _logger = logger;
            _addressValidator = addressValidator;
            _tokenHoldingsSettings = tokenHoldingsSettings;
            _tokenInfosRepository = tokenInfosRepository;
            _tokenHoldingsRepository = tokenHoldingsRepository;
        }

        /// <summary>
        /// Gets token holdings info.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>The token holdings info for top ERC-20 tokens for the provided address</returns>
        /// <response code="200">Returns token holdings info for top ERC-20 tokens for the provided address</response>
        /// <response code="400">If the address is incorrect</response>
        /// <response code="404">If the error occured while token holdings info was getting</response>
        /// <response code="500">If some error occured while token holdings info was getting</response>
        [HttpGet("{address}")]
        [ActionName("get")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(string address = "0x4E9ce36E442e55EcD9025B9a6E0D88485d628A67")
        {
            if (_addressValidator.Validate(address))
            {
                try
                {
                    return new OkObjectResult(await GetTokenHoldingsAsync(address));
                }
                catch (AppException ex)
                {
                    _logger.LogError($"Can not get token holdings info for address:{address}", ex);

                    return new NotFoundResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Some error occured during token holdings info getting for address:{address}", ex);

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _logger.LogInformation($"Provided address:{address} is invalid");

                return new BadRequestResult();
            }

        }

        private async Task<TokenHoldingsDto> GetTokenHoldingsAsync(string address)
        {
            _logger.LogTrace($"Getting the top tokens list");

            var tokens = await _tokenInfosRepository.GetAllAsync(
                new QueryRequest<TokenInfo>
                {
                    Take = _tokenHoldingsSettings.TopTokensNumber
                });

            _logger.LogTrace($"Getting token holdings info for address:{address}");
            
            var tasks = tokens.Select(token => GetTokenHoldingsInfoDtoAsync(address, token)).ToList();

            await Task.WhenAll(tasks);
            
            var tokenHoldingsDto = new TokenHoldingsDto
            {
                Address = address,
                Tokens = new List<TokenHoldingInfoDto>()
            };

            foreach (var task in tasks)
            {
                tokenHoldingsDto.Tokens.Add(await task);
            }

            return tokenHoldingsDto;
        }

        private async Task<TokenHoldingInfoDto> GetTokenHoldingsInfoDtoAsync(string address, TokenInfo tokenInfo)
        {
            var tokenHoldingInfoDto = new TokenHoldingInfoDto
            {
                Address = tokenInfo.Address,
                Title = tokenInfo.Title,
                Symbol = tokenInfo.Symbol
            };
            var tokenHoldingInfo = await _tokenHoldingsRepository.GetAsync((address: address, tokenAddress: tokenInfo.Address));
            tokenHoldingInfoDto.Value = tokenHoldingInfo.Value;
            return tokenHoldingInfoDto;
        }
    }
}