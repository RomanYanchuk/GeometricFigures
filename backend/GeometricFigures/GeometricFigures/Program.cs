using GeometricFigures.Middleware;
using GeometricFigures.Services;
using GeometricFigures.Storages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IFiguresService, FiguresService>();
builder.Services.AddTransient<IFiguresMappingFactory, FiguresMappingFactory>();
builder.Services.AddTransient<IFiguresContractReconstructionFactory, FiguresContractReconstructionFactory>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<FiguresStorage>(
    options => options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 31)),
        mySqlOptions =>
        {
            mySqlOptions.DisableBackslashEscaping();
            mySqlOptions.EnableRetryOnFailure(
                15,
                TimeSpan.FromMilliseconds(2000),
                null!);
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#if DEBUG
app.UseMiddleware<RequestLogger>();
#endif

app.UseMiddleware<ScopedLogger>();

app.UseMiddleware<ExceptionsLogger>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
