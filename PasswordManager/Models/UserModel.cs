
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

    public string? refreshToken { get; set; } = string.Empty;
    public DateTime? tokenCreated { get; set; }
    public DateTime? tokenExpires { get; set; }

    public override string ToString()
    {
        return $"username: {username}, password: {password}";
    }
}

