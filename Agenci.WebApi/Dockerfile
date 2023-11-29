FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Agenci.WebApi/Agenci.WebApi.csproj", "Agenci.WebApi/"]
RUN dotnet restore "Agenci.WebApi/Agenci.WebApi.csproj"
COPY . .
WORKDIR "/src/Agenci.WebApi"
RUN dotnet build "Agenci.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Agenci.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agenci.WebApi.dll"]
