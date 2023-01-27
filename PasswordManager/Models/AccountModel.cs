
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models;

public class AccountModel
{
    [Key]
    public string? id {get; set;}

    [Required, StringLength(32)]
    public string? title {get; set;}

    [Required, StringLength(32)]
    public string? username {get; set;}

    [Required, StringLength(512)]
    public string? password {get; set;}
    public string? aesKey { get; set; }
    public string? aesIV { get; set; }
    public string? insertedDateTime {get; set;}
    public string? lastModifiedDateTime {get; set;}


    public override string ToString()
    {
        return $"id: {id}, title: {title}, username: {username}, password: {password}, insertedDateTime: {insertedDateTime}, lastModifiedDateTime: {lastModifiedDateTime}";
    }
}

