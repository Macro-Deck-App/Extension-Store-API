ARG BASE_IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:7.0.5-bullseye-slim-amd64 AS base
WORKDIR /app

FROM ${BASE_IMAGE} AS build
WORKDIR /src/src/ExtensionStoreAPI/

RUN dotnet msbuild "ExtensionStoreAPI.csproj" -warnAsMessage:CS0618 -warnAsError -p:Configuration=Release

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

RUN rm -f Config/config.json
RUN mkdir -p Config

ENTRYPOINT ["dotnet", "ExtensionStoreAPI.dll" ]