
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Services;

// TODO: Add error handling

[Route("api/[controller]")]
[ApiController]
public class PasswordManagerController : ControllerBase
{
    private readonly PasswordManagerPostgresService _passwordManagerPostgresService;

    public PasswordManagerController(PasswordManagerPostgresService passwordManagerPostgresService)
    {
        _passwordManagerPostgresService = passwordManagerPostgresService;
    }

    [HttpPost("register")]
    public async Task<IResult> register([FromBody] PasswordManagerModel pwm)
    {
        return await _passwordManagerPostgresService.register(pwm);
    }


    [HttpPost("login")]
    public async Task<IResult> login([FromBody] PasswordManagerModel pwm)
    {
        return await _passwordManagerPostgresService.login(pwm);
    }



}