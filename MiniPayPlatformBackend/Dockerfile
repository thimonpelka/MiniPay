FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MiniPay.Application/MiniPay.Application.csproj", "MiniPay.Application/"]

# Restore dependencies
RUN dotnet restore "MiniPay.Application/MiniPay.Application.csproj"

# Copy the project files and build the application
COPY . .
WORKDIR "/src/MiniPay.Application"
RUN dotnet build "MiniPay.Application.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MiniPay.Application.csproj" -c Release -o /app/publish

# Final stage to create the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "MiniPay.Application.dll"]
