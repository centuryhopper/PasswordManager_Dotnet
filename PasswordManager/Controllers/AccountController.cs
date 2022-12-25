using System.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using PasswordManager.Models;
using PasswordManager.Utils;
using PasswordManager.Services;

// TODO: Add error handling
// TODO: Add service object and move the low level logic to the service object class

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountPostgresService _postgresService;


    public AccountController(AccountPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    /// <summary>
    /// get all accounts (decryption applied)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IResult> Get()
    {
        return await _postgresService.Get();
    }

    /// <summary>
    /// get an account with its decrypted password
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IResult> Get(string id)
    {
        return await _postgresService.Get(id);
    }

    /// <summary>
    /// add an account (updated password field will be encrypted)
    /// </summary>
    /// <param name="accountModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResult> Post([FromBody] AccountModel accountModel)
    {
        return await _postgresService.Post(accountModel);
    }

    /// <summary>
    /// update an account (password field will be re-encrypted)
    /// </summary>
    /// <param name="accountModel"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IResult> Put([FromBody] AccountModel accountModel)
    {
        return await _postgresService.Put(accountModel);
    }

    /// <summary>
    /// remove an account from the database with the provided id parameter
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
        return await _postgresService.Delete(id);
    }

}
