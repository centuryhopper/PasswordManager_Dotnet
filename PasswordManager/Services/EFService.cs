using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Models;

namespace PasswordManager.Services;

public class EFService : IEFService<AccountModel>
{
    private readonly PasswordDbContext db;
    private readonly ILogger<EFService> logger;

    public EFService(PasswordDbContext db, ILogger<EFService> logger)
    {
        this.logger = logger;
        this.db = db;
    }

    public async Task<int> Commit()
    {
        return await db.SaveChangesAsync();
    }

    public async Task<IResult> Delete(string id)
    {
        try
        {
            var model = await GetById(id);
            if (model is null)
            {
                throw new Exception();
            }

            db.PasswordTableEF.Remove(model);

            await Commit();

            return Results.Ok(model);

        }
        catch (System.Exception e)
        {
            logger.LogError("Deletion failed :(. Couldn't find model.");
            return Results.BadRequest(e.Message);
        }

    }

    public async Task<IEnumerable<AccountModel>> GetAll()
    {
        return await Task.Run(() => (from acc in db.PasswordTableEF select acc).AsEnumerable());
    }

    public async Task<AccountModel?> GetById(string id)
    {
        try
        {
            // var model = await db.PasswordTableEF.FindAsync(id);

            var model = await db.PasswordTableEF.FromSqlInterpolated($"select * from {nameof(db.PasswordTableEF)} where id = {id}").FirstOrDefaultAsync();

            return model;
        }
        catch (System.Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<IResult> GetByTitle(string title)
    {
        try
        {
            var model = await db.PasswordTableEF.FromSqlInterpolated($"select * from {nameof(db.PasswordTableEF)} where title = {title}").FirstOrDefaultAsync();

            return Results.Ok(model);
        }
        catch (System.Exception e)
        {
            logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> PostData(AccountModel model)
    {
        try
        {
            db.Add(model);
            await Commit();
            return Results.Ok($"{model} was successfully added");
        }
        catch (System.Exception e)
        {
            logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> Update(AccountModel argModel)
    {
        try
        {
            var model = await (from m in db.PasswordTableEF where m.id == argModel.id select m).FirstOrDefaultAsync();

            if (model is null)
            {
                throw new Exception();
            }

            model.title = argModel.title;
            model.username = argModel.username;
            model.password = argModel.password;

            await Commit();

            return Results.Ok(model);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }



}