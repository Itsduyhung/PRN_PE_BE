using Microsoft.EntityFrameworkCore;
using PRN_PE.Data;
using PRN_PE.Interfaces;
using PRN_PE.Repositories;
using PRN_PE.Services;
using Microsoft.Extensions.Options;

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

            // 🚨 SỬA LỖI: Di chuyển Swagger ra khỏi khối IsDevelopment()

            // Dùng Swagger và SwaggerUI trong MỌI môi trường
            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                // Bạn có thể đặt các thiết lập Dev khác ở đây nếu cần, nhưng không cần cho Swagger
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}