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
    private readonly ILogger<AuthenticationService> logger;

    public AuthenticationService(PasswordDbContext db, ILogger<AuthenticationService> logger)
    {
        this.logger = logger;
        this.db = db;
    }

    public async Task<int> Commit()
    {
        return await db.SaveChangesAsync();
    }

    public async Task<IResult> Login(UserModel argModel)
    {
        throw new NotImplementedException();
    }

    private string decryptPassword(UserModel user)
    {
        return SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(Convert.FromBase64String(user.password!), Convert.FromBase64String(user.aesKey!), Convert.FromBase64String(user.aesIV!));
    }

    public async Task<IResult> Register(UserModel argModel)
    {
        try
        {
            System.Console.WriteLine($"# of users: {db.UserTableEF.ToList().Count}");
            // make sure user is not in the database already

            int numUsers = db.UserTableEF.ToList().Count;

            if (numUsers > 0)
            {
                var model = await db.UserTableEF.Where(
                user =>
                     user.username == argModel.username && decryptPassword(user) == argModel.password
                ).FirstOrDefaultAsync();

                if (model is not null)
                {
                    throw new Exception("this user is already in the database");
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

            await db.UserTableEF.AddAsync(argModel);

            await Commit();

            return Results.Ok(argModel);
        }
        catch (System.Exception e)
        {
            logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }
}