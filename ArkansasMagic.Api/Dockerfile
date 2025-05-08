FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ArkansasMagic.Api/ArkansasMagic.Api.csproj", "ArkansasMagic.Api/"]
COPY . .
WORKDIR "/src/ArkansasMagic.Api"
RUN dotnet build "ArkansasMagic.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArkansasMagic.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "ArkansasMagic.Api.dll"]
