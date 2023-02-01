
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models;

public class UserModel
{
    [Key]
    public string? userId { get; set; }

    [Required, StringLength(32)]
    public string? username { get; set; }

    [Required, StringLength(512)]
    public string? password { get; set; }

    public string? aesKey { get; set; }

    public string? aesIV { get; set; }
    public List<AccountModel>? accounts { get; set; }
    public string currentJwtToken { get; set; } = string.Empty;
    public string? tokenCreated { get; set; }
    public string? tokenExpires { get; set; }

    public override string ToString()
    {
        return $"{nameof(username)}: {username}, {nameof(password)}: {password}, {nameof(aesKey)}: {aesKey}, {nameof(aesIV)}: {aesIV}, {nameof(currentJwtToken)}: {currentJwtToken}, {nameof(tokenCreated)}: {tokenCreated}, {nameof(tokenExpires)}: {tokenExpires}";
    }
}

