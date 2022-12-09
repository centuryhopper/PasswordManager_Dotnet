using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using PasswordManager.Models;

// TODO: Add error handling

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("all")]
    public IResult Get()
    {
        string query = @"
            select * from test_table;
        ";

        try
        {
            var table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);
            myReader = myCommand.ExecuteReader();

            table.Load(myReader);

            System.Console.WriteLine(query);

            return Results.Ok(JsonConvert.SerializeObject(table));
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IResult Post([FromBody] AccountModel accountModel)
    {
        // example:
        /*
            insert into test_table(id, title, user_name, password, inserteddatetime, lastmodifieddatetime)
            values (0, 'dummy_title', 'username', 'password', timestamp '2015-01-10 00:51:14', timestamp '2015-01-10 00:51:14');
        */

        string query = @"
            insert into test_table(id, title, user_name, password, inserteddatetime, lastmodifieddatetime)
            values (@id, @title, @username, @password, @inserteddatetime, @lastmodifieddatetime);
        ";

        try
        {
            string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);


            System.Console.WriteLine(accountModel);

            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, (object)accountModel.id ?? "placeholder");

            myCommand.Parameters.AddWithValue("@title", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.title ?? DBNull.Value);


            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.username ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.password ?? DBNull.Value);

            DateTime? myDate = null;

            if (accountModel.insertedDateTime != null)
                myDate = DateTime.ParseExact(accountModel.insertedDateTime, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            myCommand.Parameters.AddWithValue("@inserteddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            if (accountModel.lastModifiedDateTime != null)
                myDate = DateTime.ParseExact(accountModel.lastModifiedDateTime, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            myCommand.Parameters.AddWithValue("@lastmodifieddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            myReader = myCommand.ExecuteReader();

            return Results.Ok("post success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }


    [HttpPut]
    public IResult Put([FromBody] AccountModel accountModel)
    {
        string query = @"
            update test_table
            set title=@title, user_name=@username, password=@password, inserteddatetime=@inserteddatetime, lastmodifieddatetime=@lastmodifieddatetime
            where id=@id;
        ";

        try
        {
            string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);


            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.id ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@title", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.title ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.username ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.password ?? DBNull.Value);

            DateTime? myDate = null;

            if (accountModel.insertedDateTime != null)
                myDate = DateTime.ParseExact(accountModel.insertedDateTime, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            myCommand.Parameters.AddWithValue("@inserteddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            // reset to null since we reuse this variable for the next parameter
            myDate = null;

            if (accountModel.lastModifiedDateTime != null)
                myDate = DateTime.ParseExact(accountModel.lastModifiedDateTime, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            myCommand.Parameters.AddWithValue("@lastmodifieddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            System.Console.WriteLine(accountModel);
            myReader = myCommand.ExecuteReader();

            return Results.Ok("update success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }


    [HttpDelete("{id}")]
    public IResult Delete(string id)
    {
        string query = @"
            delete from test_table
            where id=@id";

        try
        {
            string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, id);

            myReader = myCommand.ExecuteReader();

            return Results.Ok("delete success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

}
