using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Services;

// TODO: Add logger

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IDataAccess<AccountModel> dataAccess;
    private readonly ILogger<AccountController> logger;

    public AccountController(IDataAccess<AccountModel> dataAccess, ILogger<AccountController> logger)
    {
        this.logger = logger;
        this.dataAccess = dataAccess;
    }

    /// <summary>
    /// get all accounts (decryption applied)
    /// </summary>
    /// <returns></returns>
    [HttpGet, Authorize]
    public async Task<IResult> Get()
    {
        return await dataAccess.Get();
    }

    /// <summary>
    /// get an account with its decrypted password
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IResult> Get(string id)
    {
        return await dataAccess.Get(id);
    }

    /// <summary>
    /// add an account (updated password field will be encrypted)
    /// </summary>
    /// <param name="accountModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResult> Post([FromBody] AccountModel accountModel)
    {
        return await dataAccess.Post(accountModel);
    }

    /// <summary>
    /// add multiple accounts in one request (updated password field will be encrypted)
    /// </summary>
    /// <param name="accountModel"></param>
    /// <returns></returns>
    [HttpPost("many")]
    public async Task<IResult> PostMany([FromBody] List<AccountModel> accountModels)
    {
        return await dataAccess.PostMany(accountModels);
    }

    /// <summary>
    /// update an account (password field will be re-encrypted)
    /// </summary>
    /// <param name="accountModel"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IResult> Put([FromBody] AccountModel accountModel)
    {
        return await dataAccess.Put(accountModel);
    }

    /// <summary>
    /// remove an account from the database with the provided id parameter
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
        return await dataAccess.Delete(id);
    }

}
