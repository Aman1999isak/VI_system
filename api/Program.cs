using ClassLibrary1;
using VI_system;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    VI_system.Ball_detaction r = new VI_system.Ball_detaction();
    r.TestIteration();
     var forecast = Enumerable.Range(1, 19).Select(index =>
       new WeatherForecast
       (
           index,
           r
           
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(int num, VI_system.Ball_detaction e)
{
    public Class1 r = new Class1();
    string b;
    public string v()
    {
        return e.balls[num];
    }
    public string test => v();
}