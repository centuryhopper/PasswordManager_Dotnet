using PasswordManager;
using PasswordManager.Services;
using PasswordManager.Settings;
// postgres support
// dotnet add package Npgsql

// json serialization/deserialization
// dotnet add package Newtonsoft.Json

// .env file loading
// dotnet add package DotNetEnv

// CORS
// dotnet add package Microsoft.AspNet.Cors



var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// looks for all controllers and adds them to the builder for the app
builder.Services.AddControllers();

// builder.Services.Configure<PasswordManagerSettings>(builder.Configuration.GetSection("PostgreSQLSettings"));

builder.Services.AddSingleton<AccountPostgresService>();

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
