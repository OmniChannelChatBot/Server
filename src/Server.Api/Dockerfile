FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Server/src/Server.Api.csproj", "Server.Api/"]
RUN dotnet restore "Server/src/Server.Api.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.Api.dll"]