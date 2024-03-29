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

        /// <summary>
        /// Challenges the current user, if not authenticated redirect to Google's authentication page
        /// </summary>
        /// <returns></returns>
        [HttpGet("google/google-link")]
        public IResult GoogleLink()
        {
            return Results.Challenge(
                properties: new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(GetToken))
                },
                authenticationSchemes: new List<string> { GoogleDefaults.AuthenticationScheme }
                );
        }

        /// <summary>
        /// Endpoint used to exchange the google generated cookie for a pair of JWT Access and refresh token
        /// This endpoint is used to restrict accepted authentication schemas to only jwt bearer
        /// [NOTE] Try not to get too coupled to this, as it should be removed at a given time
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "google-token-exchange")]
        [HttpGet("google/exchange")]
        public async Task<IActionResult> GetToken()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return Ok(await _authenticationService.AuthenticateAsync(user.Value));
        }
    }
}
