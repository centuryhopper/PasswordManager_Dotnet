


namespace PasswordManager.Services;

public interface IEFService<T>
{
    Task<IResult> PostData(T model);
    Task<T?> GetById(string id);
    Task<IResult> GetByTitle(string title);
    Task<IResult> Update(T model);
    Task<IResult> Delete(string id);
    Task<IEnumerable<T>> GetAll();
    Task<int> Commit();
}
