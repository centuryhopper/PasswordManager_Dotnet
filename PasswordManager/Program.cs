using PasswordManager;
using PasswordManager.Settings;
// dotnet add package Npgsql


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// looks for all controllers and adds them to the builder for the app
builder.Services.AddControllers();

// builder.Services.Configure<PasswordManagerSettings>(builder.Configuration.GetSection("PostgreSQLSettings"));


var app = builder.Build();

// app.MapControllers();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// for swagger
// app.UseSwaggerUI();
// app.UseSwagger(x => x.SerializeAsV2 = true);

app.UseHttpsRedirection();

app.Run();
