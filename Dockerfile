# ===============================
# STAGE 1: BUILD
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file csproj đúng đường dẫn
COPY PRN_PE/PRN_PE.csproj PRN_PE/

# Restore
RUN dotnet restore "PRN_PE/PRN_PE.csproj"

# Copy toàn bộ source của project đúng thư mục
COPY PRN_PE/ PRN_PE/

# Publish
RUN dotnet publish "PRN_PE/PRN_PE.csproj" -c Release -o /app/publish --no-restore

# ===============================
# STAGE 2: RUNTIME
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render yêu cầu cổng 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PRN_PE.dll"]
