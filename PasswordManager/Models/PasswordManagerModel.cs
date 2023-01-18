
namespace PasswordManager.Models;

public class PasswordManagerModel
{
    public string? id;
    public string? username {get; set;}
    public string? password {get; set;}
    public string? aesKey { get; set; }
    public string? aesIV { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

    public override string ToString()
    {
        return $"username: {username}, password: {password}";
    }
}

