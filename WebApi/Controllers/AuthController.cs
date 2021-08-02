using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public class AuthController : ControllerBase
    {
        private readonly string _keyCloakBaseUrl = Environment.GetEnvironmentVariable("KEYCLOAK_BASE_URL") ??
                                                   throw new InvalidOperationException("KEYCLOAK_BASE_URL Not Found!");

        private readonly string _keyCloakPassword = Environment.GetEnvironmentVariable("KEYCLOAK_PASSWORD") ??
                                                    throw new InvalidOperationException("KEYCLOAK_PASSWORD Not Found!");

        private readonly string _keyCloakUsername = Environment.GetEnvironmentVariable("KEYCLOAK_USERNAME") ??
                                                    throw new InvalidOperationException("KEYCLOAK_USERNAME Not Found!");

        private readonly IRestClient _restClient;

        public AuthController(IRestClient restClient)
        {
            _restClient = restClient;

            _restClient.BaseUrl = new Uri($"{_keyCloakBaseUrl}/auth/realms/master/protocol/openid-connect/token");
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthCredentials authCredentials)
        {
            var request = new RestRequest(Method.POST)
                .AddHeader("content-type", "application/x-www-form-urlencoded")
                .AddParameter("client_id", authCredentials.ClientId)
                .AddParameter("client_secret", authCredentials.ClientSecret)
                .AddParameter("username", _keyCloakUsername)
                .AddParameter("password", _keyCloakPassword)
                .AddParameter("scope", "openid profile")
                .AddParameter("grant_type", "password");

            var response = _restClient.Execute(request);

            if (!response.IsSuccessful)
                return BadRequest(AuthResponse.FromJson(response.Content));

            return Ok(AuthResponse.FromJson(response.Content));
        }
    }
}