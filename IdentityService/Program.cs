using IdentityService.Extiensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddOpenApi();
services.AddSwaggerGen();

services.RegisterOptions(configuration);
services.AddDatabase(configuration);
services.RegisterServices();

services.AddMappers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();