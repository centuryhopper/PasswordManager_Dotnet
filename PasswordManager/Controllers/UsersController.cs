
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Services;

// TODO: Add error handling

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAuthenticationService<UserModel> _authService;

    public UsersController(IAuthenticationService<UserModel> authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] UserModel pwm)
    {
        return await _authService.Register(pwm);
    }

    [HttpPost("login"), Authorize]
    public async Task<IResult> Login([FromBody] UserModel pwm)
    {
        // SetRefreshToken(GenerateRefreshToken(), pwm);
        return await _authService.Login(pwm);
    }

    // [HttpPost("refresh-token")]
    // public async Task<ActionResult<string>> RefreshToken(string id)
    // {
    //     var refreshToken = Request.Cookies["refreshToken"];

    //    // TODO: find user in the user_accounts table by id

    //     if (!user.RefreshToken.Equals(refreshToken))
    //     {
    //         return Unauthorized("Invalid Refresh Token.");
    //     }
    //     else if(user.TokenExpires < DateTime.Now)
    //     {
    //         return Unauthorized("Token expired.");
    //     }

    //     string token = CreateToken(user);
    //     var newRefreshToken = GenerateRefreshToken();
    //     SetRefreshToken(newRefreshToken);

    //     return Ok(token);
    // }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    private void SetRefreshToken(RefreshToken newRefreshToken, UserModel pwm)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };

        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        pwm.refreshToken = newRefreshToken.Token;
        pwm.tokenCreated = newRefreshToken.Created;
        pwm.tokenExpires = newRefreshToken.Expires;
    }

}