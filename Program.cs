////using Microsoft.EntityFrameworkCore;
////using Microsoft.Extensions.DependencyInjection;
////using Newtonsoft.Json.Serialization;

////using WBMT.Data;
////var builder = WebApplication.CreateBuilder(args);

//////builder.Services.AddDbContext<WBMTContext>(options =>
//////    options.UseSqlServer(builder.Configuration.GetConnectionString("WBMTContext") ?? throw new InvalidOperationException("Connection string 'WBMTContext' not found.")));
////builder.Services.AddCors(c => {
////    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader()
////    .AllowAnyMethod());
////});
////// Add services to the container.
////builder.Services.AddControllersWithViews()
////.AddNewtonsoftJson(options =>
////options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
////.Json.ReferenceLoopHandling.Ignore)
////.AddNewtonsoftJson(options =>
////options.SerializerSettings.ContractResolver = new DefaultContractResolver());


////builder.Services.AddControllers();
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

////var app = builder.Build();
//////if (app.Environment.IsDevelopment())
//////{
//////    app.UseDeveloperExceptionPage();
//////}
//////else
//////{
//////    app.UseExceptionHandler("/Home/Error");
//////    app.UseHsts();
//////}


////app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader()
////    .AllowAnyMethod());

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

////app.Run();
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json.Serialization;
//using WBMT.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Kết nối DbContext
//builder.Services.AddDbContext<WBMTContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("WBMT")
//    ?? throw new InvalidOperationException("Connection string 'WBMT' not found.")));

//// Cấu hình CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowOrigin",
//        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

//// Cấu hình dịch vụ JSON và tùy chọn Newtonsoft
//builder.Services.AddControllersWithViews()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
//    });

//// Thêm dịch vụ controllers
//builder.Services.AddControllers();

//// Cấu hình Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Áp dụng chính sách CORS cho tất cả các yêu cầu
//app.UseCors("AllowOrigin");

//// Sử dụng Swagger trong môi trường phát triển
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Middleware
//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//app.UseCors("AllowOrigin");


//app.Run();
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using WBMT.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WBMTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WBMT")
    ?? throw new InvalidOperationException("Connection string 'WBMT' not found.")));


// Add services to the container.
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader()
   .AllowAnyMethod());
});
// JSON Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft
.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(option =>
    option.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseRouting();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader()
    .AllowAnyMethod());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
//    RequestPath = "/Photos"
//});

app.Run();
