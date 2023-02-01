using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PasswordManager.Data;
using PasswordManager.Models;
using PasswordManager.Utils;

namespace PasswordManager.Services;

public class AuthenticationService : IAuthenticationService<UserModel>
{
    private readonly PasswordDbContext db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> logger;

    public AuthenticationService(PasswordDbContext db, IConfiguration configuration, ILogger<AuthenticationService> logger)
    {
        this.logger = logger;
        this.db = db;
        this._configuration = configuration;
    }

    public async Task<int> Commit()
    {
        return await db.SaveChangesAsync();
    }

    public async Task<IResult> Login(UserModel argModel)
    {
        try
        {
            var model = await db.UserTableEF.FirstOrDefaultAsync(m => m.username == argModel.username);

            if (model is null)
            {
                throw new Exception("incorrect username or password");
            }

            // decrypt password from model and compare with argModel's
            if (argModel.password != decryptPassword(model))
            {
                throw new Exception("incorrect username or password");
            }

            // check if token has expired. If so then generate a new one. Otherwise keep it
            DateTime tokenExpires = DateTime.Parse(model.tokenExpires!);

            // logger.LogWarning($"{model}");
            // logger.LogWarning($"{DateTime.Now}");
            // logger.LogWarning($"{tokenExpires}");

            if (DateTime.Compare(DateTime.Now, tokenExpires) > 0)
            {
                var (jwt, created, expires) = createJwtToken(model);
                model.currentJwtToken = jwt;
                model.tokenCreated = created.ToString();
                model.tokenExpires = expires.ToString();

                await Commit();
            }

            return Results.Ok($"login success! token: {model.currentJwtToken}");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> Register(UserModel argModel)
    {
        try
        {
            var userLst = await db.UserTableEF.ToListAsync();
            int numUsers = userLst.Count;
            System.Console.WriteLine($"# of users: {numUsers}");
            System.Console.WriteLine($"{userLst.FirstOrDefault()?.aesIV}");

            // make sure user is not in the database already

            // userLst.ForEach(Console.WriteLine);

            if (numUsers > 0)
            {
                var model = await db.UserTableEF.FirstOrDefaultAsync(user =>
                     user.username == argModel.username);

                if (model is not null)
                {
                    throw new Exception("This user already exists. Please try a different username.");
                }
            }

            argModel.userId = Guid.NewGuid().ToString();
            using (Aes myAes = Aes.Create())
            {
                byte[] encrypted = SymmetricEncryptionHandler.EncryptStringToBytes_Aes(argModel.password!, myAes.Key, myAes.IV);

                argModel.password = Convert.ToBase64String(encrypted);
                argModel.aesKey = Convert.ToBase64String(myAes.Key);
                argModel.aesIV = Convert.ToBase64String(myAes.IV);
            }

            var (jwt, created, expires) = createJwtToken(argModel);
            argModel.currentJwtToken = jwt;
            argModel.tokenCreated = created.ToString();
            argModel.tokenExpires = expires.ToString();

            await db.UserTableEF.AddAsync(argModel);
            await Commit();

            return Results.Ok(jwt);
        }
        catch (System.Exception e)
        {
            logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }

    // TODO: be able to delete users
    // WARN them that all theire passwords would be gone as well!

    private string decryptPassword(UserModel user)
    {
        return SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(Convert.FromBase64String(user.password!), Convert.FromBase64String(user.aesKey!), Convert.FromBase64String(user.aesIV!));
    }

    private (string, DateTime, DateTime) createJwtToken(UserModel pwm)
    {
        var now = DateTime.Now;
        var expires = now.AddDays(1);

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, pwm.username!),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Sub, pwm.userId!),
            new Claim(JwtRegisteredClaimNames.Exp, expires.ToString()),
        };

        var signingCreds = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!)), SecurityAlgorithms.HmacSha512Signature);


        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: signingCreds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwt, now, expires);
    }



}