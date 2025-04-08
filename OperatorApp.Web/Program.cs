using Microsoft.EntityFrameworkCore;
using OperatorApp.Core.Interfaces;
using OperatorApp.Infrastructure.Data;
using OperatorApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("APIClient", client =>
{
    var url = builder.Configuration["API_URL"];
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
