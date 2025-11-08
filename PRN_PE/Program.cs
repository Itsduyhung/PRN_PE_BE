using Microsoft.EntityFrameworkCore;
using PRN_PE.Data;
using PRN_PE.Interfaces;
using PRN_PE.Repositories;
using PRN_PE.Services;
using Microsoft.Extensions.Options; // Cần thiết để sử dụng IOptions và Configure<T>

namespace PRN_PE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // ✅ Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Add DbContext for PostgreSQL
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 💡 BƯỚC ĐÃ THÊM: Đăng ký cấu hình CloudinarySettings
            // Dòng này đọc section "CloudinarySettings" từ appsettings.json
            // và ánh xạ nó vào class CloudinarySettings.
            builder.Services.Configure<CloudinarySettings>(
                builder.Configuration.GetSection("CloudinarySettings"));

            // ✅ Register repositories, services, and interfaces
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();

            // Dòng này đăng ký dịch vụ (Service), nó phải nằm SAU dòng đăng ký cấu hình ở trên.
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

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
        }
    }
}