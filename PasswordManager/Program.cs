using PasswordManager.Services;

// postgres support
// dotnet add package Npgsql

// json serialization/deserialization
// dotnet add package Newtonsoft.Json

// .env file loading
// dotnet add package DotNetEnv

// CORS
// dotnet add package Microsoft.AspNet.Cors

// JWT
// dotnet add package Microsoft.IdentityModel.Tokens
// dotnet add package System.IdentityModel.Tokens.Jwt



var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// looks for all controllers and adds them to the builder for the app
builder.Services.AddControllers();

// builder.Services.Configure<PasswordManagerSettings>(builder.Configuration.GetSection("PostgreSQLSettings"));

builder.Services.AddSingleton<AccountPostgresService>();
builder.Services.AddSingleton<PasswordManagerPostgresService>();


// allow client-side apps to fetch data from this api
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build => {
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// app.MapControllers();

app.UseRouting();

// allow client-side apps to fetch data from this api
app.UseCors(builder =>
    builder.AllowAnyOrigin().
            AllowAnyHeader().
            AllowAnyMethod()
);

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// app.UseDefaultFiles();
// app.UseStaticFiles();

// for swagger
// app.UseSwaggerUI();
// app.UseSwagger(x => x.SerializeAsV2 = true);

app.UseHttpsRedirection();

app.Run();


/*
One way to link an authenticated user to the proper table in a database is to create a separate table for each user, and name the table using the user's ID or username. This way, each user would have their own table that only they have access to, and their passwords would be stored in their specific table.

When a user signs up, you could create a new table with the user's ID or username as the table name, and store the user's basic information, such as their username and password hash.

When a user logs in, you would first check their credentials against the main users table. If the credentials match, you would then retrieve the user's ID or username, and use that information to construct the name of the table where the user's passwords are stored. You can then query that specific table to retrieve the user's passwords.

Another approach could be to have a single table that stores all the passwords , instead of creating new table for each user. But in this case, you would need to have a way to link a password to a specific user. One way to do this is to add a "user_id" column to the table, and store the ID of the user that owns each password in that column. Then, when you query the table to retrieve a user's passwords, you would use the user's ID to filter the results. (THIS APPROACH HAS MY VOTE)

It's also important to encrypt the password data, stored in the table and not to store it as plain text.

It's also important to consider security and the risk of SQL injection attacks. To prevent them, you should use parameterized queries to insert and retrieve data from the database, rather than concatenating user input with SQL strings.

*/
