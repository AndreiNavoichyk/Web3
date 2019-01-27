using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web3.Api.Addresses;
using Web3.Api.TokenHoldings.V1.Dtos;
using Web3.Core.TokenHoldings;
using Web3.Core.TokenHoldings.Models;
using Web3.Core.Utils;
using Web3.Infra.Repositories;
using Web3.Infra.Repositories.Exceptions;

namespace Web3.Api.TokenHoldings.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/token-holdings")]
    [Produces("application/json")]
    public class BalancesController
    {
        private readonly ILogger<BalancesController> _logger;
        private readonly IAddressValidator _addressValidator;
        private readonly TokenHoldingsSettings _tokenHoldingsSettings;
        private readonly ITokensProvider _tokensProvider;
        private readonly IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> _tokenHoldingsRepository;

        public BalancesController(
            ILogger<BalancesController> logger,
            IAddressValidator addressValidator,
            TokenHoldingsSettings tokenHoldingsSettings,
            ITokensProvider tokensProvider,
            IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)> tokenHoldingsRepository)
        {
            _logger = logger;
            _addressValidator = addressValidator;
            _tokenHoldingsSettings = tokenHoldingsSettings;
            _tokensProvider = tokensProvider;
            _tokenHoldingsRepository = tokenHoldingsRepository;
        }

        /// <summary>
        /// Gets token holdings info.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The token holdings info for top ERC-20 tokens for the provided address</returns>
        /// <response code="200">Returns token holdings info for top ERC-20 tokens for the provided address</response>
        /// <response code="400">If the address is incorrect</response>
        /// <response code="404">If the error occured while token holdings info was getting</response>
        /// <response code="500">If some error occured while token holdings info was getting</response>
        [HttpGet("{id}")]
        [ActionName("get")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(string id)
        {
            if (_addressValidator.Validate(id))
            {
                try
                {
                    return new OkObjectResult(await GetTokenHoldingsAsync(id));
                }
                catch (Exception e) when (e is RepositoryException)
                {
                    _logger.LogError($"Can not get token holdings info for address:{id}", e);

                    return new NotFoundResult();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Some error occured during token holdings info getting for address:{id}", e);

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _logger.LogInformation($"Provided address:{id} is invalid");

                return new BadRequestResult();
            }

        }

        private async Task<TokenHoldingsDto> GetTokenHoldingsAsync(string address)
        {
            _logger.LogTrace($"Getting the top tokens list");
            
            var tokens = await _tokensProvider.GetAsync(_tokenHoldingsSettings.TopTokensNumber);

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
                Title = tokenInfo.Title
            };
            var tokenHoldingInfo = await _tokenHoldingsRepository.GetAsync((address: address, tokenAddress: tokenInfo.Address));
            tokenHoldingInfoDto.Value = tokenHoldingInfo.Value;
            return tokenHoldingInfoDto;
        }
    }
}