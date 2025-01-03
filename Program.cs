using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("http://localhost:4200")  // Chỉ cho phép truy cập từ Angular
              .AllowAnyMethod()                      // Cho phép mọi phương thức HTTP
              .AllowAnyHeader()                      // Cho phép mọi Header
              .AllowCredentials());                  // Cho phép gửi cookies
});

// Thêm các dịch vụ
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình DbContext với chuỗi kết nối và bật RetryOnFailure
builder.Services.AddDbContext<SanPhamContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("WMT"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));  // Thêm EnableRetryOnFailure

var app = builder.Build();

// Cấu hình pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy"); // Áp dụng CORS
app.UseAuthorization();

// Định tuyến controllers
app.MapControllers(); // Tự động định tuyến tất cả các controllers

app.Run();
