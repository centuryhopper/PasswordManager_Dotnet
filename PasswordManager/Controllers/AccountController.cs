using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IResult Get()
    {
        string query = @"
            select * from test_table
        ";


        var table = new DataTable();
        string sqlDataSource = _configuration.GetConnectionString("postgresqlConnectionString")!;

        NpgsqlDataReader myReader;
        using NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource);
        myCon.Open();

        using NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);
        myReader = myCommand.ExecuteReader();
        table.Load(myReader);

        System.Console.WriteLine(query);

        var builder = new StringBuilder();
        foreach(DataRow dataRow in table.Rows)
        {
            foreach(var item in dataRow.ItemArray)
            {
                Console.WriteLine(item);
                builder.Append(item);
            }
        }

        return Results.Ok(builder.ToString());
    }
}
