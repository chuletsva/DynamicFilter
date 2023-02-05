using DynamicFilter;
using DynamicFilter.Models;
using Microsoft.AspNetCore.Mvc;
using Web.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(x => new AppDbContextFactory().CreateDbContext());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Web
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public ProductsController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("filter")]
        public IActionResult GetFiltered([FromBody] Operation[] filter)
        {
            return Ok(_dbcontext.Set<Product>().ApplyDynamicFilter(filter));
        }
    }
}