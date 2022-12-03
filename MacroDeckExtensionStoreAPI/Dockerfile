FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MacroDeckExtensionStoreAPI/MacroDeckExtensionStoreAPI.csproj", "MacroDeckExtensionStoreAPI/"]
RUN dotnet restore "MacroDeckExtensionStoreAPI/MacroDeckExtensionStoreAPI.csproj"
COPY . .
WORKDIR "/src/MacroDeckExtensionStoreAPI"
RUN dotnet build "MacroDeckExtensionStoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MacroDeckExtensionStoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MacroDeckExtensionStoreAPI.dll"]
