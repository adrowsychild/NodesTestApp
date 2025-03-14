using DAL.Implementation;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using NodesTestApp.Middlewares;
using Services.Implementation;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITreeRepository, TreeRepository>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IJournalRepository, JournalRepository>();

builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<ILogExceptionService, LogExceptionService>();
builder.Services.AddScoped<IJournalService, JournalService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionLoggingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
