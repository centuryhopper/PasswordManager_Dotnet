using PasswordManager.Utils;
using System.Data;
using Newtonsoft.Json;
using Npgsql;
using PasswordManager.Models;
using System.Security.Cryptography;

namespace PasswordManager.Services;

public class AccountPostgresService : IDataAccess<AccountModel>
{
    /*
        create table test_table (
            id varchar(100) PRIMARY KEY NOT NULL,
            title varchar(20),
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

    public AccountPostgresService(IConfiguration configuration)
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

    /// <summary>
    /// read in the entire table in the postgres database
    /// </summary>
    /// <returns></returns>
    public async Task<IResult> Get()
    {
        string query = @"
            select * from test_table;
        ";

        try
        {
            var table = new DataTable();
            // string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;
            string sqlDataSource = postgresqlConnectionString;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);
            myReader = await myCommand.ExecuteReaderAsync();

            table.Load(myReader);

            // decrypt all passwords
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                string key = table.Rows[i].Field<string>("key")!;
                string iv = table.Rows[i].Field<string>("iv")!;
                string password = table.Rows[i].Field<string>("password")!;

                table.Rows[i].SetField("password", SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(Convert.FromBase64String(password), Convert.FromBase64String(key), Convert.FromBase64String(iv)));

                table.AcceptChanges();
            }

            string[] colnames = { "id", "title", "username", "password", "inserteddatetime", "lastmodifieddatetime" };
            DataTable filteredTable = new DataView(table).ToTable(false, colnames);

            // System.Console.WriteLine(JsonConvert.SerializeObject(filteredTable, Formatting.Indented));

            return Results.Ok<string>(JsonConvert.SerializeObject(filteredTable, Formatting.Indented));
        }
        catch (Exception e)
        {
            return Results.BadRequest<string>(e.Message);
        }
    }

    /// <summary>
    /// filter for the desired row by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResult> Get(string id)
    {
        string query = @"
            select * from test_table
            where id = @id;
        ";

        try
        {
            var table = new DataTable();
            // string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;
            string sqlDataSource = postgresqlConnectionString;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, (object)id ?? DBNull.Value);

            myReader = await myCommand.ExecuteReaderAsync();

            table.Load(myReader);

            // foreach (DataColumn column in table.Columns)
            // {
            //     Console.Write("Item: ");
            //     Console.Write(column.ColumnName);
            //     Console.Write(" ");
            // }

            // foreach (DataRow dataRow in table.Rows)
            // {
            //     foreach (var item in dataRow.ItemArray)
            //     {
            //         Console.WriteLine(item);
            //     }
            //     System.Console.WriteLine("-----");
            // }

            string? Id = null, title = null, username = null, password = null, inserteddatetime = null, lastmodifieddatetime = null, key = null, iv = null;

            table.AsEnumerable().Select(
            row => table.Columns.Cast<DataColumn>().ToDictionary(
                column => column.ColumnName,
                column => row[column].ToString()
            )).ToList().ForEach(dict =>
            {
                foreach (var kvp in dict)
                {
                    // System.Console.WriteLine($"key: {kvp.Key}, value: {kvp.Value}");
                    switch (kvp.Key)
                    {
                        case "id":
                            Id = kvp.Value;
                            break;
                        case "title":
                            title = kvp.Value;
                            break;
                        case "username":
                            username = kvp.Value;
                            break;
                        case "password":
                            password = kvp.Value;
                            break;
                        case "inserteddatetime":
                            inserteddatetime = kvp.Value;
                            break;
                        case "lastmodifieddatetime":
                            lastmodifieddatetime = kvp.Value;
                            break;
                        case "key":
                            key = kvp.Value;
                            break;
                        case "iv":
                            iv = kvp.Value;
                            break;
                    }
                }
            });

            password = SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(Convert.FromBase64String(password!), Convert.FromBase64String(key!), Convert.FromBase64String(iv!));

            return Results.Ok(new List<string> { Id!, title!, username!, password, inserteddatetime!, lastmodifieddatetime!, });
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> Post(AccountModel accountModel)
    {
        string query = @"
            insert into test_table(id, title, username, password, key, iv, inserteddatetime, lastmodifieddatetime)
            values (@id, @title, @username, @password, @key, @iv, @inserteddatetime, @lastmodifieddatetime);
        ";



        try
        {
            // string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            string sqlDataSource = postgresqlConnectionString;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            // System.Console.WriteLine(accountModel);

            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, accountModel.accountId ?? Guid.NewGuid().ToString());

            myCommand.Parameters.AddWithValue("@title", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.title ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.username ?? DBNull.Value);

            if (accountModel.password != null)
            {
                using (Aes myAes = Aes.Create())
                {
                    byte[] encrypted = SymmetricEncryptionHandler.EncryptStringToBytes_Aes(accountModel.password, myAes.Key, myAes.IV);

                    accountModel.password = Convert.ToBase64String(encrypted);
                    accountModel.aesKey = Convert.ToBase64String(myAes.Key);
                    accountModel.aesIV = Convert.ToBase64String(myAes.IV);

                    // System.Console.WriteLine($"encrypted password: {accountModel.password}");
                    // System.Console.WriteLine($"encrypted key: {accountModel.aesKey}");
                    // System.Console.WriteLine($"encrypted iv: {accountModel.aesIV}");

                    // System.Console.WriteLine($"decrypted: {SymmetricEncryptionHandler.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV)}");
                }

            }

            myCommand.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.password ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@key", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.aesKey ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@iv", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.aesIV ?? DBNull.Value);

            DateTime myDate = DateTime.ParseExact(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture
            );

            myCommand.Parameters.AddWithValue("@inserteddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@lastmodifieddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            myReader = await myCommand.ExecuteReaderAsync();

            return Results.Ok("post success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message + $" for {accountModel.title}");
        }
    }

    public async Task<IResult> PostMany(List<AccountModel> accountModels)
    {
        try
        {
            await Task.Run(() => accountModels.ForEach(async (model) => await Post(model)));

            return Results.Ok(accountModels);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> Put(AccountModel accountModel)
    {
        string query = @"
            update test_table
            set title=@title,
            username=@username,
            password=@password,
            key=@key,
            iv=@iv,
            lastmodifieddatetime=@lastmodifieddatetime
            where id=@id;
        ";

        // TODO: check if id exists in the database first

        try
        {
            // string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

            string sqlDataSource = postgresqlConnectionString;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            if (accountModel.password != null)
            {
                using (Aes myAes = Aes.Create())
                {
                    byte[] encrypted = SymmetricEncryptionHandler.EncryptStringToBytes_Aes(accountModel.password, myAes.Key, myAes.IV);

                    accountModel.password = Convert.ToBase64String(encrypted);
                    accountModel.aesKey = Convert.ToBase64String(myAes.Key);
                    accountModel.aesIV = Convert.ToBase64String(myAes.IV);
                }

            }

            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, accountModel.accountId!);

            myCommand.Parameters.AddWithValue("@title", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.title ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@username", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.username ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.password ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@key", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.aesKey ?? DBNull.Value);

            myCommand.Parameters.AddWithValue("@iv", NpgsqlTypes.NpgsqlDbType.Varchar, (object?)accountModel.aesIV ?? DBNull.Value);

            DateTime myDate = DateTime.ParseExact(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture
            );

            myCommand.Parameters.AddWithValue("@lastmodifieddatetime", NpgsqlTypes.NpgsqlDbType.Timestamp, (object?)myDate ?? DBNull.Value);

            myReader = await myCommand.ExecuteReaderAsync();

            return Results.Ok("update success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> Delete(string id)
    {
        string query = @"
            delete from test_table
            where id=@id";

        try
        {
            // string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;
            string sqlDataSource = postgresqlConnectionString;

            NpgsqlDataReader myReader;
            using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
            myCon.Open();
            using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);

            // configure parameters
            myCommand.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Varchar, id);

            myReader = await myCommand.ExecuteReaderAsync();

            return Results.Ok("delete success!");
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    public Task<int> Commit()
    {
        return Task.Run(() => 0);
    }

}

