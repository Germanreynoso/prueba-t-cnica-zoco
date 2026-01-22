# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY ["BackendApi/BackendApi.csproj", "BackendApi/"]
RUN dotnet restore "BackendApi/BackendApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/source/BackendApi"
RUN dotnet publish "BackendApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 (Render uses this by default)
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "BackendApi.dll"]
