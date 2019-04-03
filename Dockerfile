FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY FaviconApi/*.csproj ./FaviconApi/
COPY FaviconApi.Tests/*.csproj ./FaviconApi.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/FaviconApi
RUN dotnet build

# Run tests
FROM build-env AS test
WORKDIR /app/FaviconApi.Tests
RUN dotnet test

# Publish
FROM build-env AS publish
WORKDIR /app/FaviconApi
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=publish /app/FaviconApi/out .
HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "FaviconApi.dll"]
