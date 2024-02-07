FROM mcr.microsoft.com/dotnet/sdk:8.0.101-bookworm-slim-amd64

WORKDIR /src

COPY ./MacroDeckExtensionStoreAPI.sln .
COPY ./Directory.Build.props .
COPY ./Directory.Packages.props .
COPY ./src/ExtensionStoreAPI/ExtensionStoreAPI.csproj src/ExtensionStoreAPI/
COPY ./src/ExtensionStoreAPI.Core/ExtensionStoreAPI.Core.csproj src/ExtensionStoreAPI.Core/

COPY ./tests/ExtensionStoreAPI.Tests.UnitTests/ExtensionStoreAPI.Tests.UnitTests.csproj tests/ExtensionStoreAPI.Tests.UnitTests/

RUN dotnet restore
COPY . /src