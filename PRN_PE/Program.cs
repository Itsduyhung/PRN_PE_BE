using Microsoft.EntityFrameworkCore;
using PRN_PE.Data;
using PRN_PE.Interfaces;
using PRN_PE.Repositories;
using PRN_PE.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks; // Cần thiết cho Task.CompletedTask

namespace PRN_PE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // ✅ Add Swagger (Đăng ký dịch vụ Swagger)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Add DbContext for PostgreSQL
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 💡 Đăng ký cấu hình CloudinarySettings
            builder.Services.Configure<CloudinarySettings>(
                builder.Configuration.GetSection("CloudinarySettings"));

            // ✅ Register repositories, services, and interfaces
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // 1. Bật Swagger trong MỌI môi trường (khắc phục lỗi 404 trên Render)
            app.UseSwagger();
            app.UseSwaggerUI();

            // 2. Chuyển hướng (Redirect) đường dẫn gốc ("/") đến Swagger UI
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            if (app.Environment.IsDevelopment())
            {
                // Có thể thêm các thiết lập chỉ dành cho Dev ở đây, ví dụ: chi tiết lỗi.
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}