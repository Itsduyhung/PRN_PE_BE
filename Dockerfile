# ===============================================
# GIAI ĐOẠN 1: BUILD - Biên dịch ứng dụng
# ===============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Đặt thư mục làm việc bên trong container
WORKDIR /src

# Sao chép file project (.csproj) và phục hồi các package dependencies
# Sao chép tất cả các file .csproj (nếu bạn có nhiều project trong solution)
COPY *.csproj .
RUN dotnet restore

# Sao chép tất cả các file còn lại (source code)
COPY . .

# Biên dịch và xuất bản ứng dụng ra thư mục /app/publish
# Thay thế 'PRN_PE.csproj' bằng tên file project của bạn nếu cần
RUN dotnet publish "PRN_PE.csproj" -c Release -o /app/publish --no-restore

# ===============================================
# GIAI ĐOẠN 2: FINAL - Tạo Image Runtime (Tối ưu)
# ===============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Đặt thư mục làm việc cho image cuối cùng
WORKDIR /app

# Sao chép các file đã biên dịch từ giai đoạn 'build'
COPY --from=build /app/publish .

# Cấu hình cổng mà container sẽ lắng nghe (Render yêu cầu cổng 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Điểm khởi đầu của ứng dụng (Tên assembly của bạn)
# Thay thế 'PRN_PE.dll' bằng tên file .dll của project đã publish
ENTRYPOINT ["dotnet", "PRN_PE.dll"]