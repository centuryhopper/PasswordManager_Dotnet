
namespace PasswordManager.Models;

public class AccountModel
{
    public string id {get; set;}
    public string title {get; set;}
    public string username {get; set;}
    public string password {get; set;}
    public String insertedDateTime {get; set;}
    public string lastModifiedDateTime {get; set;}


    public override string ToString()
    {
        return $"id: {id}, title: {title}, username: {username}, password: {password}, insertedDateTime: {insertedDateTime}, lastModifiedDateTime: {lastModifiedDateTime}";
    }
}

