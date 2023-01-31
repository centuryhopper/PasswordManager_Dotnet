namespace PasswordManager.Services;

public interface IAuthenticationService<T>
{
    Task<IResult> Login(T model);
    Task<IResult> Register(T model);
    Task<int> Commit();
}
