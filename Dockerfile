FROM microsoft/dotnet:sdk AS build-env
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
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/FaviconApi/out .
HEALTHCHECK CMD curl --fail http://localhost:5000/healthcheck || exit
ENTRYPOINT ["dotnet", "FaviconApi.dll"]
