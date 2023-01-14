


using PasswordManager.Utils;
using System.Data;
using Npgsql;
using PasswordManager.Models;
using System.Security.Cryptography;

namespace PasswordManager.Services;

public class PasswordMangerPostgresService
{
    /*
        create table test_table (
            id varchar(100) PRIMARY KEY NOT NULL,
            username varchar(20),
            password varchar(512),
            insertteddatetime timestamp,
            lastmodifieddatetime timestamp,
            key varchar(512),
            iv varchar(512)
        );
    */

    private readonly IConfiguration _configuration;
    private readonly string postgresqlConnectionString;

    public PasswordMangerPostgresService(IConfiguration configuration)
    {
        _configuration = configuration;

        // ElephantSQL formatting
        var uriString = _configuration.GetConnectionString("cloudConnectionString")!;
        var uri = new Uri(uriString);
        var db = uri.AbsolutePath.Trim('/');
        var user = uri.UserInfo.Split(':')[0];
        var passwd = uri.UserInfo.Split(':')[1];
        var port = uri.Port > 0 ? uri.Port : 5432;
        var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
            uri.Host, db, user, passwd, port);
        postgresqlConnectionString = connStr;
    }

    public async Task<IResult> login(PasswordManagerModel pwm)
    {
        // query table to see if the the username and password pair exists in the database. If so the return success and the userId associated with that account (we can filter the password accounts table by this userId to display the proper accounts to the user). Otherwise we return failure to find account pair in the table

        string query = @"
            select id, username, password from user_accounts
            where username=@username;
        ";

        try
        {
            var myTable = new DataTable();
            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(postgresqlConnectionString);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            // configure params
            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)pwm.username ?? DBNull.Value);

            myReader = await myCommand.ExecuteReaderAsync();

            myTable.Load(myReader);

            if (myTable.Rows.Count == 0)
            {
                return Results.Ok("incorrect password");
            }

            string? username = null, password = null, key = null, iv = null, id = null;

            myTable.AsEnumerable().Select(
            row => myTable.Columns.Cast<DataColumn>().ToDictionary(
                column => column.ColumnName,
                column => row[column].ToString()
            )).ToList().ForEach(dict =>
            {
                foreach (var kvp in dict)
                {
                    // System.Console.WriteLine($"key: {kvp.Key}, value: {kvp.Value}");
                    switch (kvp.Key)
                    {
                        case "username":
                            username = kvp.Value;
                            break;
                        case "password":
                            password = kvp.Value;
                            break;
                        case "key":
                            key = kvp.Value;
                            break;
                        case "iv":
                            iv = kvp.Value;
                            break;
                        case "id":
                            id = kvp.Value;
                            break;
                    }
                }
            });

            password = SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(Convert.FromBase64String(password!), Convert.FromBase64String(key!), Convert.FromBase64String(iv!));

            if (pwm.password == password)
            {
                return Results.Ok($"login success! Link ID: {id}");
            }

            return Results.Ok("login failure :/");

        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }




    }

    public async Task<IResult> register(PasswordManagerModel pwm)
    {
        // abort if user already in the database. Otherwise add post a new user with the specified username and password into the database
        string query = @"
            select id, username, password from user_accounts
            where username=@username;
        ";

        try
        {
            var myTable = new DataTable();
            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(postgresqlConnectionString);
            myCon.Open();


            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            // configure params
            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?) pwm.username ?? DBNull.Value);

            myReader = await myCommand.ExecuteReaderAsync();

            myTable.Load(myReader);

            if (myTable.Rows.Count > 0)
            {
                return Results.Ok("An account with this username already exists");
            }

            string id = Guid.NewGuid().ToString();

            query = @"insert into user_accounts(id, username, password, key, iv, inserteddatetime, lastmodifieddatetime)
            values (@id, @username, @password, @key, @iv, @inserteddatetime, @lastmodifieddatetime)";

            using NpgsqlConnection myCon2 = new NpgsqlConnection(postgresqlConnectionString);
            myCon2.Open();
            using NpgsqlCommand myCommand2 = new NpgsqlCommand(query, myCon2);

            myCommand2.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, pwm.id ?? id);

            myCommand2.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?) pwm.username ?? DBNull.Value);

            DateTime myDate = DateTime.ParseExact(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture
            );

            myCommand2.Parameters.AddWithValue("@inserteddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?) myDate ?? DBNull.Value);

            myCommand2.Parameters.AddWithValue("@lastmodifieddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?) myDate ?? DBNull.Value);


            if (pwm.password != null)
            {
                using (Aes myAes = Aes.Create())
                {
                    byte[] encrypted = SymmetricEncryptionHandler.EncryptStringToBytes_Aes(pwm.password, myAes.Key, myAes.IV);

                    pwm.password = Convert.ToBase64String(encrypted);
                    pwm.aesKey = Convert.ToBase64String(myAes.Key);
                    pwm.aesIV = Convert.ToBase64String(myAes.IV);
                }

            }

            myCommand2.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, (object?) pwm.password ?? DBNull.Value);

            myCommand2.Parameters.AddWithValue("@key", NpgsqlTypes.NpgsqlDbType.Varchar, (object?) pwm.aesKey ?? DBNull.Value);

            myCommand2.Parameters.AddWithValue("@iv", NpgsqlTypes.NpgsqlDbType.Varchar, (object?) pwm.aesIV ?? DBNull.Value);

            myReader = await myCommand2.ExecuteReaderAsync();

            return Results.Ok($"User registered! Link ID: {id}");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
}

