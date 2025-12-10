# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas o csproj e restaura (melhora cache)
COPY ["src/01 - WebApi/Employee.WebApi/Employee.WebApi.csproj", "src/01 - WebApi/Employee.WebApi/"]
RUN dotnet restore "src/01 - WebApi/Employee.WebApi/Employee.WebApi.csproj"

# Copia todo o código
COPY . .

# Publica em Release para uma pasta "out"
WORKDIR /src/src/01 - WebApi/Employee.WebApi
RUN dotnet publish "Employee.WebApi.csproj" -c Release -o /app/publish /p:PublishTrimmed=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia do estágio build
COPY --from=build /app/publish ./

# Expõe porta (internamente 8080, você mapeia no docker-compose)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Employee.WebApi.dll"]
