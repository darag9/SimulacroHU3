FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY webProductos.Api/*.csproj ./webProductos.Api/
COPY webProductos.Application/*.csproj ./webProductos.Application/
COPY webProductos.Infrastructure/*.csproj ./webProductos.Infrastructure/
COPY webProductos.Domain/*.csproj ./webProductos.Domain/
COPY webProductos.Tests/*.csproj ./webProductos.Tests/

RUN dotnet restore

COPY . .

WORKDIR "/src/webProductos.Api"
RUN dotnet publish "webProductos.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "webProductos.Api.dll"]