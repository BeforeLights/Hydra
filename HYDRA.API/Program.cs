using HYDRA.BLL.Services;
using HYDRA.DAL.Models;           
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HYDRA.API", Version = "v1" });
});
// Đăng ký HydraContext dùng SQL Server
builder.Services.AddDbContext<HydraContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Đăng ký SuggestionService (cần cả HttpClient và DbContext)
builder.Services.AddHttpClient<SuggestionService>();
builder.Services.AddScoped<SuggestionService>();

// HttpClient cho SuggestionService (để gọi RAWG)
builder.Services.AddHttpClient<SuggestionService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HYDRA.API v1"));

app.MapControllers();

app.Run();
