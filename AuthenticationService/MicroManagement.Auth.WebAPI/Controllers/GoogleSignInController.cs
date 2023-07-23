﻿using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MicroManagement.Auth.WebAPI.Services;

namespace MicroManagement.Auth.WebAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class GoogleSignInController : ControllerBase
    {
        private readonly IAuthService _authenticationService;

        public GoogleSignInController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("google/google-link")]
        public async Task<IResult> GoogleLink()
        {
            return Results.Challenge(
                properties: new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(GetToken))
                },
                authenticationSchemes: new List<string> { GoogleDefaults.AuthenticationScheme }
                );
        }

        // Try not to get too coupled to this, as it should be removed at a given time
        [Authorize(Policy = "google-token-exchange")]
        [HttpGet("google/exchange")]
        public async Task<IActionResult> GetToken()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return Ok(await _authenticationService.AuthenticateAsync(user.Value));
        }
    }
}
